﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
namespace MidtermComGame;

public class MainScene 
{
    // private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    SpriteFont _font;

    List<GameObject> _gameObjects;
    int _numObject;
    Texture2D _backgroundTexture;
    Texture2D _chipTexture;
    Texture2D _chipStickTexture;
    Texture2D _handTexture;
    Texture2D _rectTexture;
    Texture2D _cannonTexture;
    Texture2D _ShopTexture;
    SoundEffect _ceilingPushingSound;
    SoundEffect _chipHitSound;
    Song _gameMusic;
    Shop _shop;

    public void Initialize()
    {
        _gameObjects = new List<GameObject>();
        Singleton.Instance.GameBoard = new GameBoard(Singleton.CHIP_GRID_HEIGHT, Singleton.CHIP_GRID_WIDTH);
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
        _font = content.Load<SpriteFont>("GameFont");

        _backgroundTexture = content.Load<Texture2D>("Background");

        _chipTexture = content.Load<Texture2D>("Chips");
        _chipStickTexture = content.Load<Texture2D>("ChipStick");
        _handTexture = content.Load<Texture2D>("Hand");

        _cannonTexture = content.Load<Texture2D>("Cannon");
        _ShopTexture = content.Load<Texture2D>("Shop");
        _rectTexture = new Texture2D(graphicsDevice, 3, 640);
        Color[] data = new Color[3 * 640];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _rectTexture.SetData(data);

        _ceilingPushingSound = content.Load<SoundEffect>("Ceilingpushing");
        _chipHitSound = content.Load<SoundEffect>("ChipHit");
        _gameMusic = content.Load<Song>("A Night Alone - TrackTribe");

        Reset();
    }

