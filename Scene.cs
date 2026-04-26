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
        private Player player;
        private Camera camera;
        private float SandRollingAvgMs = 0;
        private Random rng;

        private SandGridComponent sand;

        internal Scene(Texture2D spriteSheet, Texture2D background, Vector2 startPos, Game game, Random rng)
        {
            this.playerStartPos = startPos;
            this.player = new(this.playerStartPos, game);
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
            this.player.Draw(spriteBatch, camera);
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
            int columns = 0;
            int rows = 0;
            foreach (Color i in colors)
            {
                Point position = new(columns, rows);
                if (i == Color.Red)
                {
                    int colorMod = rng.Next(-10, 10);
                    sand.SetPixel(position, PixelId.SAND, new Color(244 + colorMod, 164 + colorMod, 96 + colorMod));
                }
                else if (i == Color.White)
                    sand.SetPixel(position, PixelId.AIR, Color.White);
                else if (i == Color.Blue)
                {
                    int colorMod = rng.Next(-10, 10);
                    sand.SetPixel(position, PixelId.FALLING_SAND, new Color(245 + colorMod, 222 + colorMod, 179 + colorMod));
                }
                else
                    sand.SetPixel(position, PixelId.INVALID, Color.Gray);

                columns++;
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
            KeyboardState keyState = Keyboard.GetState();

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            this.sand.Update();
            stopwatch.Stop();

            player.Update(keyState, gameTime, this.sand);
            this.camera.TweenTo(this.player.Center,10,(float)gameTime.ElapsedGameTime.TotalSeconds);

            this.SandRollingAvgMs *= .7f;
            this.SandRollingAvgMs += .3f * stopwatch.ElapsedMilliseconds;

        }
    }
}
