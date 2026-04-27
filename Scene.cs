using HW5_GROUP_PROJECT.sand;
using HW5_GROUP_PROJECT.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HW5_GROUP_PROJECT
{
    internal class Scene
    {
        internal static Background defaultBackground { get; set; }
        private Background background;
        private Texture2D spriteSheet;

        private Vector2 playerStartPos;
        private Vector2 flagPos;

        private Player player;
        private Flag flag;
        private Camera camera;

        private float SandRollingAvgMs = 0;
        private Random rng;

        private SandGridComponent sand;

        internal Scene(Texture2D spriteSheet, Texture2D background, Game game, Random rng)
        {
            this.camera = new();
            this.camera.Zoom = new(2);

            this.sand = new(game.GraphicsDevice);
            this.spriteSheet = spriteSheet;

            if (background != null)
            {
                this.background = new Background(background, Color.White);
            }
            else
            {
                this.background = defaultBackground;
            }

            this.rng = rng;
        }

        internal void Draw(Rectangle clientBounds,SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);

            this.camera.ClientBounds = new(clientBounds.Width, clientBounds.Height);
            
            this.sand.Draw(spriteBatch, camera);
            flag.Draw(spriteBatch, camera);
            this.player.Draw(spriteBatch, camera);
        }

        internal void LoadLevel(Game game)
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
                    int colorMod = rng.Next(-10, 10);
                    sand.SetPixel(columns, rows, PixelId.SAND, new Color(244 + colorMod, 164 + colorMod, 96 + colorMod));
                    columns++;
                }
                else if (i == Color.White)
                {
                    sand.SetPixel(columns, rows, PixelId.AIR, Color.White);
                    columns++;
                }
                else if (i == Color.Blue)
                {
                    int colorMod = rng.Next(-10, 10);
                    sand.SetPixel(columns, rows, PixelId.FALLING_SAND, new Color(245 + colorMod, 222 + colorMod, 179 + colorMod));
                    columns++;
                }
                // added start & end
                // R:0,G:255,B:0,A:255
                else if (i == Color.Lime)
                {
                    playerStartPos = new Vector2(columns, rows);
                    sand.SetPixel(columns, rows, PixelId.AIR, Color.White);
                    columns++;
                }
                else if (i == Color.Yellow)
                {
                    flagPos = new Vector2(columns, rows);
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
            this.player = new(this.playerStartPos, game);

            flag = new Flag(flagPos);
        }

        internal void Update(GameTime gameTime, SandGame game)
        {
            MouseState mouseState = Mouse.GetState();
            KeyboardState keyState = Keyboard.GetState();

            Stopwatch stopwatch = new Stopwatch();

            this.camera.Center = new(mouseState.Position.X, mouseState.Position.Y);

            stopwatch.Start();
            this.sand.Update();
            stopwatch.Stop();

            player.Update(keyState, gameTime, this.sand);
            flag.Update(player, game);

            this.SandRollingAvgMs *= .7f;
            this.SandRollingAvgMs += .3f * stopwatch.ElapsedMilliseconds;
        }
    }
}
