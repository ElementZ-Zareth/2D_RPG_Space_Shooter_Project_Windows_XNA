using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Walls
    {
        public List<Sprite> walls = new List<Sprite>();
        
        public Walls(Vector2 location, double depth, Texture2D texture, float rotation)
        {
            
            for (int x = 0; x < texture.Width; x++)
            {
                walls.Add(new Sprite(location, depth + (x * 0.0018939d * .5d), texture, new Rectangle(x, 0, 1, texture.Height), new Vector2(0, 0)));
                walls[walls.Count() - 1].Rotation = rotation;
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Sprite wall in walls)
            {
                wall.Update(gameTime);
            }
        }

    }
}
