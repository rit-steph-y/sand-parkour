using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;

namespace HW5_GROUP_PROJECT.UI
{
    internal class Button : UIElement
    {
        private Microsoft.Xna.Framework.Rectangle rect;
        private Microsoft.Xna.Framework.Vector2 textSize;
        private TextBox textBox;

        internal int width { get{ return rect.Width; } }
        internal int height { get { return rect.Height; } }

        internal delegate void ButtonClicked();
        internal event ButtonClicked OnButtonClicked;

        internal Button(Texture2D tex, SpriteFont font, string label, int width, int height,
            Microsoft.Xna.Framework.Color color, Microsoft.Xna.Framework.Color textColor) : base(tex, origin, color)
        {
            rect = new Microsoft.Xna.Framework.Rectangle(0, 0, width, height);

            // centering text on the button
            textSize = font.MeasureString(label);
            textBox = new TextBox
                (font, label, new Vector2(rect.X + width / 2 - textSize.X / 2, rect.Y + height / 2 - textSize.Y / 2), textColor);
        }


        // this method makes sure that if the button moves, the text will also move to the apropriate place
        internal void Move(int x, int y)
        {
            rect.X = x; 
            rect.Y = y;

            textBox.X = x + width / 2 - textSize.X / 2;
            textBox.Y = y + height / 2 - textSize.Y / 2;
        }


        internal void Update(MouseState mouseState, MouseState prevMouseState)
        {
            if (rect.Contains(mouseState.Position) && 
                mouseState.LeftButton == ButtonState.Released && 
                prevMouseState.LeftButton == ButtonState.Pressed)
            {
                OnButtonClicked();
            }
        }


        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, rect, color);
            textBox.Draw(spriteBatch);
        }
    }
}
