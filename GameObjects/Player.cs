using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class Player : GameObject
{
    public Bubble Bubble;
    public Keys Left, Right, Fire;
    
    public Player(Texture2D texture) : base(texture)
    {

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(_texture, Position, Viewport, Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
        base.Draw(spriteBatch);
    }

    public override void Reset()
    {
        Position = new Vector2((Singleton.SCREENWIDTH - Rectangle.Width) / 2, 400);
        base.Reset();
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        // if(Singleton.Instance.CurrentKey.IsKeyDown(Left))
        // {
        //     Velocity.X = -500;
        // }
        // if(Singleton.Instance.CurrentKey.IsKeyDown(Right))
        // {
        //     Velocity.X = 500;
        // }
        if( Singleton.Instance.CurrentKey.IsKeyDown(Fire) &&
            Singleton.Instance.PreviousKey != Singleton.Instance.CurrentKey)
        {
            var newBubble = Bubble.Clone() as Bubble;
            newBubble.Position = new Vector2(Rectangle.Width / 2 + Position.X - newBubble.Rectangle.Width / 2,
                                            Position.Y);
            newBubble.Reset();
            gameObjects.Add(newBubble);
        }

        // float newX = Position.X + Velocity.X * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
        // newX = MathHelper.Clamp(newX, 0, Singleton.SCREENWIDTH - Rectangle.Width);
        // Position = new Vector2(newX, Position.Y);

        // Velocity = Vector2.Zero;

        base.Update(gameTime, gameObjects);
    }
}