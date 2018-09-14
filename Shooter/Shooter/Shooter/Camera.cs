using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Shooter
{
    public static class Camera
    {
        #region Declarations

        private static Vector2 position = Vector2.Zero;
        private static Vector2 viewPortSize = Vector2.Zero;
        private static Vector2 cameraCenter = Vector2.Zero;
        private static Rectangle worldRectangle = new Rectangle(0, 0, 0, 0);
        private static double depth = 0;

        #endregion

        #region Properties

        public static Vector2 Position
        {
            get { return position; }
            set
            {
                position = new Vector2(
                    MathHelper.Clamp(value.X, WorldRectangle.X, WorldRectangle.Width / 2),
                    MathHelper.Clamp(value.Y, WorldRectangle.Y, WorldRectangle.Height / 2)
                    );
            }
        }

        public static Rectangle WorldRectangle
        {
            get { return worldRectangle; }
            set { worldRectangle = value; }
        }

        public static int ViewPortWidth
        {
            get { return (int)viewPortSize.X; }
            set { viewPortSize.X = value; }
        }

        public static int ViewPortHeight
        {
            get { return (int)viewPortSize.Y; }
            set { viewPortSize.Y = value; }
        }

        public static Rectangle ViewPort
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, ViewPortWidth, ViewPortHeight); }
        }

        public static Vector2 CameraCenter
        {
            get { return new Vector2(Position.X + ViewPortWidth / 2, Position.Y + ViewPortHeight / 2); }
        }

        public static Vector2 worldCenter
        {
            get { return new Vector2(WorldRectangle.Width / 2, WorldRectangle.Height / 2); }
        }

        public static double Depth
        {
            get { return depth; }
            set { depth = value; }
        }


        #endregion
        
        #region Public Methods

        public static void Move(Vector2 offset)
        {
            Position += offset;
        }

        public static void ChangeDepth(double change, double targetDepth)
        {
            depth -= (change / 10d) * (depth - targetDepth);
        }

        public static bool ObjectIsVisible(Rectangle bounds)
        {
            return (ViewPort.Intersects(bounds));
        }

        public static Vector2 Transform(Vector2 point)
        {
            return point - position + CameraCenter;
        }

        public static float DistanceX(Vector2 point)
        {
            return point.X - CameraCenter.X;
        }

        public static float DistanceY(Vector2 point)
        {
            return point.Y - CameraCenter.Y;
        }

        public static Rectangle Transform(Rectangle rectangle)
        {
            return new Rectangle(rectangle.Left - (int)Position.X,
                rectangle.Top - (int)Position.Y,
                rectangle.Width,
                rectangle.Height);
        }

        #endregion

    }
}