    public void Update(GameTime gameTime)
    {
        Singleton.Instance.CurrentKey = Keyboard.GetState();

        _numObject = _gameObjects.Count;

        switch (Singleton.Instance.CurrentGameState)
        {
            case Singleton.GameState.SetLevel:
                SetUpInitalChipsPattern();

                Singleton.Instance.CurrentChip = Singleton.Instance.GameBoard.GetRandomChipColor();
                Singleton.Instance.NextChip = Singleton.Instance.GameBoard.GetRandomChipColor();

                Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;

                break;
            case Singleton.GameState.Playing:
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(_gameMusic);
                }
                for (int i = 0; i < _numObject; i++)
                {
                    if(_gameObjects[i].IsActive)
                        _gameObjects[i].Update(gameTime, _gameObjects);
                }
                for (int i = 0; i < _numObject; i++)
                {
                    if(!_gameObjects[i].IsActive)
                    {
                    _gameObjects.RemoveAt(i);
                    i--;
                    _numObject--;
                    }
                }
                break;
            case Singleton.GameState.CheckChipAndCeiling:
                CheckAndDestroyHangingChips();
                Singleton.Instance.NextChip = Singleton.Instance.GameBoard.GetRandomChipColor();
                CheckAndPushDownCeiling();
                CheckGameOver();
                break;
            case Singleton.GameState.GameOver:
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Stop();
                }
                break;
        }

        Singleton.Instance.PreviousKey = Singleton.Instance.CurrentKey;

    }

    public void Draw(GameTime gameTime)
    {
        _numObject = _gameObjects.Count;

        _spriteBatch.Draw(_backgroundTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

        _spriteBatch.Draw(_chipStickTexture, new Vector2(Singleton.PLAY_AREA_START_X, -_chipStickTexture.Height + Singleton.Instance.CeilingPosition),
        null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

        for (int i = 0; i < _numObject; i++)
        {
            _gameObjects[i].Draw(_spriteBatch);
        }
        
        //Next Chip Display 
        // Red blue green Yellow
        // 0 1 2 3
        // _spriteBatch.Draw(_chipTexture,new Vector2(Singleton.SCREEN_WIDTH / 8, 400),Singleton.GetChipColor(Singleton.Instance.NextChip));

        int chipIndex =0;
        switch (Singleton.Instance.NextChip)
        {
            case ChipType.Red: 
                chipIndex =0;
                break;
            case ChipType.Blue: 
                chipIndex =1;
                break;
            case ChipType.Green: 
                chipIndex =2;
                break;
            case ChipType.Yellow: 
                chipIndex =3;
                break;
            default:
                break;
        }
        
        // Draw the chip using the sourceRectangle
        _spriteBatch.Draw(_chipTexture, new Vector2(Singleton.SCREEN_WIDTH / 8, 400), 
            new Rectangle(chipIndex * Singleton.CHIP_SIZE, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT),Color.White); 

        //Game Over Line
        //_spriteBatch.Draw(_rectTexture, new Vector2(0, Singleton.CHIP_GRID_HEIGHT * Singleton.CHIP_SIZE), null, Color.White, (float) (3*Math.PI/2), Vector2.Zero, 1, SpriteEffects.None, 0f);

        Vector2 fontSize = _font.MeasureString("Score : " + Singleton.Instance.Score.ToString());
        _spriteBatch.DrawString(_font,
            "Score : " + Singleton.Instance.Score.ToString(),
            new Vector2((Singleton.SCREEN_WIDTH / 4 - fontSize.X) / 2, 30),
            Color.White);


    }

    protected void Reset()
    {
        Singleton.Instance.GameBoard.ClearBoard();

        Singleton.Instance.Random = new System.Random();

        Singleton.Instance.CeilingPosition = 0;
        Singleton.Instance.ChipShotAmount = 0;
        Singleton.Instance.Score = 0;

        Singleton.Instance.CurrentGameState = Singleton.GameState.SetLevel;

        // Texture2D cannonTexture = content.Load<Texture2D>("Cannon");

        _gameObjects.Add(new Player(_handTexture)
        {
            Name = "Player",
            Viewport = new Rectangle(0, 0, _handTexture.Width, _handTexture.Height),
            Position = new Vector2(Singleton.SCREEN_WIDTH / 2, Singleton.CHIP_SHOOTING_HEIGHT),
            Left = Keys.Left,
            Right = Keys.Right,
            Fire = Keys.Space,
            Chip = new Chip(_chipTexture)
            {
                Name = "Chip",
                _isShot = false,
                Viewport = new Rectangle(0, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT), 
                ChipHitSound = _chipHitSound,
                Speed = 0,
                Score = 10
            },
            ExplosiveChip = new ExplosiveChip(_chipTexture)
            {
                Name = "ExplosiveChi[]",
                _isShot = false,
                Viewport = new Rectangle(0, 0, Singleton.CHIP_SIZE, Singleton.CHIP_SIZE + Singleton.CHIP_SHADOW_HEIGHT), 
                ChipHitSound = _chipHitSound,
                Speed = 0,
                Score = 10
            }
        });

        //add shop content
        _shop = new Shop(_ShopTexture){
            Name = "Shop",
            Position = new Vector2(Singleton.SCREEN_WIDTH *3/4 ,30)
        };
        // _shop.AddItems   
        RedChip redChip = new RedChip(_chipTexture){
            Viewport = Singleton.GetChipViewPort(ChipType.Red),
            Price = 50,
            BuyKey = Keys.A
        };
        _shop.AddShopItem(redChip);
        BlueChip blueChip = new BlueChip(_chipTexture){
            Viewport = Singleton.GetChipViewPort(ChipType.Blue),
            Price = 50,
            BuyKey = Keys.S
        };
        _shop.AddShopItem(blueChip);
        GreenChip greenChip = new GreenChip(_chipTexture){
            Viewport = Singleton.GetChipViewPort(ChipType.Green),
            Price = 50,
            BuyKey = Keys.D
        };
        _shop.AddShopItem(greenChip);
        YellowChip yellowChip = new YellowChip(_chipTexture){
            Viewport = Singleton.GetChipViewPort(ChipType.Yellow),
            Price = 50,
            BuyKey = Keys.F
        };
        _shop.AddShopItem(yellowChip);
        ExplosiveChipItem ExplosiveChip = new ExplosiveChipItem(_chipTexture){
            Viewport = Singleton.GetChipViewPort(ChipType.Explosive),
            Price = 50,
            BuyKey = Keys.Q
        };
        _shop.AddShopItem(ExplosiveChip);



        _gameObjects.Add(_shop);

        foreach (GameObject s in _gameObjects)
        {
            s.Reset();
        }
    }

    protected void SetUpInitalChipsPattern()
    {
        //temp pattern
        Singleton.Instance.GameBoard[0, 0] = ChipType.Red;
        Singleton.Instance.GameBoard[0, 1] = ChipType.Red;
        Singleton.Instance.GameBoard[0, 2] = ChipType.Yellow;
        Singleton.Instance.GameBoard[0, 3] = ChipType.Yellow;
        Singleton.Instance.GameBoard[0, 4] = ChipType.Blue;
        Singleton.Instance.GameBoard[0, 5] = ChipType.Blue;
        Singleton.Instance.GameBoard[0, 6] = ChipType.Green;
        Singleton.Instance.GameBoard[0, 7] = ChipType.Green;

        Singleton.Instance.GameBoard[1, 0] = ChipType.Red;
        Singleton.Instance.GameBoard[1, 1] = ChipType.Red;
        Singleton.Instance.GameBoard[1, 2] = ChipType.Yellow;
        Singleton.Instance.GameBoard[1, 3] = ChipType.Yellow;
        Singleton.Instance.GameBoard[1, 4] = ChipType.Blue;
        Singleton.Instance.GameBoard[1, 5] = ChipType.Blue;
        Singleton.Instance.GameBoard[1, 6] = ChipType.Green;

        Singleton.Instance.GameBoard[2, 0] = ChipType.Blue;
        Singleton.Instance.GameBoard[2, 1] = ChipType.Blue;
        Singleton.Instance.GameBoard[2, 2] = ChipType.Green;
        Singleton.Instance.GameBoard[2, 3] = ChipType.Green;
        Singleton.Instance.GameBoard[2, 4] = ChipType.Red;
        Singleton.Instance.GameBoard[2, 5] = ChipType.Red;
        Singleton.Instance.GameBoard[2, 6] = ChipType.Yellow;
        Singleton.Instance.GameBoard[2, 7] = ChipType.Yellow;

        Singleton.Instance.GameBoard[3, 0] = ChipType.Blue;
        Singleton.Instance.GameBoard[3, 1] = ChipType.Green;
        Singleton.Instance.GameBoard[3, 2] = ChipType.Green;
        Singleton.Instance.GameBoard[3, 3] = ChipType.Red;
        Singleton.Instance.GameBoard[3, 4] = ChipType.Red;
        Singleton.Instance.GameBoard[3, 5] = ChipType.Yellow;
        Singleton.Instance.GameBoard[3, 6] = ChipType.Yellow;

        for (int j = 0; j < Singleton.CHIP_GRID_HEIGHT; j++)
        {
            for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
            {
                if(Singleton.Instance.GameBoard[j, i] != ChipType.None)
                    AddChipToBoard(i, j);
            }
        }
    }

    protected void AddChipToBoard(int i, int j)
    {
        int offSetX = (j % 2 == 1) ? Singleton.CHIP_SIZE / 2 : 0;
        Chip newChip = new Chip(_chipTexture)
        {
            Name = "Chip",
            _isShot = true,
            Position = new Vector2(Singleton.PLAY_AREA_START_X + i * Singleton.CHIP_SIZE + offSetX, j * Singleton.CHIP_SIZE),
            ChipHitSound = _chipHitSound,
            ChipType = Singleton.Instance.GameBoard[j, i],
            Score = 10
        };

        newChip.Reset();
        newChip.BoardCoord =  new Vector2(i, j);

        _gameObjects.Add(newChip);
    }

    protected void CheckAndPushDownCeiling()
    {
        if(Singleton.Instance.ChipShotAmount % Singleton.CEILING_WAITING_TURN == 0){

            _numObject = _gameObjects.Count;

            for (int i = 0; i < _numObject; i++)
            {
                if(_gameObjects[i].Name.Contains("Chip"))
                {
                    _gameObjects[i].Position.Y += Singleton.CHIP_SIZE;
                }
            }

            Singleton.Instance.CeilingPosition += Singleton.CHIP_SIZE;
            _ceilingPushingSound.Play();
        }
    }

    protected void CheckAndDestroyHangingChips()
    {
        for (int j = 1; j < Singleton.CHIP_GRID_HEIGHT; j++)
        {
            for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
            {
                //skip last column
                if(Singleton.Instance.GameBoard.IsUnUseSpot(j, i))
                    continue;
                if(Singleton.Instance.GameBoard[j, i] == ChipType.None)
                    continue;

                List<Vector2> AdjacentChips = new List<Vector2>();

                CheckHighestHangingChips(new Vector2(i, j), AdjacentChips);
                int highestRow = Singleton.CHIP_GRID_HEIGHT;

                foreach (Vector2 b in AdjacentChips)
                {
                    if(b.Y < highestRow) highestRow = (int) b.Y;
                }

                if(highestRow != 0)
                    DestroyChips(AdjacentChips);
            }
        }
    }

    private void CheckHighestHangingChips(Vector2 boardCoord, List<Vector2> AdjacentChips)
    {
        if(AdjacentChips.Contains(boardCoord))
            return;

        int X = (int)boardCoord.X;
        int Y = (int)boardCoord.Y;

        AdjacentChips.Add(new Vector2(X, Y));

        if(HaveChip(X-1, Y)) CheckHighestHangingChips(new Vector2(X-1, Y), AdjacentChips);
        if(HaveChip(X+1, Y)) CheckHighestHangingChips(new Vector2(X+1, Y), AdjacentChips);
        if(HaveChip(X, Y-1)) CheckHighestHangingChips(new Vector2(X, Y-1), AdjacentChips);
        if(HaveChip(X, Y+1)) CheckHighestHangingChips(new Vector2(X, Y+1), AdjacentChips);

        bool isOddRow = (Y % 2 == 1);
        
        if (isOddRow)
        {
            if(HaveChip(X+1, Y-1)) CheckHighestHangingChips(new Vector2(X+1, Y-1), AdjacentChips);
            if(HaveChip(X+1, Y+1)) CheckHighestHangingChips(new Vector2(X+1, Y+1), AdjacentChips);
        }
        else
        {
            if(HaveChip(X-1, Y-1)) CheckHighestHangingChips(new Vector2(X-1, Y-1), AdjacentChips);
            if(HaveChip(X-1, Y+1)) CheckHighestHangingChips(new Vector2(X-1, Y+1), AdjacentChips);
        }
    }

    protected bool HaveChip(int x, int y)
    {
        if (Singleton.Instance.GameBoard.IsInsideBounds(y, x))
        {
            return Singleton.Instance.GameBoard[y, x] != ChipType.None;
        }
        return false;
    }

    protected void DestroyChips(List<Vector2> AdjacentChips)
    {
        for (int i = 0; i < AdjacentChips.Count; i++)
        {
            Singleton.Instance.GameBoard[(int)AdjacentChips[i].Y, (int)AdjacentChips[i].X] = ChipType.None;
            _numObject = _gameObjects.Count;
            for (int j = 0; j < _numObject; j++)
            {
                if(_gameObjects[j] is Chip && (_gameObjects[j] as Chip).BoardCoord == AdjacentChips[i])
                {
                    _gameObjects[j].IsActive = false;
                    break;
                }
            }
        }
        //Score
        Console.WriteLine(AdjacentChips.Count);
        Singleton.Instance.Score += (int)(10 * Math.Pow(2, AdjacentChips.Count));
        Console.WriteLine((int)(10 * Math.Pow(2, AdjacentChips.Count)));
    }

    protected void CheckGameOver()
    {
        Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;

        int ceilingPushedAmount = Singleton.Instance.ChipShotAmount / Singleton.CEILING_WAITING_TURN;

        for (int i = 0; i < Singleton.CHIP_GRID_WIDTH; i++)
        {
            for (int j = Singleton.CHIP_GRID_HEIGHT - ceilingPushedAmount; j < Singleton.CHIP_GRID_HEIGHT; j++)
            {
                if(Singleton.Instance.GameBoard[j, i] != ChipType.None)
                {
                    Singleton.Instance.CurrentGameState = Singleton.GameState.GameOver;
                    return;
                }
            }
        }
    }
}
