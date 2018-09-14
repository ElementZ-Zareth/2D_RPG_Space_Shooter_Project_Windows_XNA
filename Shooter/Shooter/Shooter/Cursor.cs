using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    class Cursor
    {
        public Sprite MouseSprite;

        public Vector2 Position;

        MouseState mState;

        public float mX;

        public float mY;


        public void Initialize(Sprite mouseSprite, Vector2 position)
        {

            MouseSprite = mouseSprite;

            Position = position;

        }

        public void Update(GameTime gameTime)
        {

            mState = Mouse.GetState();
            mX = mState.X;
            mY = mState.Y;

            Position = new Vector2(mX + Camera.Position.X - MouseSprite.FrameWidth / 2, mY + Camera.Position.Y - MouseSprite.FrameHeight / 2);
                        
            MouseSprite.WorldLocation = Position;
            
            MouseSprite.Update(gameTime);

        }

        public void Draw(SpriteBatch spriteBatch)
        {

            MouseSprite.Draw(spriteBatch);

        }

    }
}
