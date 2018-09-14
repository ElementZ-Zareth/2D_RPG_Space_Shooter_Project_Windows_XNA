using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    class HitText
    {

        #region Declarations

        public SpriteFont Font;
        private Vector2 worldLocation = Vector2.Zero;
        private Color tintColor = Color.White;
        private float rotation = 0.0f;
        private float layer = 0.0f;
        private double depth;
        private float scale = 1f;
        private float moveSpeed = 5f;
        public bool Expired = false;
        public String Damage;

        #endregion

        #region Constructors

        public void SpriteText(SpriteFont font, double depth, Vector2 worldLocation, String damage)
        {

            Font = font;
            this.worldLocation = worldLocation;
            this.depth = depth;
            Damage = damage;
            
        }

        public Color TintColor
        {
            get { return tintColor; }
            set { tintColor = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }

        public float Layer
        {
            get { return layer; }
            set { layer = value; }
        }

        public double Depth
        {
            get { return depth; }
            set { depth = value; }
        }
        
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        public int FrameWidth
        {
            get { return Damage.Length; }
        }

        public int FrameHeight
        {
            get { return 30; }
        }

        #endregion

        #region Positional Properties

        public Vector2 WorldLocation
        {
            get { return worldLocation; }
            set { worldLocation = value; }
        }

        public Vector2 ScreenLocation
        {
            get { return Camera.Transform(worldLocation); }
        }

        public Rectangle WorldRectangle
        {
            get { return new Rectangle((int)worldLocation.X, (int)worldLocation.Y, FrameWidth, FrameHeight); }
        }

        public Rectangle ScreenRectangle
        {
            get { return Camera.Transform(WorldRectangle); }
        }

        public Vector2 RelativeCenter
        {
            get { return new Vector2(FrameWidth / 2, FrameHeight / 2); }
        }

        public Vector2 WorldCenter
        {
            get { return (worldLocation + RelativeCenter) / (float)DepthOffset; }
        }

        public Vector2 ScreenCenter
        {
            get { return Camera.Transform(WorldCenter) - Camera.CameraCenter / (float)DepthOffset; }
        }

        public double DepthOffset
        {
            get
            {
                return (Depth - Camera.Depth);

            }
        }

        #endregion

        public virtual void Update(GameTime gameTime)
        {

            layer = 1f;
            
            depth -= .035f;

            if (Depth < Camera.Depth - 1d)
            {
                Expired = true;
            }

            

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Expired)
            {
                if ((double)depth - (double)Camera.Depth >= 0)
                {
                    spriteBatch.DrawString(Font, Damage, ScreenCenter, tintColor, rotation, RelativeCenter, scale / (float)DepthOffset, SpriteEffects.None, Layer);
                }
            }
        }
    }
}
