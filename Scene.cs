using HW5_GROUP_PROJECT.sand;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HW5_GROUP_PROJECT
{
    internal class Scene
    {
        private Texture2D spriteSheet;

        internal Scene(Texture2D spriteSheet)
        {
            this.spriteSheet = spriteSheet;
        }


        internal void LoadLevel(SandGridComponent sand)
        {
            //initializes an array of colors the exact size of said texture
            Color[] colors = new Color[spriteSheet.Height * spriteSheet.Width];

            //then copys the color data from the texture to the color array
            spriteSheet.GetData(colors);

            //then prints out the color at the top left corner of the image.
            Console.WriteLine($"{colors[0]}");

            // Aj's code 
            uint columns = 0;
            uint rows = 0;
            foreach (Color i in colors)
            {
                if (i == Color.Red)
                {
                    sand.SetPixel(columns, rows, PixelId.SAND, Color.SandyBrown);
                    columns++;
                }
                else if (i == Color.White)
                {
                    sand.SetPixel(columns, rows, PixelId.AIR, Color.White);
                    columns++;
                }
                else
                {
                    sand.SetPixel(columns, rows, PixelId.INVALID, Color.Gray);
                    columns++;
                }

                if (columns >= spriteSheet.Width)
                {
                    rows++;
                    columns = 0;
                }
            }

            //

            //this might be ridculus 
        }
    }
}
