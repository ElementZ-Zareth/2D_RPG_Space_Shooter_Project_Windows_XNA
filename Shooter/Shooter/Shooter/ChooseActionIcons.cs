using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class ChooseActionIcons
    {
        public Vector2 Position;

        public Rectangle IconRectangle;

        public bool Active;

        public bool Selected;

        public int Type;

        public float Transparency;

        public Texture2D IconType;

        public int Slot;

        public Color color;

        public void Initialize(Texture2D iconType, Vector2 position, int slot, int type)
        {
            IconType = iconType;
            Position = position;
            Slot = slot;
            Type = type;
            Selected = false;
            Active = false;
            IconRectangle = new Rectangle((int)Position.X, (int)Position.Y, 30, 30);
            color = Color.White;
        }

        public void Update(GameTime gameTime)
        {
            IconRectangle = new Rectangle((int)Position.X, (int)Position.Y, 30, 30);
            if (!Selected) { Transparency = .25f; }
            if (Selected) { Transparency = 1.0f; }
            if (Type == 1 && Active == true) { color = Color.Yellow; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(IconType, new Vector2(Position.X, Position.Y), new Rectangle(0, 0, 40, 40), color * Transparency, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.99999997f);
        }

    }
}
