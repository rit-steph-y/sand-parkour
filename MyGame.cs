using HW5_GROUP_PROJECT.extern_testing;
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

        public SandGame()
        {

            FallingSand sand = new();
            sand.Dispose();
            Debugger.Log(0,null, $"{"owo"}");
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);

            _spriteBatch.Begin();

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
