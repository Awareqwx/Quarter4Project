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
        BehaviorMode mode;

        int hp, hpMax;

        Boolean flip;

        Texture2D fireball;

        public Enemy(Texture2D[] t, Vector2 p, float s, GameManager g, int h)
            : base(t, p, g)
        {
            speed = s;
            hp = hpMax = h;
            type = "Enemy";
            shotTimer = 0;
            shotDelay = 750;
            mode = BehaviorMode.IDLE;
            fireball = myGame.myGame.Content.Load<Texture2D>(@"Images\Test\Fireball");
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
            fireball = myGame.myGame.Content.Load<Texture2D>(@"Images\Test\Fireball");
            colors[0] = c;
            addAnimations();
        }

        public override void Update(GameTime gameTime)
        {
            if (Collision.getDistance(myGame.testPlayer.getPos(), getPos()) > 500 || myGame.myGame.noClip)
            {
                mode = BehaviorMode.IDLE;
            }
            else if(Collision.getDistance(myGame.testPlayer.getPos(), getPos()) > 350)
            {
                mode = BehaviorMode.ADVANCE;
            }
            else if (Collision.getDistance(myGame.testPlayer.getPos(), getPos()) < 200)
            {
                mode = BehaviorMode.FLEE;
            }
            else
            {
                mode = BehaviorMode.ATTACK;
            }

            if (hp <= 0)
            {
                if (currentSet.name == "DIE" && animIsOver)
                {
                    deleteMe = true;
                }
                else
                {
                    setAnimation("DIE");
                    mode = BehaviorMode.DIE;
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
                    if(position.X > myGame.testPlayer.getPos().X)
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
                    if (position.X > myGame.testPlayer.getPos().X)
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
                    flip = position.X - myGame.testPlayer.getPos().X > 1;
                    if(shotTimer <= 0)
                    {
                        myGame.enemyAttacks.Add(new Projectile(fireball, Color.White, 5, (getCenter() + new Vector2(0, 0)), 5, (float)Math.Atan2(myGame.testPlayer.getCenter().Y - (getCenter() + new Vector2(0, -10)).Y, myGame.testPlayer.getCenter().X - (getCenter() + new Vector2(0, -10)).X), myGame));
                        shotTimer = shotDelay;
                    }
                    break;
            }
            
            base.Update(gameTime);

            position += direction * speed;

            foreach(Attack a in myGame.friendlyAttacks)
            {
                if(collisionRect().Intersects(a.collisionRect()))
                {
                    takeDamage(a.damage);
                    a.deleteMe = true;
                }
            }

            shotTimer -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void addAnimations()
        {
            AnimationSet idle = new AnimationSet("IDLE", new Point(40, 80), new Point(1, 1), new Point(0, 0), 1000, false);
            AnimationSet walk = new AnimationSet("WALK", new Point(40, 80), new Point(6, 1), new Point(0, 1), 100, true);
            AnimationSet jump = new AnimationSet("JUMP", new Point(40, 80), new Point(3, 1), new Point(0, 2), 100, false);
            AnimationSet shoot = new AnimationSet("SHOOT", new Point(60, 80), new Point(2, 1), 150, new Point(240, 0), false);
            sets.Add(idle);
            sets.Add(walk);
            sets.Add(jump);
            sets.Add(shoot);
            setAnimation("IDLE");

            base.addAnimations();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                if (!flip)
                {
                    spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i]);
                }
                else
                {
                    spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i], 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                }
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
    }
}
