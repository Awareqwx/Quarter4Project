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
    public class MenuManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        Texture2D menuBackground;
        Texture2D logo;
        Texture2D Name;
        Texture2D cursorImage;
        Texture2D buttonImage;

        SpriteFont fontLarge;

        public Game1 myGame;

        public MenuCursor cursor;

        MenuButton[] buttons;

        SpriteBatch spriteBatch;

        public MenuManager(Game1 game)
            : base(game)
        {
            myGame = game;
            Initialize();
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
            cursorImage = myGame.Content.Load<Texture2D>(@"Images\Test\Cursor");
            menuBackground = myGame.Content.Load<Texture2D>(@"Images\Backgrounds\TitleBackground");
            logo = myGame.Content.Load<Texture2D>(@"Images\PARADOX LOGO");
            Name = myGame.Content.Load<Texture2D>(@"Images\UpgradeTitle");
            buttonImage = myGame.Content.Load<Texture2D>(@"Images\MenuButton");
            fontLarge = myGame.Content.Load<SpriteFont>(@"Fonts\FontLarge");

            cursor = new MenuCursor(cursorImage, this);

            buttons = new MenuButton[] { 
                new MenuButton(new Vector2(80, 200), MenuButton.ButtonAction.PLAY, buttonImage, "Play", fontLarge, this), 
                new MenuButton(new Vector2(80, 300), MenuButton.ButtonAction.OPTIONS, buttonImage, "Options", fontLarge, this) 
            };

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if(!Enabled)
            {
                return;
            }

            cursor.Update(gameTime);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Update(gameTime);
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

            spriteBatch.Draw(menuBackground, new Rectangle(0, 0, 1000, 600), Color.White);
            spriteBatch.Draw(logo, new Rectangle(780, 516, 220, 84), Color.White);
            spriteBatch.Draw(Name, new Rectangle(40, 40, 600, 87), Color.White);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].Draw(gameTime, spriteBatch);
            }

            cursor.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
