using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

namespace Shooter
{
    class Damage
    {
        public List<HitText> hitTexts = new List<HitText>();
        
                        
        public void DamageEnemy(Player player, Projectile projectile, Enemy enemy, SpriteFont font)
        {
            Random random = new Random();
            if (enemy.elementType == Enemy.ElementType.Neutral)
            {
                int FinalAccuracy = player.Accuracy - enemy.Evasion;
                int EvasionCheck = random.Next(0, player.Accuracy);

                if (FinalAccuracy >= EvasionCheck)
                {
                    int FinalOutput = (projectile.Damage - enemy.Defense) + random.Next((projectile.Damage * -1) + (projectile.Damage / 2), (projectile.Damage * 3) / 2);
                    enemy.Health -= FinalOutput;
                    AddHitText(enemy.Position, FinalOutput.ToString(), enemy.EnemySprite.Depth, font);
                }
                else { AddHitText(enemy.Position, "MISS", enemy.EnemySprite.Depth, font); }
            }
            else if (enemy.elementType == Enemy.ElementType.Ice)
            {
                enemy.Health -= projectile.Damage;
            }
            else if (enemy.elementType == Enemy.ElementType.Electric)
            {
                enemy.Health -= projectile.Damage;
            }
            else if (enemy.elementType == Enemy.ElementType.Heat)
            {
                enemy.Health -= projectile.Damage;
            }
        }

        public void DamagePlayer(Player player, Projectile projectile, SpriteFont font)
        {
            Random random = new Random();

            int FinalAccuracy = projectile.Accuracy - player.Evasion;
            int EvasionCheck = random.Next(0, projectile.Accuracy);

            if (FinalAccuracy >= EvasionCheck)
            {
                int FinalOutput = (projectile.Damage - player.Defense) + random.Next((projectile.Damage * -1) + (int)(projectile.Damage / 2), (int)((projectile.Damage * 3) / 2));
                FinalOutput = (int)MathHelper.Clamp(FinalOutput, 0, 9999);
                player.Health -= FinalOutput;
                AddHitText(player.Position, FinalOutput.ToString(), player.PlayerSprite.Depth, font);
            }
            else { AddHitText(player.Position, "MISS", player.PlayerSprite.Depth, font); }
        }

        public void HealPlayer(Player player, int healAmount, SpriteFont font)
        {
            Random random = new Random();

            int FinalOutput = healAmount + random.Next((healAmount / 2) * -1, (healAmount / 2));
            player.Health += FinalOutput;
            AddHitText(player.Position, FinalOutput.ToString(), player.PlayerSprite.Depth, font);
        }

        protected void AddHitText(Vector2 position, String textdamage, double depth, SpriteFont font)
        {
            HitText hitText = new HitText();
            hitText.SpriteText(font, depth, position, textdamage);
            hitTexts.Add(hitText);
        }

        public virtual void UpdateHitTexts(GameTime gameTime)
        {
            for (int i = hitTexts.Count - 1; i >= 0; i--)
            {
                hitTexts[i].WorldLocation = hitTexts[i].WorldLocation;
                hitTexts[i].Update(gameTime);
                if (hitTexts[i].Expired == true)
                {
                    hitTexts.RemoveAt(i);
                }
            }
        }

    }
}
