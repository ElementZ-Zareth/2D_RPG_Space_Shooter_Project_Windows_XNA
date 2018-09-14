using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Enemy
    {
        public enum ElementType { Neutral, Ice, Electric, Heat }
        public ElementType elementType;

        // Animation representing the enemy
        public Sprite EnemySprite;

        // The position of the enemy ship relative to the top left corner of the screen
        public Vector2 Position;

        // Target movement
        public Vector2 Target;
        public Vector2 enemyMovement;
        public Rectangle enemyRectangle;
        public Vector2 previousPosition;

        // The state of the Enemy
        public bool Active;

        //public bool IsDead = false;
        public bool IsEngaged = true;
        public bool SeesTarget = false;
        public float SightRange;

        // The hit points of the enemy, if this goes to zero the enemy dies
        public int Health;

        // The amount of damage the enemy inflicts on the player ship
        public int Damage;

        // Accuracy and Evasion
        public int Accuracy = 10;
        public int Evasion = 5;
        public int Defense = 1;

        // The amount of score the enemy will give to the player
        public int Value;

        // The amount of experience the enemy will give the player
        public int ExperienceValue;

  
        // Get the width of the enemy ship
        public int Width
        {
            get { return EnemySprite.FrameWidth; }
        }

        // Get the height of the enemy ship
        public int Height
        {
            get { return EnemySprite.FrameHeight; }
        }

        //public bool jumpTime;

        // Enemy Movement Timing
        TimeSpan enemyMovementTime;
        TimeSpan previousMovementTime;
        TimeSpan enemyPauseTime;
        TimeSpan previousPauseTime;

        public TimeSpan enemyShootTime;
        public TimeSpan previousEShootTime;
        
        // The speed at which the enemy moves
        public float enemyMoveSpeed;


        public void Initialize(Sprite sprite, Vector2 position, float sightRange, ElementType ElementType)
        {
            // Load the enemy ship texture
            EnemySprite = sprite;
           
            // Set the position of the enemy
            Position = position;

            // Set sight range
            SightRange = sightRange;

            elementType = ElementType;

            enemyRectangle = new Rectangle((int)Position.X, (int)Position.Y, 75, 55);

            previousPosition = position;

            // We initialize the enemy to be active so it will be updated in the game
            Active = true;
            

            // Set the health of the enemy
            Health = 50;

            // Set the amount of damage the enemy can do
            Damage = 10;

            // Set how fast the enemy moves
            enemyMoveSpeed = 3f;

            // Set the score value of the enemy
            Value = 100;

            // Set the experience value of the enemy
            ExperienceValue = 2;

            // Reset enemy movement time
            previousMovementTime = TimeSpan.Zero;
            // How long do enemies move
            enemyMovementTime = new TimeSpan(0, 0, 0, 0, 400);
            // Reset Pause Time
            previousPauseTime = TimeSpan.Zero;
            // How long do enemies move
            //enemyPauseTime = TimeSpan.FromSeconds(.5f);
            enemyPauseTime = new TimeSpan(0, 0, 0, 0, 300);
            enemyShootTime = new TimeSpan(0, 0, 0, 1, 0);
            previousEShootTime = TimeSpan.Zero;
            
        }

        public void Update(GameTime gameTime, Vector2 target, Sprite spritetarget)

            // The enemy always moves to the players Position
            
            {


                
            //This code saved for later possibly for when I actually use the jumping slimes
              //  if (jumpTime == false)
               // {
                    //previousMovementTime = gameTime.TotalGameTime;
                    //Set Target
                if (Vector2.Distance(Position, target) < SightRange)
                {
                    SeesTarget = true;
                    Target = target;
                    float xPosition = Target.X - Position.X;
                    float yPosition = Target.Y - Position.Y;
                    float desiredAngle = (float)Math.Atan2(yPosition, xPosition);
                    Vector2 targetAngle = new Vector2((float)Math.Cos(desiredAngle), (float)Math.Sin(desiredAngle));

                    enemyMovement = targetAngle;



                    //}

                    //if (gameTime.TotalGameTime - previousMovementTime < enemyMovementTime)
                    //if (jumpTime == true)
                    // {

                    //                    previousPauseTime = gameTime.TotalGameTime;
                    //                  previousPosition = Position;
                    Position += (enemyMovement * enemyMoveSpeed);
                }
                else { SeesTarget = false; }
      //              
        //        }


            // Update the position of the Animation
            EnemySprite.WorldLocation = Position;
            enemyRectangle = new Rectangle((int)Position.X, (int)Position.Y, 100, 100);

            EnemySprite.ChangeDepth(1f, spritetarget);

            EnemySprite.Rotation -= .1f;

            // Update Animation
            EnemySprite.Update(gameTime);




            // If the enemy is past the screen or its health reaches 0 then deactivate it
            if (Health <= 0)
            {
                // By setting the active flag to false, the game will remove this object from the active game list
                Health = 0;
                Active = false;

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw the animation
            EnemySprite.Draw(spriteBatch);
        }

    }
}
