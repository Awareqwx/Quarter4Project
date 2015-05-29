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
    public class WinManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        Game1 myGame;

        int panelNum;
        Texture2D[] panels;
        Texture2D currentPanel;
        Color washColor;
        int fadeStage, comicStage;
        int fadeElapsed, comicElapsed;

        SoundEffectInstance blast;
        SoundEffectInstance crash;

        SpriteFont font;

        SpriteBatch spriteBatch;

        public WinManager(Game1 game)
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
            font = myGame.Content.Load<SpriteFont>(@"Fonts\Font");

            blast = myGame.Content.Load<SoundEffect>(@"Audio\LaserCannon").CreateInstance();
            crash = myGame.Content.Load<SoundEffect>(@"Audio\Collapse").CreateInstance();

            spriteBatch = new SpriteBatch(myGame.GraphicsDevice);

            washColor = Color.Black;

            panels = new Texture2D[11];

            for (int i = 1; i <= 11; i++)
            {
                panels[i - 1] = myGame.Content.Load<Texture2D>(@"Images\Win\end_card_" + i);
            }
            panelNum = 0;
            currentPanel = panels[0];
            fadeStage = 1;
            comicStage = 1;
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }
            
            switch(comicStage)
            {
                case 1:
                    if(fadeIn(gameTime))
                    {
                        comicStage++;
                    }
                    break;
                case 2:
                    comicElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if(comicElapsed >= 1500)
                    {
                        comicElapsed = 0;
                        comicStage++;
                    }
                    break;
                case 3:
                    panelNum++;
                    currentPanel = panels[panelNum];
                    comicStage++;
                    myGame.gameManager.Walk.Play();
                    break;
                case 4:
                    comicElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if(comicElapsed >= 1500)
                    {
                        comicElapsed = 0;
                        comicStage++;
                    }
                    break;                
                case 5:
                    if(fadePanel(gameTime, 2000))
                    {
                        comicStage++;
                    }
                    break;
                case 6:
                    blast.Play();
                    if(fadePanel(gameTime, 150))
                    {
                        comicStage++;
                    }
                    break;
                case 7:
                    panelNum++;
                    currentPanel = panels[panelNum];
                    comicStage++;
                    crash.Play();
                    break;
                case 8:
                    comicElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if(comicElapsed >= 1400)
                    {
                        comicElapsed = 0;
                        comicStage++;
                    }
                    break;
                case 9:
                    if (fadeOut(gameTime))
                    {
                        comicStage++;
                    }
                    break;
                case 10:
                    comicElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if(comicElapsed >= 2500)
                    {
                        comicElapsed = 0;
                        comicStage++;
                        panelNum++;
                        currentPanel = panels[panelNum];
                    }
                    break;
                case 11:
                    if (fadeIn(gameTime))
                    {
                        comicStage++;
                        myGame.menuManager.startSound();
                    }
                    break;
                case 12:
                    comicElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if(comicElapsed >= 3000)
                    {
                        comicElapsed = 0;
                        comicStage++;
                    }
                    break;
                case 13:
                    if (fadePanel(gameTime, 3000))
                    {
                        comicStage++;
                    }
                    break;
                case 14:
                    if (fadePanel(gameTime, 3000))
                    {
                        comicStage++;
                    }
                    break;
                case 15:
                    if (fadePanel(gameTime, 5000))
                    {
                        comicStage++;
                    }
                    break;
                case 16:
                    if (fadePanel(gameTime, 5000))
                    {
                        comicStage++;
                    }
                    break;
                case 17:
                    if (fadeOut(gameTime))
                    {
                        comicStage++;
                    }
                    break;
                case 18:
                    comicElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if (comicElapsed >= 2000)
                    {
                        myGame.SetCurrentLevel(Game1.GameLevels.MENU);
                        myGame.Reset();
                    }
                    break;
            }

            // TODO: Add your update code here

            base.Update(gameTime);
        }

        public Boolean fadePanel(GameTime gameTime, int t)
        {
            switch (fadeStage)
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
                            fadeStage++;
                        }
                    }
                    break;
                case 2:
                    if (panelNum + 1 < panels.Length)
                    {
                        panelNum++;
                        currentPanel = panels[panelNum];
                    }
                    fadeStage++;
                    break;
                case 3:
                    if (fadeIn(gameTime))
                    {
                        fadeStage++;
                    }
                    break;
                case 4:
                    fadeElapsed += gameTime.ElapsedGameTime.Milliseconds;
                    if (fadeElapsed >= t)
                    {
                        fadeStage = 1;
                        fadeElapsed = 0;
                        return true;
                    }
                    break;
            }
            return false;
        }

        public Boolean fadeOut(GameTime gameTime)
        {
            double modifier = 0.75;
            Boolean done = false;
            int r;
            int g;
            int b;
            r = g = b = washColor.R;

            r = g = b -= (int)((double)gameTime.ElapsedGameTime.Milliseconds * modifier);

            if (r <= 0 || g <= 0 || b <= 0)
            {
                r = g = b = 0;
            }

            washColor.R = (byte)r;
            washColor.G = (byte)g;
            washColor.B = (byte)b;

            if (washColor == Color.Black)
            {
                done = true;
            }

            return done;
        }

        public Boolean fadeIn(GameTime gameTime)
        {
            double modifier = 0.75;
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
            if (!Visible)
            {
                return;
            }

            spriteBatch.Begin();

            spriteBatch.Draw(currentPanel, new Rectangle(244, 44, 512, 512), washColor);
            spriteBatch.DrawString(font, comicStage + "", Vector2.Zero, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void resetComic()
        {
            comicElapsed = 0;
            comicStage = 1;
            panelNum = 0;
            currentPanel = panels[0];
        }

    }
}