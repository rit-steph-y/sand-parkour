using HW5_GROUP_PROJECT.sand;
using HW5_GROUP_PROJECT.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

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
        private int levelIndex = 0;

        // using this for the buttons
        private Texture2D blankTexture;
        private SpriteFont font;

        private Player player;
        private Texture2D playerSprite;
        private Vector2 playerPos;

        private Menu mainMenu;
        private Menu pauseMenu;

        private MouseState mouseState;
        private MouseState prevMouseState;
        private KeyboardState keyState;
        private KeyboardState prevKeyState;

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

            blankTexture = Content.Load<Texture2D>("blank_tex");
            font = Content.Load<SpriteFont>("NotoSansCJK-JP");

            // Set up the main menu
            // I added a placeholder image
            mainMenu = new Menu(Content.Load<Texture2D>("main_menu"), Color.White);

            mainMenu.AddButton(new Button(blankTexture, font, "Start Game", 175, 75, Color.Wheat, Color.Sienna));
            mainMenu.buttons[0].OnButtonClicked += StartLevel;

            // Set up the pause menu
            pauseMenu = new Menu(blankTexture, Color.Transparent);

            pauseMenu.AddButton(new Button(blankTexture, font, "Quit", 175, 75, Color.Wheat, Color.Sienna));
            pauseMenu.buttons[0].OnButtonClicked += Exit;
            pauseMenu.AddButton(new Button(blankTexture, font, "Main Menu", 175, 75, Color.Wheat, Color.Sienna));
            pauseMenu.buttons[1].OnButtonClicked += GoToMainMenu;
            pauseMenu.AddButton(new Button(blankTexture, font, "Resume Game", 175, 75, Color.Wheat, Color.Sienna));
            pauseMenu.buttons[2].OnButtonClicked += StartSimulation;

            playerSprite = Content.Load<Texture2D>("sandPlayerSprite");
            playerPos = new Vector2(120,120);
            this.player = new Player(playerPos, playerSprite);
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // There is no need to add anything to Game1's Update method!
        // Well I think I had to add the switch statment here unless I'm dumb - Jimmy
        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //  Exit();

            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            switch (currentState)
            {
                case GameState.MainMenu:
                    mainMenu.Update(mouseState, prevMouseState);
                    break;

                case GameState.SandSimulation:
                    if (keyState.IsKeyDown(Keys.Escape) && !prevKeyState.IsKeyDown(Keys.Escape))
                    {
                        currentState = GameState.Pause;
                    }

                    Stopwatch stopwatch = new Stopwatch();
                    stopwatch.Start();
                    this.sand.xOffset = mouseState.Position.X;
                    this.sand.yOffset = mouseState.Position.Y;
                    this.sand.Update();
                    stopwatch.Stop();
                    this.SandRollingAvgMs *= .7f;
                    this.SandRollingAvgMs += .3f * stopwatch.ElapsedMilliseconds;

                    player.Update(keyState);
                    break;

                case GameState.Pause:
                    pauseMenu.Update(mouseState, prevMouseState);
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
            Point windowSize = new(this.Window.ClientBounds.Width, this.Window.ClientBounds.Height);

            switch (currentState)
            {
                case GameState.MainMenu:
                    mainMenu.Draw(_spriteBatch);
                    break;

                case GameState.SandSimulation:
                    this.sand.Draw(this._spriteBatch, windowSize.ToVector2());
                    this.player.Draw(this._spriteBatch);
                    break;

                case GameState.Pause:
                    this.sand.Draw(this._spriteBatch,  windowSize.ToVector2());
                    pauseMenu.Draw(this._spriteBatch);
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void StartLevel()
        {
            currentScene = new Scene(levels[levelIndex]);
            currentScene.LoadLevel(sand);
            currentState = GameState.SandSimulation;
        }

        protected void StartSimulation()
        {
            currentState = GameState.SandSimulation;
        }

        protected void GoToMainMenu()
        {
            currentState = GameState.MainMenu;
        }

        protected void PauseGame()
        {
            currentState = GameState.Pause;
        }
    }
}
