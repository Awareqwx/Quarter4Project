using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Player : Entity
    {
        KeyboardState keyboard, keyboardPrev;
        MouseState mouse, mousePrev;

        public int hp, hpMax;
        int scrap;

        int upgradeLevel;
        public int nextUpgrade;

        Texture2D fireball;
        Boolean flip;
        private bool won;

        public Player(Texture2D[] t, Vector2 p, float s, GameManager g)
            : base(t, p, g)
        {
            keyboard = keyboardPrev = Keyboard.GetState();
            mouse = mousePrev = Mouse.GetState();
            direction = Vector2.Zero;
            speed = s;
            isFalling = true;
            jumps = 0;
            maxJumps = 2;
            hp = hpMax = 100;
            type = "Player";
            addAnimations();

            upgradeLevel = 1;
            nextUpgrade = 5;

            fireball = myGame.myGame.Content.Load<Texture2D>(@"Images\Test\Fireball");

        }

        public override void Update(GameTime gameTime)
        {

            if(scrap >= nextUpgrade)
            {
                upgrade();
            }

            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();
            if (hp > 0)
            {
                if (!won)
                {
                    if (keyboard.IsKeyDown(Keys.D))
                    {
                        direction.X = 1;
                        flip = false;
                        if (!isFalling)
                        {
                            setAnimation("WALK");
                        }
                    }
                    else if (keyboard.IsKeyDown(Keys.A))
                    {
                        direction.X = -1;
                        flip = true;
                        if (!isFalling)
                        {
                            setAnimation("WALK");
                        }
                    }
                    else
                    {
                        direction.X = 0;
                        if ((currentSet.name != "SHOOT" || animIsOver) && !isFalling)
                        {
                            setAnimation("IDLE");
                        }
                    }
                }
                if (!myGame.myGame.noClip)
                {
                    if (won)
                    {
                        setAnimation("WON");
                    }
                    else if ((keyboard.IsKeyDown(Keys.W) && keyboardPrev.IsKeyUp(Keys.W)) && (!isFalling || jumps < maxJumps || myGame.myGame.noClip))
                    {
                        direction.Y = -1.8f;
                        isFalling = true;
                        jumps++;
                        resetAnimation();
                    }
                }
                else
                {
                    if (keyboard.IsKeyDown(Keys.W))
                    {
                        direction.Y = -1;
                    }
                    else if (keyboard.IsKeyDown(Keys.S))
                    {
                        direction.Y = 1;
                    }
                    else
                    {
                        direction.Y = 0;
                    }
                }
                if (myGame.myGame.noClip && colors[0].A != 128)
                {
                    colors[0].A = 128;
                }
                if (!myGame.myGame.noClip && colors[0].A != 255)
                {
                    colors[0].A = 255;
                }

                base.Update(gameTime);

                if (Math.Abs(direction.Y) > 5)
                {
                    direction.Y = 5 * Math.Sign(direction.Y);
                }

                if (keyboard.IsKeyDown(Keys.K))
                {
                    position = new Vector2(1120, 2080);
                    direction = Vector2.Zero;
                    won = false;
                    myGame.cameraOffset = position - new Vector2(600, 300);
                }

                if (position.X + getFrameSize().X - myGame.cameraOffset.X > Game1.screenSize.X - 400 && direction.X > 0)
                {
                    myGame.incrementOffset(new Vector2(direction.X * speed, 0));
                }
                else if (position.X - myGame.cameraOffset.X < 400 && direction.X < 0)
                {
                    myGame.incrementOffset(new Vector2(direction.X * speed, 0));
                }
                if (position.Y + getFrameSize().Y - myGame.cameraOffset.Y > Game1.screenSize.Y - 200 && direction.Y > 0)
                {
                    myGame.incrementOffset(new Vector2(0, direction.Y * speed));
                }
                else if (position.Y - myGame.cameraOffset.Y < 200 && direction.Y < 0)
                {
                    myGame.incrementOffset(new Vector2(0, direction.Y * speed));
                }

                if (mouse.LeftButton == ButtonState.Pressed && mousePrev.LeftButton != ButtonState.Pressed)
                {
                    Vector2 v = myGame.cursor.getPos();
                    flip = v.X < position.X;
                    myGame.friendlyAttacks.Add(new Projectile(fireball, Color.White, 2 + upgradeLevel, (getCenter() + new Vector2(0, -20)), 5, (float)Math.Atan2(myGame.cursor.getPos().Y - (getCenter() + new Vector2(0, -10)).Y, myGame.cursor.getPos().X - (getCenter() + new Vector2(0, -10)).X), myGame));
                    setAnimation("SHOOT");
                }
                if (isFalling && currentSet.name != "WON")
                {
                    setAnimation("JUMP", new Point(40, 80));
                }
            }
            else
            {
                if (currentSet.name == "DIE" && animIsOver)
                {
                    myGame.gameOver = true;
                }
                else
                {
                    setAnimation("DIE");
                    direction.X = 0;
                }
                base.Update(gameTime);
            }

            if(keyboard.IsKeyDown(Keys.H) && keyboardPrev.IsKeyUp(Keys.H))
            {
                takeDamage(4 + upgradeLevel);
            }
            if (keyboard.IsKeyDown(Keys.J) && keyboardPrev.IsKeyUp(Keys.J))
            {
                heal(5);
            }

            foreach(Attack a in myGame.enemyAttacks)
            {
                if(collisionRect().Intersects(a.collisionRect()))
                {
                    takeDamage(a.damage);
                    a.deleteMe = true;
                }
            }

            keyboardPrev = keyboard;
            mousePrev = mouse;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length && i < upgradeLevel; i++)
            {
                if (!flip)
                {
                    spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(size.X * currentFrame.X + currentSet.startPos.X, size.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i]);
                }
                else
                {
                    spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(size.X * currentFrame.X + currentSet.startPos.X, size.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i], 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                }
            }
        }

        public override void addAnimations()
        {
            AnimationSet idle = new AnimationSet("IDLE", new Point(40, 80), new Point(1, 1), new Point(0, 0), 1000, false);
            AnimationSet walk = new AnimationSet("WALK", new Point(40, 80), new Point(6, 1), new Point(0, 1), 100, true);
            AnimationSet jump = new AnimationSet("JUMP", new Point(40, 91), new Point(2, 1), 100, new Point(0, 161), false);
            AnimationSet shoot = new AnimationSet("SHOOT", new Point(60, 80), new Point(2, 1), 150, new Point(240, 0), false);
            AnimationSet win = new AnimationSet("WON", new Point(40, 80), new Point(1, 1), 1000, new Point(80, 161), false);
            AnimationSet die = new AnimationSet("DIE", new Point(60, 80), new Point(2, 2), 1000, new Point(240, 81), false);
            sets.Add(idle);
            sets.Add(walk);
            sets.Add(jump);
            sets.Add(shoot);
            sets.Add(win);
            sets.Add(die);
            setAnimation("IDLE");

            base.addAnimations();
        }

        public override void ifWon()
        {
                won = true;
        }

        public int getJumps()
        {
            return jumps;
        }

        public Boolean getFalling()
        {
            return isFalling;
        }

        public Vector2 getDirection()
        {
            return direction;
        }

        public void takeDamage(int d)
        {
            d -= upgradeLevel - 1;
            if(d < 1)
            {
                d = 1;
            }
            hp -= d;
            if(hp < 0)
            {
                hp = 0;
            }
        }

        public void heal(int h)
        {
            hp += h;
            if(hp > hpMax)
            {
                hp = hpMax;
            }
        }

        public void setPos(Vector2 v)
        {
            position = v;
        }

        public void addScrap(int i)
        {
            scrap += i;
        }

        public int getScrap()
        {
            return scrap;
        }

        public int getLevel()
        {
            return upgradeLevel;
        }

        public void upgrade()
        {
            upgradeLevel++;
            scrap -= nextUpgrade;
            nextUpgrade += 2;
        }

    }
}
