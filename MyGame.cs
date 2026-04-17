using HW5_GROUP_PROJECT.sand;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace HW5_GROUP_PROJECT
{
    public class SandGame : Game
    {
        // Fields created by the MG template
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Color bgColor = Color.White;
        private Random rng = new Random();
        private SandGridComponent sand;

        private float SandRollingAvgMs = 0;

        public SandGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            this.sand = new(this.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {

            /// here it is, glorious monogame texture loading.
            //this is it. this is the code that loads a texture, in this case, rick astley
            Texture2D spriteSheet = Content.Load<Texture2D>("testLevel2");

            //then initializes an array of colors the exact size of said texture
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
                    sand.SetPixel(columns, rows, PixelId.SAND);
                    columns++;
                }
                else if (i == Color.White)
                {
                    sand.SetPixel(columns, rows, PixelId.AIR);
                    columns++;
                }
                else
                {
                    sand.SetPixel(columns, rows, PixelId.AIR);
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

            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // There is no need to add anything to Game1's Update method!
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            this.sand.Update();
            stopwatch.Stop();
            this.SandRollingAvgMs *= .7f;
            this.SandRollingAvgMs += .3f * stopwatch.ElapsedMilliseconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);

            _spriteBatch.Begin();
            this.sand.Draw(this._spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
