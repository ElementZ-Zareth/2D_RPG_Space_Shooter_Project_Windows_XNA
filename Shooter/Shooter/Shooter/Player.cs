using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Player
    {

        // Animation representing the player
        public Sprite PlayerSprite;

        // Positioning of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // State of the Player
        public bool Active;

        // Amount of hit points that player has
        public int TotalHealth;
        public int Health;
        public float HPPercentage;
        public float TotalShields;
        public float Shields;
        public float ShieldPercentage;

        public float ShieldRecovery;

        public int Accuracy;
        public int Evasion;

        public int Defense;

        // Charge 
        public float TotalCharge;
        public float Charge;
        public float ChargePercentage;
        public float ChargeCost;
        public float ChargeRecovery;
        public float ChargeCounter;
        public bool ChargeRelease;
        

        // Experience
        public int Experience;

        // Level
        public int Level;

        // Shots
        public int Shots;

        //Hit state
        public bool Hit;
        //Blink state
        public bool Blink;
        //Is moving state;
        public bool IsMoving;

        // Get the width of the player ship
        public int Width
        {
            get { return PlayerSprite.FrameWidth; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return PlayerSprite.FrameHeight; }
        }

        // The rate of fire of the player laser
        public TimeSpan fireTime;
        public TimeSpan previousFireTime;
        public float fireSeconds;

        public void Initialize(Sprite playerSprite, Vector2 position)
        {

            PlayerSprite = playerSprite;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;

            // Set the player health
            TotalHealth = 100;
            Health = TotalHealth;
            TotalShields = 100;
            Shields = TotalShields;

            ShieldRecovery = .025f;

            Accuracy = 10;
            Evasion = 5;

            Defense = 1;

            // Initialize Charge
            TotalCharge = 100;
            Charge = TotalCharge;
            ChargeCost = 0;
            ChargeRecovery = 1f;
            ChargeCounter = 0f;
            ChargeRelease = false;
            


            // Set player experience
            Experience = 0;

            // Set player level
            Level = 1;

            // Set hit phase for player
            Hit = false;
            Blink = false;
            IsMoving = false;

            fireSeconds = 0.05f;
        }

        public void Update(GameTime gameTime)
        {
            HPPercentage = (float)Health / (float)TotalHealth;
            ChargePercentage = (float)Charge / (float)TotalCharge;
            ShieldPercentage = (float)Shields / (float)TotalShields;

            if (Shields < TotalShields) { Shields += ShieldRecovery; Shields = MathHelper.Clamp(Shields, 0, TotalShields); }
            if (Charge < TotalCharge) { Charge += ChargeRecovery; Charge = MathHelper.Clamp(Charge, 0, TotalCharge); }
            fireTime = TimeSpan.FromSeconds(fireSeconds);
            PlayerSprite.WorldLocation = Position;
            PlayerSprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerSprite.Draw(spriteBatch);
        }
    }
}
