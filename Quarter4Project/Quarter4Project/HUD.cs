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
    public class HUD : Microsoft.Xna.Framework.DrawableGameComponent
    {
        Texture2D healthBarLiquidTex, healthBarVialTex, healthBarPlungerTex;

        CompoundSprite healthBarVial, healthBarLiquid, healthBarPlunger;

        Vector2 healthBarOffset, minimapOffset;

        Game1 myGame;
        GameManager gm;

        public HUD(Game1 game, GameManager g)
            : base(game)
        {
            myGame = game;
            gm = g;

            healthBarOffset = new Vector2(10, 40);

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

            healthBarLiquidTex = myGame.Content.Load<Texture2D>(@"Images\HealthBar\SyringeLiquid");
            healthBarVialTex = myGame.Content.Load<Texture2D>(@"Images\HealthBar\SyringeVial");
            healthBarPlungerTex = myGame.Content.Load<Texture2D>(@"Images\HealthBar\SyringePlunger");

            healthBarLiquid = new CompoundSprite(new Texture2D[] { healthBarLiquidTex }, healthBarOffset, new Point(63, 19), new Color[] { Color.Red });
            healthBarVial = new CompoundSprite(new Texture2D[] { healthBarVialTex }, healthBarOffset, new Point(72, 19));
            healthBarPlunger = new CompoundSprite(new Texture2D[] { healthBarPlungerTex }, healthBarOffset, new Point(84, 19));

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            if(myGame.showDebug && healthBarOffset.Y == 10)
            {
                healthBarOffset.Y = 40;
                healthBarVial.setPos(healthBarOffset);
                healthBarLiquid.setPos(healthBarOffset);
            }
            else if (!myGame.showDebug && healthBarOffset.Y == 40)
            {
                healthBarOffset.Y = 10;
                healthBarVial.setPos(healthBarOffset);
                healthBarLiquid.setPos(healthBarOffset);
            }
            float a = gm.testPlayer.hp;
            float b = gm.testPlayer.hpMax;
            float f = (a / b) * 42;
            healthBarPlunger.setPos(new Vector2(f + healthBarOffset.X, healthBarOffset.Y));
            healthBarLiquid.setFS(new Point((int)(21 + f), 19));

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            healthBarLiquid.Draw(gameTime, spriteBatch);
            healthBarPlunger.Draw(gameTime, spriteBatch);
            healthBarVial.Draw(gameTime, spriteBatch);
            
            base.Draw(gameTime);
        }

        /*public void generateMinimap (GameManager.Map m)
        {

        }*/

    }
}
