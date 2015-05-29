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
    public class ComicManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        Game1 myGame;

        int panelNum;
        Texture2D[] panels;
        Texture2D currentPanel;
        Color washColor;
        int stage;
        int elapsed;

        SpriteBatch spriteBatch;

        public ComicManager(Game1 game)
            : base(game)
        {
            // TODO: Construct any child components here
            myGame = game;
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

            washColor = Color.White;

            panels = new Texture2D[7];

            for (int i = 0; i <= 6; i++)
            {
                panels[i] = myGame.Content.Load<Texture2D>(@"Images\Comic\intro_card_" + i);
            }
            panelNum = 0;
            currentPanel = panels[0];
            stage = 1;
                base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if(!Enabled)
            {
                return;
            }
            switch (stage)
            {
                case 1:
                    if (fadeOut(gameTime))
                    {
                        if (panelNum + 1 >= panels.Length)
                        {
                            myGame.SetCurrentLevel(Game1.GameLevels.PLAY);
                        }
                        else
                        {
                            stage++;
                        }
                    }
                    break;
                case 2:
                    if (panelNum + 1 < panels.Length)
                    {
                        panelNum++;
                        currentPanel = panels[panelNum];
                    }
                    stage++;
                    break;
                case 3:
                    if (fadeIn(gameTime))
                    {
                        stage++;
                    }
                    break;
                case 4:
                    elapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if(elapsed >= 3000)
                    {
                        stage = 1;
                        elapsed = 0;
                    }
                    break;
            }

            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public Boolean fadeOut(GameTime gameTime)
        {
            double modifier = 1;
            Boolean done = false;
            int r;
            int g;
            int b;
            r = g = b = washColor.R;

            r = g = b -= (int)((double)gameTime.ElapsedGameTime.Milliseconds * modifier);

            if(r <= 0 || g <= 0 || b <= 0)
            {
                r = g = b = 0;
            }

            washColor.R = (byte)r;
            washColor.G = (byte)g;
            washColor.B = (byte)b;

            if(washColor == Color.Black)
            {
                done = true;
            }

            return done;
        }

        public Boolean fadeIn(GameTime gameTime)
        {
            double modifier = 1;
            Boolean done = false;
            int r;
            int g;
            int b;
            r = g = b = washColor.R;

            r = g = b += (int)((double)gameTime.ElapsedGameTime.Milliseconds * modifier);

            if (r >= 255 || g >= 255 || b >= 255)
            {
                r = g = b = 255;
            }

            washColor.R = (byte)r;
            washColor.G = (byte)g;
            washColor.B = (byte)b;

            if (washColor == Color.White)
            {
                done = true;
            }

            return done;
        }

        public override void Draw(GameTime gameTime)
        {
            if(!Visible)
            {
                return;
            }

            spriteBatch.Begin();

            spriteBatch.Draw(currentPanel, new Rectangle(244, 44, 512, 512), washColor);

            spriteBatch.End();
            
            base.Draw(gameTime);
        }

        public void resetComic()
        {
            panelNum = 0;
            currentPanel = panels[0];
        }

    }
}