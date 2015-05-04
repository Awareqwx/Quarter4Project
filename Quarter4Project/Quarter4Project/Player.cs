using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    class Player : Entity
    {
        KeyboardState keyboard, keyboardPrev;
        MouseState mouse, mousePrev;

        Vector2 direction;
        float speed;
        Boolean isFalling;
        int jumps, maxJumps;

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
            maxJumps = 20;
            addAnimations();

            fireball = myGame.myGame.Content.Load<Texture2D>(@"Images\Test\Fireball");

        }

        public override void Update(GameTime gameTime)
        {
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();

            if (!won)
            {
                if (keyboard.IsKeyDown(Keys.D))
                {
                    direction.X = 1;
                    flip = false;
                    //if (!isFalling)
                    {
                        setAnimation("WALK");
                    }
                }
                else if (keyboard.IsKeyDown(Keys.A))
                {
                    direction.X = -1;
                    flip = true;
                    //if (!isFalling)
                    {
                        setAnimation("WALK");
                    }
                }
                else
                {
                    direction.X = 0;
                    if (currentSet.name != "SHOOT" || animIsOver)
                    {
                        setAnimation("IDLE");
                    }
                }
            }

            if(won)
            {
                setAnimation("HURT");
            }
            else if ((keyboard.IsKeyDown(Keys.W) && keyboardPrev.IsKeyUp(Keys.W)) && (!isFalling || jumps < maxJumps))
            {
                direction.Y = -1.8f;
                isFalling = true;
                jumps++;
            }

            if (isFalling)
            {
                direction.Y += 0.06f;
            }
            Color c = Color.White;
            for (int i = 0; i < myGame.currentMap.GetLength(0); i++)
            {
                for (int j = 0; j < myGame.currentMap.GetLength(1); j++)
                {
                    if (myGame.currentMap[i,j].getType() == Tile.TileTypes.WALL)
                    {
                        if (collidesWithTile(myGame.currentMap[i, j]) == 1)
                        {
                            if (direction.Y > 0)
                            {
                                direction.Y = 0;
                                isFalling = false;
                                jumps = 0;
                            }
                            position.Y -= position.Y % 40;
                            c = Color.Green;
                        }
                        else
                        {
                            isFalling = true;
                            //setAnimation("JUMP");

                            if (collidesWithTile(myGame.currentMap[i, j]) == 2)
                            {
                                if (direction.Y < 0)
                                {
                                    direction.Y = 0;
                                }
                                position.Y += 39 - ((position.Y - 1) % 40);
                                c = Color.Red;
                            }
                            else if (collidesWithTile(myGame.currentMap[i, j]) == 3)
                            {
                                if (direction.X > 0)
                                {
                                    direction.X = 0;
                                }
                                position.X -= position.X % 40;
                                c = Color.Blue;
                            }
                            else if (collidesWithTile(myGame.currentMap[i, j]) == 4)
                            {
                                if (direction.X < 0)
                                {
                                    direction.X = 0;
                                }
                                position.X += 39 - ((position.X - 1) % 40);
                                c = Color.Orange;
                            }
                        }
                        myGame.currentMap[i, j].setTint(c);
                        c = Color.White;
                    }
                    else if (myGame.currentMap[i, j].getType() == Tile.TileTypes.WIN)
                    {
                        if (collidesWithTile(myGame.currentMap[i, j]) != 0)
                        {
                            won = true;
                        }
                    }
                }
            }

            if (Math.Abs(direction.Y) > 5)
            {
                direction.Y = 5 * Math.Sign(direction.Y);
            }

            position = new Vector2(position.X + direction.X * speed, position.Y + direction.Y * speed);

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

            if(mouse.LeftButton == ButtonState.Pressed && mousePrev.LeftButton != ButtonState.Pressed)
            {
                Vector2 v = myGame.cursor.getPos();
                myGame.friendlyAttacks.Add(new Projectile(fireball, Color.White, 5, (getCenter() + new Vector2(0, -20)), 5, (float)Math.Atan2(myGame.cursor.getPos().Y - (getCenter() + new Vector2(0, -10)).Y, myGame.cursor.getPos().X - (getCenter() + new Vector2(0, -10)).X), myGame));
                setAnimation("SHOOT");
            }

            keyboardPrev = keyboard;
            mousePrev = mouse;

            base.Update(gameTime);
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

        public override void addAnimations()
        {
            AnimationSet idle = new AnimationSet("IDLE", new Point(40, 80), new Point(1, 1), new Point(0, 0), 1000, false);
            AnimationSet walk = new AnimationSet("WALK", new Point(40, 80), new Point(6, 1), new Point(0, 1), 100, true);
            AnimationSet jump = new AnimationSet("JUMP", new Point(40, 80), new Point(3, 1), new Point(0, 2), 100, false);
            AnimationSet shoot = new AnimationSet("SHOOT", new Point(60, 80), new Point(2, 1), 150, new Point(240, 0), false);
            AnimationSet hurt = new AnimationSet("HURT", new Point(40, 80), new Point(1, 1), 1000, new Point(0, 240), false);
            sets.Add(idle);
            sets.Add(walk);
            sets.Add(jump);
            sets.Add(shoot);
            sets.Add(hurt);
            setAnimation("IDLE");

            base.addAnimations();
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

    }
}
