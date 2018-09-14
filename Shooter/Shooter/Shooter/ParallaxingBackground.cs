using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
	class ParallaxingBackground
	{
        Texture2D texture;

        Vector2[] positions;

        int speed;

        public void Initialize(ContentManager content, String texturePath, int screenHeight, int speed)
        {
            // Load the background texture we will be using
            texture = content.Load<Texture2D>(texturePath);

            // Set the speed of the background
            this.speed = speed;

            // If we divide the screen with the texture width then we can determine the number of tiles need.
            // We add 1 to it so that we won't have a gap in the tiling
            positions = new Vector2[screenHeight / texture.Height + 1];

            // Set the initial positions of the parallaxing background
            for (int i = 0; i < positions.Length; i++)
            {
                // We need the tiles to be side by side to create a tiling effect
                positions[i] = new Vector2(0, i * -texture.Height);
            }
        }

        public void Update()
        {
            // Update the positions of the background
            for (int i = 0; i < positions.Length; i++)
            {
                // Update the position of the screen by adding the speed
                positions[i].Y -= speed;
                // If the speed has the background moving to the left
                if (speed <= 0)
                {
                    // Check the texture is out of view then put that texture at the end of the screen
                    if (positions[i].Y >= texture.Height)
                    {
                        positions[i].Y = texture.Height * -(positions.Length - 1);
                    }
                }

                // If the speed has the background moving to the right
                else
                {
                    // Check if the texture is out of view then position it to the start of the screen
                    if (positions[i].Y >= texture.Height * (positions.Length - 1))
                    {
                        positions[i].Y = -texture.Height;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                spriteBatch.Draw(texture, positions[i], new Rectangle(0, 0, 1280, 720), Color.White * .5f, 0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.000001f);
            }
        }
	}


}
