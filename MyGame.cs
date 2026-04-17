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

        // using this for the buttons
        private Texture2D blankTexture;
        private SpriteFont font;

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
            blankTexture = Content.Load<Texture2D>("blank_tex");
            font = Content.Load<SpriteFont>("NotoSansCJK-JP");

            // Set up the main menu
            // I added a placeholder image
            mainMenu = new Menu(Content.Load<Texture2D>("main_menu"), Color.White);

            mainMenu.AddButton(new Button(blankTexture, font, "Start Game", 175, 75, Color.Wheat, Color.Sienna));
            mainMenu.buttons[0].OnButtonClicked += StartSimulation;

            // Set up the pause menu
            pauseMenu = new Menu(blankTexture, Color.Transparent);

            pauseMenu.AddButton(new Button(blankTexture, font, "Quit", 175, 75, Color.Wheat, Color.Sienna));
            pauseMenu.buttons[0].OnButtonClicked += Exit;
            pauseMenu.AddButton(new Button(blankTexture, font, "Resume Game", 175, 75, Color.Wheat, Color.Sienna));
            pauseMenu.buttons[1].OnButtonClicked += StartSimulation;

            /// here it is, glorious monogame texture loading.
            //this is it. this is the code that loads a texture, in this case, rick astley
            Texture2D spriteSheet = Content.Load<Texture2D>("testLevel");

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
                    // Color.Red is exactly #ff0000 or 255 0 0
                    // if it is off by even one this will not work
                    sand.SetPixel(columns, rows, PixelId.SAND, Color.SandyBrown);
                    columns++;
                }
                else if (i == Color.White)
                {
                    // Color.Red is exactly #ff0000 or 255 0 0
                    sand.SetPixel(columns, rows, PixelId.AIR, Color.White);
                    columns++;
                }
                else if (i == Color.Blue)
                {
                    //
                }
                else
                {
                    sand.SetPixel(columns, rows, PixelId.INVALID, Color.Black);
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
                    this.sand.Update();
                    stopwatch.Stop();
                    this.SandRollingAvgMs *= .7f;
                    this.SandRollingAvgMs += .3f * stopwatch.ElapsedMilliseconds;
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

            switch (currentState)
            {
                case GameState.MainMenu:
                    mainMenu.Draw(_spriteBatch);
                    break;

                case GameState.SandSimulation:
                    this.sand.Draw(this._spriteBatch);
                    break;

                case GameState.Pause:
                    this.sand.Draw(this._spriteBatch);
                    pauseMenu.Draw(this._spriteBatch);
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void StartSimulation()
        {
            currentState = GameState.SandSimulation;
        }

        protected void PauseGame()
        {
            currentState = GameState.Pause;
        }
    }
}
