using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HW5_GROUP_PROJECT.UI
{
    internal class Menu : UIElement
    {
        internal static Microsoft.Xna.Framework.Rectangle Rect { get; set; }

        internal List<Button> buttons { get; set; }

        internal Menu(Texture2D tex, Microsoft.Xna.Framework.Color color) : base(tex, origin, color) 
        { 
            buttons = new List<Button>();
        }


        // this will automatically space out the buttons on the side of the screen
        // I have no clue why I did this
        internal void AddButton(Button button)
        {
            buttons.Add(button);
            int i = Rect.Height - 10;
            foreach (Button b in buttons)
            {
                b.Move(10, i - b.height);

                i -= 10 + b.height;
            }
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
        }
    }
}
