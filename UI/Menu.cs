using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HW5_GROUP_PROJECT.UI
{
    internal class Menu : UIElement
    {
        internal static Rectangle Rect { get; set; }

        internal List<Button> buttons { get; set; }
        internal List<TextBox> text { get; set; }

        internal Menu(Texture2D tex, Color color) : base(tex, origin, color) 
        { 
            buttons = new List<Button>();
            text = new List<TextBox>();
        }


        // this will automatically space out the buttons on the side of the screen
        // I have no clue why I did this
        internal void AddButton(Texture2D texture, SpriteFont font, String t, int width, int height, Color buttonColor, Color textColor)
        {
            buttons.Add(new Button(texture, font, t, width, height, buttonColor, textColor));
            int i = Rect.Height - 10;
            foreach (Button b in buttons)
            {
                b.Move(10, i - b.height);

                i -= 10 + b.height;
            }
        }

        internal void AddText(SpriteFont font, String t, Color color)
        {
            Vector2 textSize = font.MeasureString(t);
            Vector2 tPosition = new Vector2(Rect.Width - textSize.X, 0);

            TextBox textBox = new TextBox(font, t, tPosition, color);

            text.Add(textBox);
        }
        internal void AddText(SpriteFont font, String t, Vector2 tPosition, Color color)
        {
            TextBox textBox = new TextBox(font, t, tPosition, color);

            text.Add(textBox);
        }

        internal void Update(MouseState mouseState, MouseState prevMouseState)
        {
            foreach (Button b in buttons)
            {
                b.Update(mouseState, prevMouseState);
            }
        }

        internal override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, Rect, color);

            foreach (Button b in buttons)
            {
                b.Draw(spriteBatch);
            }

            foreach (TextBox t in text)
            {
                t.Draw(spriteBatch);
            }
        }
    }
}
