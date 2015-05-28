using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Entity : AnimatedSprite
    {

        public Boolean deleteMe;
        Vector2[] points;
        protected GameManager myGame;

        protected Vector2 direction;
        protected float speed;
        protected Boolean isFalling;
        protected int jumps, maxJumps;
        protected string type;

        protected Point size;

        #region Constructors

        public Entity(Texture2D[] t, Vector2 p, GameManager g) : base(t)
        {
            deleteMe = false;
            position = p;
            myGame = g;
            size = Point.Zero;
        }

        public Entity(Texture2D t, Vector2 p) : base(t)
        {
            deleteMe = false;
            position = p;
            size = Point.Zero;
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {

            points = new Vector2[] { position, new Vector2(position.X + size.X, position.Y), new Vector2(position.X, position.Y + size.Y), new Vector2(position.X + size.X, position.Y + size.Y) };

            isFalling = true;
            if ((!myGame.myGame.noClip && type == "Player") || type != "Player")
            {
                for (int i = 0; i < myGame.currentMap.tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < myGame.currentMap.tiles.GetLength(1); j++)
                    {
                        if (myGame.currentMap.tiles[i, j].getType() == Tile.TileTypes.WALL)
                        {
                            int t = collidesWithTile(myGame.currentMap.tiles[i, j]);
                            if (t == 1)
                            {
                                if (direction.Y > 0)
                                {
                                    direction.Y = 0;
                                    jumps = 0;
                                }
                                isFalling = false;
                                collide(0);
                            }
                            else
                            {

                                if (t == 2)
                                {
                                    if (direction.Y < 0)
                                    {
                                        direction.Y = 0;
                                    }
                                    collide(1);
                                }
                                else if (t == 3)
                                {
                                    if (direction.X > 0)
                                    {
                                        direction.X = 0;
                                    }
                                    collide(2);
                                }
                                else if (t == 4)
                                {
                                    if (direction.X < 0)
                                    {
                                        direction.X = 0;
                                    }
                                    collide(3);
                                }
                            }
                        }
                        else if (myGame.currentMap.tiles[i, j].getType() == Tile.TileTypes.WIN)
                        {
                            if (collidesWithTile(myGame.currentMap.tiles[i, j]) != 0)
                            {
                                ifWon();
                            }
                        }
                    }
                }
                if (isFalling)
                {
                    direction.Y += (float)(0.3 / speed);
                    if(jumps == 0)
                    {
                        jumps = 1;
                    }
                }
            }

            position = new Vector2(position.X + direction.X * speed, position.Y + direction.Y * speed);

            base.Update(gameTime);
        }

        public virtual void ifWon()
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(size.X * currentFrame.X + currentSet.startPos.X, size.Y * currentFrame.Y + currentSet.startPos.Y, size.X, size.Y), colors[i]);
            }
        }

        public Boolean shouldDelete()
        {
            return deleteMe;
        }

        public int collidesWithTile(Tile t)
        {
            if (t.getType() == Tile.TileTypes.AIR)
            {
                return 0;
            }
            if (Collision.getDistanceSquared(position, t.getPos()) > 500 * 500)
            {
                return 0;
            }

            Collision.mapSegment[] segs = new Collision.mapSegment[] { new Collision.mapSegment(new Point((int)position.X + 2, (int)position.Y + size.Y), new Point((int)position.X - 2 + size.X, (int)position.Y + size.Y)), new Collision.mapSegment(new Point((int)position.X - 2 + size.X, (int)position.Y), new Point((int)position.X + 2, (int)position.Y)), new Collision.mapSegment(new Point((int)position.X + size.X, (int)position.Y + size.Y - 5), new Point((int)position.X + size.X, (int)position.Y + 5)), new Collision.mapSegment(new Point((int)position.X, (int)position.Y + 5), new Point((int)position.X, (int)position.Y - 5 + size.Y)) };
            for (int i = 0; i < segs.Length; i++)
            {
                if (Collision.CheckSegmentRectangleCollision(segs[i], t.collisionRect()))
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public override void setAnimation(string setName)
        {
            base.setAnimation(setName);
            size = currentSet.frameSize;
        }

        public void setAnimation(string setName, Point s)
        {
            setAnimation(setName);
            size = s;
        }

        public Vector2 getCenter()
        {
            return new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
        }

        public virtual void collide(int i)
        {
            switch(i)
            {
                case 0:
                    position.Y -= position.Y % 40 - 2;
                    break;
                case 1:
                    position.Y += 39 - ((position.Y - 1) % 40);
                    break;
                case 2:
                    position.X -= position.X % 40;
                    break;
                case 3:
                    position.X += 39 - ((position.X - 1) % 40);
                    break;
            }
        }

        #endregion

    }
}
