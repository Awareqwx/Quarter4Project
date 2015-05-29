using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Enemy : Entity
    {

        public enum BehaviorMode { ATTACK, ADVANCE, FLEE, IDLE, DIE };

        int shotTimer, shotDelay;
        protected BehaviorMode mode;

        protected int hp, hpMax;
        protected int level;

        Boolean flip;

        protected Texture2D attackSprite;

        public Enemy(Texture2D[] t, Vector2 p, float s, GameManager g, int h, int l)
            : base(t, p, g)
        {
            speed = s;
            hp = hpMax = h;
            type = "Enemy";
            shotTimer = 0;
            shotDelay = 750;
            mode = BehaviorMode.IDLE;
            setLevel(l);
            attackSprite = myGame.myGame.Content.Load<Texture2D>(@"Images\Test\Fireball");
            addAnimations();
        }

        public Enemy(Texture2D[] t, Vector2 p, float s, GameManager g, int h, Color c)
            : base(t, p, g)
        {
            speed = s;
            hp = hpMax = h;
            type = "Enemy";
            shotTimer = 0;
            shotDelay = 750;
            mode = BehaviorMode.IDLE;
            attackSprite = myGame.myGame.Content.Load<Texture2D>(@"Images\Test\Fireball");
            colors[0] = c;
            setLevel(1);
            addAnimations();
        }

        public override void Update(GameTime gameTime)
        {
            if(position.Y > 10000)
            {
                deleteMe = true;
            }

            if (hp > 0)
            {
                setBehavior();
            }
            else
            {
                if (currentSet.name == "DIE" && animIsOver)
                {
                    if (deleteMe == false)
                    {
                        myGame.currentMap.scrapList.Add(new Scrap( myGame.getScrapImage(1), getCenter(), myGame, level));
                    }
                    deleteMe = true;
                }
                else
                {
                    if (currentSet.name != "DIE")
                    {
                        setAnimation("DIE");
                        myGame.EnemyDeath.Play();
                    }
                    mode = BehaviorMode.DIE;
                    direction.X = 0;
                }
            }

            switch (mode)
            {
                case BehaviorMode.IDLE:
                    if (!isFalling)
                    {
                        setAnimation("IDLE");
                    }
                    direction.X = 0;
                    break;
                case BehaviorMode.ADVANCE:
                    if (!isFalling)
                    {
                        setAnimation("WALK");
                    }
                    if(position.X > myGame.player.getPos().X)
                    {
                        direction.X = -1;
                        flip = true;
                    }
                    else
                    {
                        direction.X = 1;
                        flip = false;
                    }
                    break;
                case BehaviorMode.FLEE:
                    if (!isFalling)
                    {
                        setAnimation("WALK");
                    }
                    if (position.X > myGame.player.getPos().X)
                    {
                        direction.X = 1;
                        flip = false;
                    }
                    else
                    {
                        direction.X = -1;
                        flip = true;
                    }
                    break;
                case BehaviorMode.ATTACK:
                    if (!isFalling)
                    {
                        setAnimation("SHOOT");
                    }
                    direction.X = 0;
                    flip = position.X - myGame.player.getPos().X > 1;
                    if(shotTimer <= 0)
                    {
                        myGame.enemyAttacks.Add(new Projectile(attackSprite, Color.White, 5, (getCenter() + new Vector2(0, 0)), 5, (float)Math.Atan2(myGame.player.getCenter().Y - (getCenter() + new Vector2(0, -10)).Y, myGame.player.getCenter().X - (getCenter() + new Vector2(0, -10)).X), myGame));
                        shotTimer = shotDelay;
                    }
                    break;
            }
            
            base.Update(gameTime);

            position += direction * speed;

            foreach(Attack a in myGame.friendlyAttacks)
            {
                if (!a.deleteMe && currentSet.name != "DIE")
                {
                    if (collisionRect().Intersects(a.collisionRect()))
                    {
                        takeDamage(a.damage);
                        a.deleteMe = true;
                    }
                }
            }

            shotTimer -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void addAnimations()
        {
            AnimationSet idle = new AnimationSet("IDLE", new Point(80, 80), new Point(1, 1), new Point(0, 0), 1000, false);
            AnimationSet walk = new AnimationSet("WALK", new Point(80, 80), new Point(5, 1), new Point(0, 1), 100, true);
            AnimationSet shoot = new AnimationSet("SHOOT", new Point(83, 80), new Point(2, 1), 150, new Point(400, 0), false);
            AnimationSet hurt = new AnimationSet("HURT", new Point(80, 80), new Point(1, 1), new Point(0, 3), 150, false);
            AnimationSet die = new AnimationSet("DIE", new Point(83, 80), new Point(2, 2), 300, new Point(400, 81), false);
            sets.Add(idle);
            sets.Add(walk);
            sets.Add(shoot);
            sets.Add(hurt);
            sets.Add(die);
            setAnimation("IDLE");

            base.addAnimations();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                if (flip)
                {
                    spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i]);
                }
                else
                {
                    spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i], 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
        }

        public override void collide(int i)
        {
            switch (i)
            {
                case 0:
                    position.Y -= position.Y % 40 - 2;
                    break;
                case 1:
                    position.Y += 38 - ((position.Y - 1) % 40);
                    break;
                case 2:
                    position.X -= position.X % 40;
                    break;
                case 3:
                    position.X += 38 - ((position.X - 1) % 40);
                    break;
            }
        }

        public void takeDamage(int d)
        {
            hp -= d;
            if (hp < 0)
            {
                hp = 0;
            }
        }

        public void heal(int h)
        {
            hp += h;
            if (hp > hpMax)
            {
                hp = hpMax;
            }
        }

        public virtual void setBehavior()
        {
            if (Collision.getDistance(myGame.player.getPos(), getPos()) > 500 || myGame.myGame.noClip)
            {
                mode = BehaviorMode.IDLE;
            }
            else if (Collision.getDistance(myGame.player.getPos(), getPos()) > 350)
            {
                mode = BehaviorMode.ADVANCE;
            }
            else if (Collision.getDistance(myGame.player.getPos(), getPos()) < 200)
            {
                mode = BehaviorMode.FLEE;
            }
            else
            {
                mode = BehaviorMode.ATTACK;
            }
        }

        public void setLevel(int l)
        {
            level = l;
            switch (level)
            {
                case 1:
                    colors[1] = Color.Yellow;
                    break;
                case 2:
                    colors[1] = Color.Red;
                    break;
                case 3:
                    colors[1] = Color.Purple;
                    break;
                case 4:
                    colors[1] = Color.Blue;
                    break;
                default:
                case 5:
                    colors[1] = Color.Cyan;
                    break;
            }
        }

    }
}
