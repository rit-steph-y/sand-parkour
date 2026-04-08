using HW5_GROUP_PROJECT.sand;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Threading;

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
