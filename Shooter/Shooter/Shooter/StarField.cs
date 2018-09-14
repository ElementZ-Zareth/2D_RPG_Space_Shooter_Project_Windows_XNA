using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class StarField
    {

        private List<Sprite> stars = new List<Sprite>();
        private float screenWidth;
        private float screenHeight;
        private Random rand = new Random();
        private Color[] colors = { Color.White, Color.Yellow, 
                           Color.Wheat, Color.WhiteSmoke, 
                           Color.SlateGray };

        public StarField(
            float screenWidth,
            float screenHeight,
            int starCount,
            Vector2 starVelocity,
            Texture2D texture,
            Rectangle frameRectangle)
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            for (int x = 0; x < starCount; x++)
            {
                stars.Add(new Sprite(
                    new Vector2(-(float)rand.NextDouble() * rand.Next(-6, 6) * 10000000000000f + (float)rand.NextDouble() * rand.Next(-6, 6) * 10000000000000f,
                        -(float)rand.NextDouble() * rand.Next(-6, 6) * 10000000000000f + (float)rand.NextDouble() * rand.Next(-6, 6) * 10000000000000f),
                        ((float)rand.NextDouble() * rand.Next(4, 6) * 1000000000000f),
                    texture,
                    frameRectangle,
                    starVelocity));
                Color starColor = colors[rand.Next(0, colors.Count())];
                starColor *= (float)(rand.Next(30, 80) / 100f);
                stars[stars.Count() - 1].TintColor = starColor;
                stars[stars.Count() - 1].Scale = 22441172800f;
                stars[stars.Count() - 1].layerDepth = false;
                stars[stars.Count() - 1].Layer = 0f;
            }

        }   

        public void Update(GameTime gameTime)
        {
            foreach (Sprite star in stars)
            {
                star.Update(gameTime);
                if (star.WorldLocation.Y >= screenHeight * 2)
                {
                    star.WorldLocation = new Vector2(rand.Next(-4, 4) * 100000000000f, -screenHeight);
                }
                
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Sprite star in stars)
            {
                star.Draw(spriteBatch);
            }
        }

    }
}
