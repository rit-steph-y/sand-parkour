using HW5_GROUP_PROJECT.sand;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

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
            Microsoft.Xna.Framework.Color[] colors = new Microsoft.Xna.Framework.Color[spriteSheet.Height * spriteSheet.Width];

            //then copys the color data from the texture to the color array
            spriteSheet.GetData(colors);

            //then prints out the color at the top left corner of the image.
            Console.WriteLine($"{colors[0]}");

            // Aj's code 
            uint columns = 0;
            uint rows = 0;
            foreach (Microsoft.Xna.Framework.Color i in colors)
            {
                if (i == Microsoft.Xna.Framework.Color.Red)
                {
                    sand.SetPixel(columns, rows, PixelId.SAND);
                    columns++;
                }
                else if (i == Microsoft.Xna.Framework.Color.White)
                {
                    sand.SetPixel(columns, rows, PixelId.AIR);
                    columns++;
                }
                else
                {
                    sand.SetPixel(columns, rows, PixelId.INVALID);
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
