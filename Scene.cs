using HW5_GROUP_PROJECT.sand;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HW5_GROUP_PROJECT
{
    internal class Scene
    {
        private Texture2D spriteSheet;
        private float SandRollingAvgMs = 0;

        private SandGridComponent sand;
        
        internal Scene(Texture2D spriteSheet, GraphicsDevice device)
        {
            this.sand = new(device);
            this.spriteSheet = spriteSheet;
        }

        internal void Draw(Rectangle clientBounds,SpriteBatch spriteBatch)
        {
            Point windowSize = new(clientBounds.Width, clientBounds.Height);
            this.sand.Draw(spriteBatch, windowSize.ToVector2());
        }

        internal void LoadLevel()
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
        }

        internal void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.sand.xOffset = mouseState.Position.X;
            this.sand.yOffset = mouseState.Position.Y;
            this.sand.Update();
            stopwatch.Stop();
            this.SandRollingAvgMs *= .7f;
            this.SandRollingAvgMs += .3f * stopwatch.ElapsedMilliseconds;

        }
    }
}
