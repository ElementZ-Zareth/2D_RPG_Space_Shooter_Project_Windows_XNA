using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Shooter
{
    class Projectile : Sprite
    {
        
        public Vector2 Target;

        
        // Position of mouse
        MouseState currentMouseState = Mouse.GetState();
        public Vector2 projectileMovement;
        
        // State of the Projectile
        public bool Active;
        
        // The amount of damage the projectile can inflict to an enemy
        public int Damage;

        // Determines how fast the projectile moves
        public float projectileMoveSpeed;

        public float rotation;

        public float desiredAngle;

        float RandomAngleValue;

        public enum ParticleType { isplayer, isenemy, isfiring, ishit }
        ParticleType pType;

        private float xPosition;
        private float yPosition;
        private float xAngle;
        private float yAngle;

        double tDepth;
        double tDistance;
        double trueDistance;

        public TimeSpan expiredTime;
        public TimeSpan StartCounter;
        public bool StartTime = true;

        public int Accuracy;

        public Projectile(Vector2 position, double depth, Texture2D texture, Rectangle initialframe, Vector2 velocity, Random random, Vector2 target, Sprite targetDepth, float pScale, int pDamage, float randomAngleValue, ParticleType PType, int accuracy) : base(position, depth, texture, initialframe, velocity)
        {
            pType = PType;
            WorldLocation = position;
            Accuracy = accuracy;
            
            RandomAngleValue = randomAngleValue;
            Target = target;
            Damage = pDamage;
            depth = Depth;

            switch (pType)
            {
                // case is player shot
                case ParticleType.isplayer:
                    pDamage = 0;

                    this.Scale = pScale;

                    random = new Random();

                    Active = true;

                    projectileMoveSpeed = 15f;

                    xPosition = target.X - position.X;
                    yPosition = target.Y - position.Y;

                    desiredAngle = (float)Math.Atan2(yPosition, xPosition) - ((float)random.NextDouble() / randomAngleValue) + ((float)random.NextDouble() / randomAngleValue);

                    xAngle = (float)Math.Cos(desiredAngle);
                    yAngle = (float)Math.Sin(desiredAngle);

                    projectileMovement = new Vector2(xAngle, yAngle);

                    WrapAngle(desiredAngle);

                    rotation = desiredAngle;

                    tDepth = depth - targetDepth.Depth;
                    tDistance = Vector2.Distance(this.WorldCenter, targetDepth.WorldCenter);
                    trueDistance = (tDepth / 0.0018939) + tDistance;

                break;

                // case is enemy shot
                case ParticleType.isenemy:

                    pDamage = 0;

                    this.Scale = pScale;
                    
                    random = new Random();

                    Active = true;

                    projectileMoveSpeed = 8f;

                    xPosition = target.X - position.X;
                    yPosition = target.Y - position.Y;

                    desiredAngle = (float)Math.Atan2(yPosition, xPosition) - ((float)random.NextDouble() / randomAngleValue) + ((float)random.NextDouble() / randomAngleValue);

                    xAngle = (float)Math.Cos(desiredAngle);
                    yAngle = (float)Math.Sin(desiredAngle);

                    projectileMovement = new Vector2(xAngle, yAngle);

                    WrapAngle(desiredAngle);

                    rotation = desiredAngle;

                    tDepth = depth - targetDepth.Depth;
                    tDistance = Vector2.Distance(this.WorldCenter * (float)this.DepthOffset, targetDepth.WorldCenter * (float)targetDepth.DepthOffset);
                    trueDistance = (tDepth / 0.0018939) + tDistance;


                break;


                // case is firing sprite
                case ParticleType.isfiring:

                    expiredTime = TimeSpan.FromSeconds(.015f);
                    

                    this.Scale = pScale;
                    this.Rotation = randomAngleValue;

                    random = new Random();

                    Active = true;

                    projectileMoveSpeed = 15f;

                    xPosition = target.X - position.X;
                    yPosition = target.Y - position.Y;

                    

                    desiredAngle = (float)Math.Atan2(yPosition, xPosition) - ((float)random.NextDouble() / randomAngleValue) + ((float)random.NextDouble() / randomAngleValue);

                    xAngle = (float)Math.Cos(desiredAngle);
                    yAngle = (float)Math.Sin(desiredAngle);

                    WrapAngle(desiredAngle);

                    rotation = desiredAngle;

                                        tDepth = depth - targetDepth.Depth;
                    tDistance = Vector2.Distance(this.WorldCenter * (float)this.DepthOffset, targetDepth.WorldCenter * (float)targetDepth.DepthOffset);
                    trueDistance = (tDepth / 0.0018939) + tDistance;

                break;

                // case is hit sprite
                case ParticleType.ishit:

                    expiredTime = TimeSpan.FromSeconds(.025f);
                    

                    this.Scale = pScale;
                    this.Rotation = randomAngleValue;

                    random = new Random();

                    Active = true;

                    projectileMoveSpeed = 15f;

                    xPosition = target.X - position.X;
                    yPosition = target.Y - position.Y;

                    desiredAngle = (float)Math.Atan2(yPosition, xPosition) - ((float)random.NextDouble() / randomAngleValue) + ((float)random.NextDouble() / randomAngleValue);

                    xAngle = (float)Math.Cos(desiredAngle);
                    yAngle = (float)Math.Sin(desiredAngle);

                    WrapAngle(desiredAngle);

                    rotation = desiredAngle;
                                        tDepth = depth - targetDepth.Depth;
                    tDistance = Vector2.Distance(this.WorldCenter * (float)this.DepthOffset, targetDepth.WorldCenter * (float)targetDepth.DepthOffset);
                    trueDistance = (tDepth / 0.0018939) + tDistance;

                break;

            }                 
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

        public override void Update(GameTime gameTime)
        {
            //ParticleType pType = new ParticleType();
            switch (pType)
            {
                // if type is the player shot
                case ParticleType.isplayer:
                // Projectile Movement based on target
                    

                    if (tDepth != 0)
                    {
                        WorldLocation += projectileMovement * (projectileMoveSpeed * (float)((tDistance) * 0.0018939d)) / (float)(trueDistance * 0.0018939d);
                    } else { WorldLocation += projectileMovement * projectileMoveSpeed; }

                    Depth -= ((tDepth * 0.0018939d) * (projectileMoveSpeed)) / (trueDistance * 0.0018939d);

                // Deactiveate the bullet if it goes out of screen
                //if (WorldLocation.Y < -60 || WorldLocation.Y > Camera.WorldRectangle.Height + 60 || WorldLocation.X < -60 || WorldLocation.X > Camera.WorldRectangle.Width + 60)
                //Active = false;
                break;

                // if type is the enemy shot
                case ParticleType.isenemy:

                // Projectile Movement based on target
                    if (tDepth != 0)
                    {
                        WorldLocation += projectileMovement * (projectileMoveSpeed * (float)((tDistance) * 0.0018939d)) / (float)(trueDistance * 0.0018939d);
                    } else { WorldLocation += projectileMovement * projectileMoveSpeed; }

                    Depth -= ((tDepth * 0.0018939d) * (projectileMoveSpeed)) / (trueDistance * 0.0018939d);
                


                // Deactiveate the bullet if it goes out of screen
                //if (WorldLocation.Y < -60 || WorldLocation.Y > Camera.WorldRectangle.Height + 60 || WorldLocation.X < -60 || WorldLocation.X > Camera.WorldRectangle.Width + 60)
                //Active = false;
                break;

                // if type is firing sprite
                case ParticleType.isfiring:
                
                //WorldLocation = Position;

                break;

                // if type is getting hit sprite
                case ParticleType.ishit:

                //WorldLocation = Position;

                break;

            }

            base.Update(gameTime);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
                       
    }
}
