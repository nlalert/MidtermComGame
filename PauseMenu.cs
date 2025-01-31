using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace MidtermComGame;

public class PauseMenu
{
    private SpriteBatch _spriteBatch;

    private Texture2D _texture;
    private Texture2D _rectTexture;

    private Button _resumeButton;
    private Button _restartButton;
    private Button _settingsButton;
    private Button _mainmenuButton;

    private Button _musicSlideChip;
    private Button _sfxSlideChip;
    private Button _backButton;

    private int _pauseTitleHeight;
    private int _resumeButtonHeight;
    private int _settingsButtonHeight;
    private int _restartButtonHeight;
    private int _mainmenuButtonHeight;

    private int _musicLabelHeight;
    private int _musicSlideBarHeight;
    private int _musicSlideChipHeight;
    private int _sfxLabelHeight;
    private int _sfxSlideBarHeight;
    private int _sfxSlideChipHeight;
    private int _backButtonHeight;

    private float _musicSlideChipPosition;
    private float _sfxSlideChipPosition;

    private int _slideBarMaxValue;
    private int buttonGap;
    private bool _settings;

    public void Initialize()
    {
        Console.WriteLine("Paused");

        _settings = false;

        _slideBarMaxValue = 320;

        // Y positon of the pause sign
        _pauseTitleHeight = 70;

        // Y positon of the resume button
        _resumeButtonHeight = 180;

        // a gap between each button
        buttonGap = 5;

        // Calculating Y position of other buttons
        for (int i = 0; i < 4; i++){
            switch (i)
            {
                case 1:
                    _restartButtonHeight = _resumeButtonHeight + (buttonGap + Singleton.GetViewPortFromSpriteSheet("Pause_Button").Height)*i;
                    break;
                case 2:
                    _settingsButtonHeight = _resumeButtonHeight + (buttonGap + Singleton.GetViewPortFromSpriteSheet("Pause_Button").Height)*i;                
                    break;
                case 3:
                    _mainmenuButtonHeight = _resumeButtonHeight + (buttonGap + Singleton.GetViewPortFromSpriteSheet("Pause_Button").Height)*i;
                    break;    
                default:
                    break;
            }
        }

        _musicLabelHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) - (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Height/4) - 
        (Singleton.GetViewPortFromSpriteSheet("Slide_Bar").Height/2) - buttonGap;
        _sfxLabelHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) + (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Height/12) - 
        (Singleton.GetViewPortFromSpriteSheet("Slide_Bar").Height/2) - buttonGap;

        _musicSlideBarHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.GetViewPortFromSpriteSheet("Slide_Bar").Height / 2) - (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Height/4) + buttonGap;
        _sfxSlideBarHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.GetViewPortFromSpriteSheet("Slide_Bar").Height / 2) + (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Height/12) + buttonGap;

        _musicSlideChipHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) - (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Height/4) + buttonGap;
        _sfxSlideChipHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.CHIP_SIZE / 2) + (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Height/12) + buttonGap;

        _backButtonHeight = (Singleton.SCREEN_HEIGHT / 2) - (Singleton.GetViewPortFromSpriteSheet("Back_Button").Height / 2) + (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Height*2/5);
    }

    public void LoadContent(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {

        _spriteBatch = spriteBatch;

        _texture = content.Load<Texture2D>("Sprite_Sheet");

        _rectTexture = new Texture2D(graphicsDevice, 1, 1);
        Color[] data = new Color[1 * 1];
        for (int i = 0; i < data.Length; i++) data[i] = Color.White;
        _rectTexture.SetData(data);

         _resumeButton = new Button(_texture)
        {
            Name = "ResumeButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Pause_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Pause_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Pause_Button").Width) / 2,
                    _resumeButtonHeight - Singleton.GetViewPortFromSpriteSheet("Pause_Button").Height / 2),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("Resume_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("Resume_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Resume_Label").Width) / 2,
                    _resumeButtonHeight - Singleton.GetViewPortFromSpriteSheet("Resume_Label").Height / 2),
            IsActive = true
        };

        _restartButton = new Button(_texture)
        {
            Name = "RestartButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Pause_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Pause_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Pause_Button").Width) / 2,
                    _restartButtonHeight - Singleton.GetViewPortFromSpriteSheet("Pause_Button").Height / 2),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("Restart_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("Restart_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Restart_Label").Width) / 2,
                    _restartButtonHeight - Singleton.GetViewPortFromSpriteSheet("Restart_Label").Height / 2),
            IsActive = true
        };

        _settingsButton = new Button(_texture)
        {
            Name = "SettingsButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Pause_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Pause_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Pause_Button").Width) / 2,
                    _settingsButtonHeight - Singleton.GetViewPortFromSpriteSheet("Pause_Button").Height / 2),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("Settings_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("Settings_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Settings_Label").Width) / 2,
                    _settingsButtonHeight - Singleton.GetViewPortFromSpriteSheet("Settings_Label").Height / 2),
            IsActive = true
        };

        _mainmenuButton = new Button(_texture)
        {
            Name = "MainmenuButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Pause_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Pause_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Pause_Button").Width) / 2,
                    _mainmenuButtonHeight - Singleton.GetViewPortFromSpriteSheet("Pause_Button").Height / 2),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("Resume_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("Resume_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Resume_Label").Width) / 2,
                    _mainmenuButtonHeight - Singleton.GetViewPortFromSpriteSheet("Resume_Label").Height / 2),
            IsActive = true
        };

        _musicSlideChip = new Button(_texture)
        {
            Name = "MusicSlideChip",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip0"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Slide_Chip0").Width) / 2 + (int)(Singleton.Instance.Volume*_slideBarMaxValue) - (_slideBarMaxValue/2),
                    _musicSlideChipHeight),
            IsActive = true
        };

        _sfxSlideChip = new Button(_texture)
        {
            Name = "SFXSlideChip",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip0"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Slide_Chip0").Width) / 2 + (int)(Singleton.Instance.Volume*_slideBarMaxValue) - (_slideBarMaxValue/2),
                    _sfxSlideChipHeight),
            IsActive = true
        };

        _backButton = new Button(_texture)
        {
            Name = "BackButton",
            Viewport = Singleton.GetViewPortFromSpriteSheet("Back_Button"),
            HighlightedViewPort = Singleton.GetViewPortFromSpriteSheet("Back_Button_Highlighted"),
            Position = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Back_Button").Width) / 2,
                    _backButtonHeight - Singleton.GetViewPortFromSpriteSheet("Back_Button").Height / 2),
            LabelViewPort = Singleton.GetViewPortFromSpriteSheet("Back_Label"),
            HighlightedLabelViewPort = Singleton.GetViewPortFromSpriteSheet("Back_Label_Highlighted"),
            LabelPosition = new Vector2((Singleton.SCREEN_WIDTH - Singleton.GetViewPortFromSpriteSheet("Back_Label").Width) / 2,
                    _backButtonHeight - Singleton.GetViewPortFromSpriteSheet("Back_Label").Height / 2),
            IsActive = true
        };

    }

    public void Update(GameTime gameTime)
    {
    
        if (!_settings)
        {

            // Unpause when left clicked & released on resume button or pressed & released "Escape key"
            if (_resumeButton.IsClicked() || (Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)){
                 Singleton.Instance.CurrentGameState = Singleton.GameState.Playing;
            }

            // Restart to stage 1 when left clicked & released on restart button
            if (_restartButton.IsClicked())
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.StartingGame;
            }

            // Show settings when left clicked & released on settings button
            if (_settingsButton.IsClicked())
            {
                _settings = true;
            }

            // Exit to main-menu when left clicked & released on main-menu button
            if (_mainmenuButton.IsClicked())
            {
                Singleton.Instance.CurrentGameState = Singleton.GameState.MainMenu;
            }

        }

        else
        {

            // Exit to pause menu when left clicked & released on back cutton or pressed & released "Escape key"
            if (_backButton.IsClicked() || Singleton.Instance.CurrentKey.IsKeyDown(Keys.Escape) && Singleton.Instance.CurrentKey != Singleton.Instance.PreviousKey)
            {
                _settings = false;
            }

            // Prevent dragging boyh chips at the same time
            if (!_sfxSlideChip.Dragging)
            {
            _musicSlideChip.ButtonUpdate();
            }
            if (!_musicSlideChip.Dragging)
            {
            _sfxSlideChip.ButtonUpdate();
            }

            // Adjust music volume by left click and drag the music slide chip
            if (_musicSlideChip.Dragging)
            {

                int newX = Math.Clamp(Singleton.Instance.CurrentMouseState.X - (_musicSlideChip.Viewport.Width / 2), (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - _slideBarMaxValue/2, (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + _slideBarMaxValue/2);
                _musicSlideChip.Position.X = newX;

                float _slideBarMinPosition = (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - (_slideBarMaxValue/2);
                _musicSlideChipPosition = _musicSlideChip.Position.X - _slideBarMinPosition;
                Singleton.Instance.Volume = _musicSlideChipPosition / _slideBarMaxValue;

            }
            
            // Adjust sfx volume by left click and drag the sfx slide chip
            if (_sfxSlideChip.Dragging)
            {

                int newX = Math.Clamp(Singleton.Instance.CurrentMouseState.X - (_sfxSlideChip.Viewport.Width / 2), (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - _slideBarMaxValue/2, (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) + _slideBarMaxValue/2);
                _sfxSlideChip.Position.X = newX;

                float _slideBarMinPosition = (Singleton.SCREEN_WIDTH / 2) - (Singleton.CHIP_SIZE/ 2) - (_slideBarMaxValue/2);
                _sfxSlideChipPosition = _sfxSlideChip.Position.X - _slideBarMinPosition;
                //Singleton.Instance.Volume = _sfxSlideChipPosition / _slideBarMaxValue;

            }

        }
    }

    public void Draw(GameTime gameTime)
    {
        // Tranparent background
        _spriteBatch.Draw(_rectTexture, Vector2.Zero, new Rectangle(0, 0, Singleton.SCREEN_WIDTH, Singleton.SCREEN_HEIGHT), new Color(0, 0, 0, 150));

        if (!_settings)
        {         
            // Pause Title
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (Singleton.GetViewPortFromSpriteSheet("Pause_Title0").Width / 2), 
            _pauseTitleHeight - (Singleton.GetViewPortFromSpriteSheet("Pause_Title0").Height / 2)), Singleton.GetViewPortFromSpriteSheet("Pause_Title0"), Color.White);

            // Buttons
            _resumeButton.Draw(_spriteBatch);
            _restartButton.Draw(_spriteBatch);
            _settingsButton.Draw(_spriteBatch);
            _mainmenuButton.Draw(_spriteBatch);

        }

        else {

            // Settings Box
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Width/ 2),
             (Singleton.SCREEN_HEIGHT / 2) - (Singleton.GetViewPortFromSpriteSheet("Settings_Box0").Height/ 2)), Singleton.GetViewPortFromSpriteSheet("Settings_Box0"), Color.White);
            
            // Music
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (Singleton.GetViewPortFromSpriteSheet("Music_Label").Width/ 2),
            _musicLabelHeight), Singleton.GetViewPortFromSpriteSheet("Music_Label"), Color.White);

            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (Singleton.GetViewPortFromSpriteSheet("Slide_Bar").Width/ 2),
            _musicSlideBarHeight), Singleton.GetViewPortFromSpriteSheet("Slide_Bar"), Color.White);
            
            // SFX
            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (Singleton.GetViewPortFromSpriteSheet("SFX_Label").Width/ 2),
            _sfxLabelHeight), Singleton.GetViewPortFromSpriteSheet("SFX_Label"), Color.White);

            _spriteBatch.Draw(_texture, new Vector2((Singleton.SCREEN_WIDTH / 2) - (Singleton.GetViewPortFromSpriteSheet("Slide_Bar").Width/ 2),
            _sfxSlideBarHeight), Singleton.GetViewPortFromSpriteSheet("Slide_Bar"), Color.White);
            
            // Back button
            _backButton.Draw(_spriteBatch);

            // Music slide chip base on music volume
            if (Singleton.Instance.Volume <= 0)
            {
                _musicSlideChip.Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip0");
            }
            else if (Singleton.Instance.Volume <= 0.33)
            {
                _musicSlideChip.Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip1");
            }
            else if (Singleton.Instance.Volume <= 0.66)
            {
                _musicSlideChip.Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip2");
            }
            else
            {
                _musicSlideChip.Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip3");             
            }

            // SFX slide chip base on SFX volume
            if (Singleton.Instance.Volume <= 0)
            {
                _sfxSlideChip.Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip0");
            }
            else if (Singleton.Instance.Volume <= 0.33)
            {
                _sfxSlideChip.Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip1");
            }
            else if (Singleton.Instance.Volume <= 0.66)
            {
                _sfxSlideChip.Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip2");
            }
            else
            {
                _sfxSlideChip.Viewport = Singleton.GetViewPortFromSpriteSheet("Slide_Chip3");             
            }

            // Slide Chip
            _musicSlideChip.Draw(_spriteBatch);
            _sfxSlideChip.Draw(_spriteBatch);
        }
    }
}
