using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    class Drops : Sprite
    {
        public enum DropType { Experience, Item, Heal, Upgrade };
        public DropType dropType;
        public bool Active = true;
        public int amountOrid;
        public Vector2 Target;
        
        public float moveSpeed;
        public Vector2 moveTarget;

        public Drops(Vector2 position, double depth, Texture2D texture, Rectangle initialframe, Vector2 velocity, float pScale, Player player, int AmountOrID, DropType DropType)
            : base(position, depth, texture, initialframe, velocity)
        {
            WorldLocation = position;
            dropType = DropType;
            amountOrid = AmountOrID;
            

            switch (dropType)
            {

                case DropType.Experience:
                    int ExperienceAmount = AmountOrID;
                    moveSpeed = 20f;
                break;

                case DropType.Item:
                    int ItemID = AmountOrID;
                break;

                case DropType.Heal:
                    int HealAmount = AmountOrID;
                break;

                case DropType.Upgrade:
                    int UpgradeID = AmountOrID;
                break;

            }

        }

        public void Update(GameTime gameTime, Vector2 target)
        {
            Target = target;
            float xPosition = Target.X - WorldLocation.X;
            float yPosition = Target.Y - WorldLocation.Y;
            float desiredAngle = (float)Math.Atan2(yPosition, xPosition);
            Vector2 targetAngle = new Vector2((float)Math.Cos(desiredAngle), (float)Math.Sin(desiredAngle));

            moveTarget = targetAngle;

            switch (dropType)
            {
                case DropType.Experience:

                    
                    WorldLocation += (moveSpeed * moveTarget);
                    break;

                case DropType.Item:
                    
                    break;

                case DropType.Heal:
                    
                    break;

                case DropType.Upgrade:
                    
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
