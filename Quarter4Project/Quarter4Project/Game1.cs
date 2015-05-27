using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Quarter4Project
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public enum GameLevels { SPLASH, MENU, PLAY, WIN, LOSE };

        public GameLevels currentLevel;

        public static Point screenSize = new Point(1000, 600);

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameManager gameManager;
        MenuManager menuManager;

        HUD hud;

        #region Cheats

        public Boolean showDebug;
        public Boolean noClip;

        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = screenSize.X;
            graphics.PreferredBackBufferHeight = screenSize.Y;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            this.IsMouseVisible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            Maps.init(this);

            gameManager = new GameManager(this);
            menuManager = new MenuManager(this);
            hud = new HUD(this, gameManager);

            SetCurrentLevel(GameLevels.MENU);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            menuManager.Update(gameTime);
            gameManager.Update(gameTime);
            hud.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            menuManager.Draw(gameTime);
            gameManager.Draw(gameTime);
            hud.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void SetCurrentLevel(GameLevels level)
        {
            //if (currentLevel != level)
            {
                currentLevel = level;
                //splashManager.Enabled = false;
                //splashManager.Visible = false;
                menuManager.Enabled = false;
                menuManager.Visible = false;
                gameManager.Enabled = false;
                gameManager.Visible = false;
                hud.Enabled = false;
                hud.Visible = false;
                //winScreen.Enabled = false;
                //winScreen.Visible = false;
                //loseScreen.Enabled = false;
                //loseScreen.Visible = false;

                switch (currentLevel)
                {
                    case GameLevels.SPLASH:
                        //splashManager.Enabled = true;
                        //splashManager.Visible = true;
                        break;
                    case GameLevels.MENU:
                        menuManager.Enabled = true;
                        menuManager.Visible = true;
                        break;
                    case GameLevels.PLAY:
                        gameManager.Enabled = true;
                        gameManager.Visible = true;
                        hud.Enabled = true;
                        hud.Visible = true;
                        break;
                    case GameLevels.WIN:
                        //winScreen.Enabled = true;
                        //winScreen.Visible = true;
                        break;
                    case GameLevels.LOSE:
                        //loseScreen.Enabled = true;
                        //loseScreen.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        public GraphicsDeviceManager getGraphics()
        {
            return graphics;
        }

    }
}
