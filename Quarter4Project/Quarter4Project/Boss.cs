using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Boss : Enemy
    {

        public enum BossBehaviors { SPAWN, IDLE, RADIAL, DIRECT, SUMMON, DIE };

        Random r;
        int time;
        Boolean done;

        Texture2D[] goblinTextures;

        BossBehaviors behavior;

        public Boss(Texture2D[] t, Vector2 p, GameManager g, int h) : base(t, p, 0, g, h, 1)
        {
            type = "Boss";
            colors = new Color[] { Color.White, Color.White };
            attackSprite = myGame.myGame.Content.Load<Texture2D>(@"Images\Test\RocketProjectile");
            r = new Random();
            behavior = BossBehaviors.IDLE;
            goblinTextures = new Texture2D[] { myGame.myGame.Content.Load<Texture2D>(@"Images\Enemies\GoblinBase"), myGame.myGame.Content.Load<Texture2D>(@"Images\Enemies\GoblinLights") };
        }

        public override void Update(GameTime gameTime)
        {

            if(currentSet.name == "DIE" && animIsOver)
            {
                myGame.won = true;
            }

            time += gameTime.ElapsedGameTime.Milliseconds;

            if(hp <= 0)
            {
                behavior = BossBehaviors.DIE;
            }
            if (!done)
            {
                switch (behavior)
                {
                    case BossBehaviors.IDLE:
                        setAnimation("IDLE");
                        break;
                    case BossBehaviors.DIRECT:
                        setAnimation("ATTACK");
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, (float)Math.Atan2(myGame.player.getCenter().Y - (getCenter() + new Vector2(0, -10)).Y, myGame.player.getCenter().X - (getCenter() + new Vector2(0, -10)).X), myGame));
                        break;
                    case BossBehaviors.RADIAL:
                        setAnimation("ATTACK");
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, 0, myGame));
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, (float)(Math.PI / 4), myGame));
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, (float)(Math.PI / 2), myGame));
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, (float)(3 * Math.PI / 4), myGame));
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, (float)(Math.PI), myGame));
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, (float)(5 * Math.PI / 4), myGame));
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, (float)(3 * Math.PI / 2), myGame));
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 10, (getCenter() + new Vector2(0, 0)), 15, (float)(7 * Math.PI / 4), myGame));
                        break;
                    case BossBehaviors.SUMMON:
                        myGame.currentMap.enemyList.Add(new Goblin(goblinTextures, getCenter() + new Vector2(500, 0), 2, myGame, 10, 3));
                        myGame.currentMap.enemyList.Add(new Goblin(goblinTextures, getCenter() - new Vector2(500, 0), 2, myGame, 10, 3));
                        break;
                }
                done = true;
            }
            else
            {
                if(animIsOver)
                {
                    setAnimation("IDLE");
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public override void addAnimations()
        {
            sets.Add(new AnimationSet("IDLE", new Point(256, 512), new Point(8, 1), new Point(0, 0), 100, true));
            sets.Add(new AnimationSet("ATTACK", new Point(256, 512), new Point(2, 1), new Point(0, 1), 1000, false));
            sets.Add(new AnimationSet("HURT", new Point(256, 512), new Point(1, 1), new Point(0, 2), 100, false));
            sets.Add(new AnimationSet("DIE", new Point(256, 512), new Point(6, 1), new Point(0, 3), 300, false));

            setAnimation("IDLE");
        }

        public override void setBehavior()
        {
            if(time >= 2500 && behavior != BossBehaviors.DIE)
            {
                done = false;
                time = 0;
                switch (r.Next(4))
                {
                    default:
                    case 0:
                        behavior = BossBehaviors.IDLE;
                        break;
                    case 1:
                        behavior = BossBehaviors.DIRECT;
                        break;
                    case 2:
                        behavior = BossBehaviors.RADIAL;
                        break;
                    case 3:
                        behavior = BossBehaviors.SUMMON;
                        break;
                }
            }
        }

    }
}
