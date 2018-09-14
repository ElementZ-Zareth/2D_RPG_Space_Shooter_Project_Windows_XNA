using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Sprite
    {
        #region Declarations

        public Texture2D Texture;
        private Vector2 worldLocation = Vector2.Zero;
        private Vector2 velocity = Vector2.Zero;
        private List<Rectangle> frames = new List<Rectangle>();
        private int currentFrame;
        private float frameTime = 0.1f;
        private float timeForCurrentFrame = 0.0f;
        private Color tintColor = Color.White;
        private float rotation = 0.0f;
        private float layer = 0.0f;
        private float scale = 1.0f;
        private double depth = 1.0f;
        private int depthSequence;
        private float desiredAngle;
        public bool layerDepth = true;
        public bool setDepth = true;
        public bool Expired = false;
        public bool Animate = true;
        public bool AnimateWhenStopped = true;
        public bool Collidable = true;
        public int CollisionRadius = 0;
        public int BoundingXPadding = 0;
        public int BoundingYPadding = 0;

        public float YPosition;
        
        #endregion

        #region Constructors

        public Sprite(Vector2 worldLocation, double depth, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            this.worldLocation = worldLocation;
            this.depth = depth;
            Texture = texture;
            this.velocity = velocity;
            frames.Add(initialFrame);
        }

        #endregion

        #region Drawing and Animation Properties

        public int FrameWidth
        {
            get { return frames[0].Width; }
        }

        public int FrameHeight
        {
            get { return frames[0].Height; }
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

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public double Depth
        {
            get
            {
                if (setDepth)
                {
                    return depth;
                } else { return depth = Camera.Depth + 1f; }
            }
            set { depth = value; }
        }

        public int DepthSequence
        {
            get { return depthSequence; }
            set { depthSequence = value; }
        }

        public int Frame
        {
            get { return currentFrame; }
            set { currentFrame = (int)MathHelper.Clamp(value, 0, frames.Count - 1); }
        }

        public float FrameTime
        {
            get { return frameTime; }
            set { frameTime = MathHelper.Max(0, value); }
        }

        public Rectangle Source
        {
            get { return frames[currentFrame]; }
        }

        #endregion

        #region Positional Properties

        public Vector2 WorldLocation
        {
            get { return worldLocation ; }
            set { worldLocation = value; }
        }

        public Vector2 ScreenLocation
        {
            get { return Camera.Transform(worldLocation); }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Rectangle WorldRectangle
        {
            get { return new Rectangle((int)worldLocation.X, (int)worldLocation.Y, (int)FrameWidth, FrameHeight); }
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

        public double DepthOffset
        {
            get 
            {
                return (Depth - Camera.Depth);

            }
        }

        public Vector2 ScreenCenter
        {
            get
            {
                return Camera.Transform(WorldCenter) - Camera.CameraCenter / (float)DepthOffset;
            }
        }

        public float DistanceXOffset
        {
            get { return Camera.DistanceX(WorldCenter) / Camera.WorldRectangle.Width; }
        }

        public float DistanceYOffset
        {
            get { return Camera.DistanceY(WorldCenter) / Camera.WorldRectangle.Height; }
        }

        #endregion

        #region Collision Related Properties

        public Rectangle BoundingBoxRect
        {
            get
            {
                return new Rectangle((int)ScreenCenter.X + BoundingXPadding,
                    (int)ScreenCenter.Y + BoundingYPadding,
                    (int)(FrameWidth * (scale / DepthOffset)) - (BoundingXPadding * 2),
                    (int)(FrameHeight * (scale / DepthOffset)) - (BoundingYPadding * 2));
            }
        }

        #endregion

        #region Collision Detection Methods

        public bool IsBoxColliding(Rectangle OtherBox)
        {
            if ((Collidable) && (!Expired))
            {
                return BoundingBoxRect.Intersects(OtherBox);
            }
            else { return false; }
        }

        public bool IsCircleColliding(Vector2 otherCenter, float otherRadius)
        {
            if ((Collidable) && (!Expired))
            {
                if (Vector2.Distance(WorldCenter, otherCenter) < (CollisionRadius + otherRadius))
                    return true;
                else return false;
            }
            else { return false; }
        }

        #endregion

        #region Animation-Related Methods

        public void AddFrame(Rectangle frameRectangle)
        {
            frames.Add(frameRectangle);
        }

        public void MoveTo(Vector2 destination, float moveSpeed)
        {
            float yMovement = destination.Y - worldLocation.Y;
            float xMovement = destination.X - worldLocation.X;
            float desiredMovement = (float)Math.Atan2(yMovement, xMovement);
            Vector2 targetMovement = new Vector2((float)Math.Cos(desiredMovement), (float)Math.Sin(desiredMovement));

            if (worldLocation != destination)
            {
                worldLocation += targetMovement * moveSpeed * (float)(Vector2.Distance(destination, worldLocation) / 10);
            }
        }

        public void ShootTo(Vector2 destination, float moveSpeed)
        {
            float yMovement = destination.Y - worldLocation.Y;
            float xMovement = destination.X - worldLocation.X;
            float desiredMovement = (float)Math.Atan2(yMovement, xMovement);
            Vector2 targetMovement = new Vector2((float)Math.Cos(desiredMovement), (float)Math.Sin(desiredMovement));

            worldLocation += targetMovement * moveSpeed;

        }

        public void ChangeDepth(double change, Sprite target)
        {
            depth -= (change / 10d) * (depth - target.Depth);
        }

        public void RotateTo(Vector2 position, Vector2 direction)
        {
            desiredAngle = (float)Math.Atan2(position.Y - direction.Y, position.X - direction.X);
            WrapAngle(desiredAngle);
            Rotation = desiredAngle;
        }

        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi)
            {
                radians += MathHelper.TwoPi;
            }
            while (radians > MathHelper.Pi)
            {
                radians -= MathHelper.TwoPi;
            }
            return radians;
        }

        #endregion

        #region Update and Draw Methods

        public virtual void Update(GameTime gameTime)
        {
            for (int d = 1; d < 3; d++)
            {
                if (DepthOffset <= 90d)
                {
                    DepthSequence = 1;
                }
                if (DepthOffset > ((d * 1000) - 910) && DepthOffset <= ((d * 1000) + 90))
                {
                    DepthSequence = d + 1;
                }
                if (DepthOffset > 2090)
                {
                    DepthSequence = 4;
                }


                YPosition = (ScreenCenter.Y) / 500000000f;

                if (DepthSequence == 1)
                {
                    if (layerDepth)
                    {
                        Layer = .9f - (float)((DepthOffset - 1) / 100d) + YPosition;
                    }
                    else { Layer = .9f - (float)((DepthOffset - 1) / 100d); }
                }

                if (DepthSequence == d + 1)
                {
                    Layer = 1f - (float)((DepthOffset - (d * 1000 - 910)) / 1000d);
                }

                if (DepthSequence == 4)
                {
                    Layer = 1f - (float)((DepthOffset - 2090) / 4830000000d);
                }
            }

            #region junkcode
            // Relation to world
                //layer = (((worldLocation.Y + RelativeCenter.Y) / (Camera.WorldRectangle.Height * 200f)) + 0.4975f) + ((depth - 1) * 0.4975f);

                // Relation to Screen Center and Camera
                //layer = ((((ScreenCenter.Y) / (Camera.ViewPortHeight)) / 100000f) + 0.499995f) - ((float)((DepthOffset)/1000000f) * 0.499995f);
                //layer = ((((ScreenCenter.Y) / (Camera.ViewPortHeight)) / 1000000f) + 0.4999995f) + ((float)((DepthOffset) * 0.001894d) * 0.4999995f);
                //layer = (((ScreenCenter.Y) / Camera.ViewPortHeight / 100000f) + 0.499995f) - (float)((DepthOffset - 1) * 0.001894d / 1000000f);

            //if (DepthOffset < 1000d)
            //{
                //layer = (((ScreenCenter.Y) / Camera.ViewPortHeight / 100000f) + .5f) - (float)((DepthOffset - 1) * 0.001894d / 1000000f);
                //layer = 0.499995f - (float)((DepthOffset - 1) * 0.001894d) + (((ScreenCenter.Y) / Camera.ViewPortHeight / 1000000f));
                //layer = 0.499995f - (float)((DepthOffset - 1) * 0.001894d) + (((ScreenCenter.Y) / Camera.ViewPortHeight / 1000000f));
                
                //if (!layerDepth)
                //{
                //    layer =  0.4999948f - (float)((DepthOffset - 1) * 0.001894d );
                    //layer = 0.4999948f - (float)(depth / 4830000000d);
                //}
            //}
            //else
            //{
                //layer = 0.4999948f - (float)(depth / 4830000000d);
            //    layer = .4999948f- (float)(DepthOffset / 4830000000d);
            //}
            #endregion

            layer = MathHelper.Clamp(layer, 0, 1);

            if (!Expired)
            {
                float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                timeForCurrentFrame += elapsed;
                if (Animate)
                {
                    if (timeForCurrentFrame >= FrameTime)
                    {
                        if ((AnimateWhenStopped) || (velocity != Vector2.Zero))
                        {
                            currentFrame = (currentFrame + 1) % (frames.Count);
                            timeForCurrentFrame = 0.0f;
                        }
                    }
                }

                worldLocation += (velocity * elapsed);
                
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!Expired)
            {
                if((double)depth - (double)Camera.Depth >=0 && (scale / (float)DepthOffset) > 0.0001f)
                {
                    spriteBatch.Draw(Texture, ScreenCenter, Source, tintColor, rotation, RelativeCenter, scale / (float)DepthOffset, SpriteEffects.None, Layer);
                }
                

            }
        }

        #endregion

    }
}
