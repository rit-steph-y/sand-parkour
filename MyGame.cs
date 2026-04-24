using HW5_GROUP_PROJECT.sand;
using HW5_GROUP_PROJECT.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;

namespace HW5_GROUP_PROJECT
{
    internal enum GameState
    {
        MainMenu,
        SandSimulation,
        Pause
    }

    public class SandGame : Game
    {
        // Fields created by the MG template
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameState currentState;
        private Scene currentScene;
        private Texture2D[] levels;
        private Texture2D[] backgrounds;
        private int levelIndex = 0;

        private Texture2D backgroundTex;
        private Texture2D blankTexture;
        private SpriteFont titleFont;
        private SpriteFont font;

        private Menu currentMenu;

        private MouseState mouseState;
        private MouseState prevMouseState;
        private KeyboardState keyState;
        private KeyboardState prevKeyState;

        private Random rng = new Random();

        private Color bgColor = Color.White;

        public SandGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            Menu.Rect = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Add new levels to this array
            levels = 
                [
                    Content.Load<Texture2D>("testLevel"),
                    Content.Load<Texture2D>("testLevel2")
                ];
            // Add corresponding backgrounds here
            backgrounds =
                [
                    null,
                    null,
                    // <a href="https://www.vecteezy.com/free-photos/desert">Desert Stock photos by Vecteezy</a>
                    Content.Load<Texture2D>("background2")
                ];

            backgroundTex = Content.Load<Texture2D>("background");
            Scene.defaultBackground = new Background(backgroundTex, Color.White);

            blankTexture = Content.Load<Texture2D>("blank_tex");
            titleFont = Content.Load<SpriteFont>("TitleFont");
            font = Content.Load<SpriteFont>("NotoSansCJK-JP");

            MainMenu();
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // There is no need to add anything to Game1's Update method!
        protected override void Update(GameTime gameTime)
        {

            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            switch (currentState)
            {
                case GameState.MainMenu:
                    currentMenu.Update(mouseState, prevMouseState);
                    break;

                case GameState.SandSimulation:
                    if (keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
                    {
                        PauseGame();
                    }

                    this.currentScene.Update(gameTime);
                    break;

                case GameState.Pause:
                    currentMenu.Update(mouseState, prevMouseState);
                    if (keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
                    {
                        StartSimulation();
                    }

                    break;
            }

            prevMouseState = mouseState;
            prevKeyState = keyState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bgColor);

            _spriteBatch.Begin();
            
            switch (currentState)
            {
                case GameState.MainMenu:
                    currentMenu.Draw(_spriteBatch);
                    break;

                case GameState.SandSimulation:
                    this.currentScene.Draw(this.Window.ClientBounds, this._spriteBatch);
                    break;

                case GameState.Pause:
                    this.currentScene.Draw(this.Window.ClientBounds, this._spriteBatch);
                    currentMenu.Draw(this._spriteBatch);
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void NextLevel()
        {
            if (levelIndex + 1 < levels.Length)
            {
                levelIndex++;
                StartLevel();
            }

            else
            {
                EndScreen();
            }
        }

        private void RestartGame()
        {
            levelIndex = 0;
            StartLevel();
        }

        private void StartLevel()
        {
            currentScene = new Scene(levels[levelIndex], backgrounds[levelIndex], new Vector2(120,120), this, rng);
            currentScene.LoadLevel();
            currentState = GameState.SandSimulation;
        }

        private void StartSimulation()
        {
            currentState = GameState.SandSimulation;
        }

        protected void MainMenu()
        {
            levelIndex = 0;

            currentMenu = new Menu(backgroundTex, Color.White);

            // Buttons
            currentMenu.AddButton(blankTexture, font, "Quit", 175, 75, Color.Wheat, Color.Sienna);
            currentMenu.buttons[0].OnButtonClicked += Exit;
            currentMenu.AddButton(blankTexture, font, "Start Game", 175, 75, Color.Wheat, Color.Sienna);
            currentMenu.buttons[1].OnButtonClicked += StartLevel;

            // Text
            currentMenu.AddText(titleFont, "SAND", Color.Black);

            currentState = GameState.MainMenu;
        }

        protected void PauseGame()
        {
            currentMenu = new Menu(blankTexture, Color.Transparent);

            // Buttons
            currentMenu.AddButton(blankTexture, font, "Quit", 165, 60, Color.Wheat, Color.Sienna);
            currentMenu.buttons[0].OnButtonClicked += Exit;
            currentMenu.AddButton(blankTexture, font, "Main Menu", 165, 60, Color.Wheat, Color.Sienna);
            currentMenu.buttons[1].OnButtonClicked += MainMenu;
            currentMenu.AddButton(blankTexture, font, "Skip Level\n(Debug)", 165, 60, Color.Wheat, Color.Sienna);
            currentMenu.buttons[2].OnButtonClicked += NextLevel;
            currentMenu.AddButton(blankTexture, font, "Restart Level", 165, 60, Color.Wheat, Color.Sienna);
            currentMenu.buttons[3].OnButtonClicked += StartLevel;
            currentMenu.AddButton(blankTexture, font, "Resume Game", 165, 60, Color.Wheat, Color.Sienna);
            currentMenu.buttons[4].OnButtonClicked += StartSimulation;

            // Text
            currentMenu.AddText(titleFont, "Paused", Color.Black);

            currentState = GameState.Pause;
        }

        private void EndScreen()
        {
            currentMenu = new Menu(backgrounds.Last(), Color.White);

            // Buttons
            currentMenu.AddButton(blankTexture, font, "Quit", 165, 60, Color.Wheat, Color.Sienna);
            currentMenu.buttons[0].OnButtonClicked += Exit;
            currentMenu.AddButton(blankTexture, font, "Restart Game", 165, 60, Color.Wheat, Color.Sienna);
            currentMenu.buttons[1].OnButtonClicked += RestartGame;

            // Text
            currentMenu.AddText(titleFont, "END", Color.Black);
            currentMenu.AddText(font, "You Won!", new Vector2(Menu.Rect.Width - 100, 75), Color.Black);
        }
    }
}
