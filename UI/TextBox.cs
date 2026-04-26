using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HW5_GROUP_PROJECT.UI
{
    internal class TextBox
    {
        private SpriteFont font;
        private string text;
        private Vector2 position;
        private Microsoft.Xna.Framework.Color color;

        internal float X
        {
            get { return position.X; }

            set { position.X = value; }
        }
        internal float Y
        {
            get { return position.Y; }

            set { position.Y = value; }
        }

        internal TextBox(SpriteFont font, string text, Vector2 position, Microsoft.Xna.Framework.Color color)
        {
            this.font = font;
            this.text = text;
            this.position = position;
            this.color = color;
        }


        internal void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, position, color);
        }
    }
}
