using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace HW5_GROUP_PROJECT.UI
{
    // Abstract class for any of the UI Elements to inherit from
    internal abstract class UIElement
    {
        internal static Vector2 origin { get; } = new Vector2(0, 0);

        protected Texture2D tex;
        protected Vector2 pos;
        protected Microsoft.Xna.Framework.Color color;

        internal float X
        {
            get { return pos.X; }

            set { pos.X = value; }
        }
        internal float Y
        {
            get { return pos.Y; }

            set { pos.Y = value; }
        }

        internal UIElement(Texture2D tex, Vector2 pos, Microsoft.Xna.Framework.Color color)
        {
            this.tex = tex;
            this.pos = pos;
            this.color = color;
        }


        internal virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, pos, color);
        }
    }
}
