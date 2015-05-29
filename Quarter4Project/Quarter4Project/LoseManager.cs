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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class LoseManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        Game1 myGame;
        int timer;

        Texture2D loseScreen;

        SpriteBatch spriteBatch;

        public LoseManager(Game1 game)
            : base(game)
        {
            myGame = game;
            // TODO: Construct any child components here
            Initialize();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
            loseScreen = myGame.Content.Load<Texture2D>(@"Images\Backgrounds\game_over");

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            if (!Enabled)
            {
                return;
            }

            timer += gameTime.ElapsedGameTime.Milliseconds;

            if(timer >= 6000)
            {
                myGame.Reset();
                myGame.SetCurrentLevel(Game1.GameLevels.MENU);
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }

            spriteBatch.Begin();

            spriteBatch.Draw(loseScreen, new Rectangle(244, 44, 512, 512), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
