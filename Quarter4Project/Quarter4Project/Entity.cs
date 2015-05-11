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

        #region Constructors

        public Entity(Texture2D[] t, Vector2 p, GameManager g) : base(t)
        {
            deleteMe = false;
            position = p;
            myGame = g;
        }

        public Entity(Texture2D t, Vector2 p) : base(t)
        {
            deleteMe = false;
            position = p;
        }

        #endregion

        #region Methods

        public override void Update(GameTime gameTime)
        {

            points = new Vector2[] { position, new Vector2(position.X + getFrameSize().X, position.Y), new Vector2(position.X, position.Y + getFrameSize().Y), new Vector2(position.X + getFrameSize().X, position.Y + getFrameSize().Y) };

            isFalling = true;
            if ((!myGame.myGame.noClip && type == "Player") || type != "Player")
            {
                for (int i = 0; i < myGame.currentMap.tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < myGame.currentMap.tiles.GetLength(1); j++)
                    {
                        if (myGame.currentMap.tiles[i, j].getType() == Tile.TileTypes.WALL)
                        {
                            if (collidesWithTile(myGame.currentMap.tiles[i, j]) == 1)
                            {
                                if (direction.Y > 0)
                                {
                                    direction.Y = 0;
                                    jumps = 0;
                                }
                                isFalling = false;
                                position.Y -= position.Y % 40 - 2;
                            }
                            else
                            {

                                if (collidesWithTile(myGame.currentMap.tiles[i, j]) == 2)
                                {
                                    if (direction.Y < 0)
                                    {
                                        direction.Y = 0;
                                    }
                                    position.Y += 39 - ((position.Y - 1) % 40);
                                }
                                else if (collidesWithTile(myGame.currentMap.tiles[i, j]) == 3)
                                {
                                    if (direction.X > 0)
                                    {
                                        direction.X = 0;
                                    }
                                    position.X -= position.X % 40;
                                }
                                else if (collidesWithTile(myGame.currentMap.tiles[i, j]) == 4)
                                {
                                    if (direction.X < 0)
                                    {
                                        direction.X = 0;
                                    }
                                    position.X += 39 - ((position.X - 1) % 40);
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
                    direction.Y += 0.06f;
                }
            }
            if(isFalling && currentSet.name != "WON")
            {
                setAnimation("JUMP");
            }

            base.Update(gameTime);
        }

        public virtual void ifWon()
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i]);
            }
        }

        public Boolean shouldDelete()
        {
            return deleteMe;
        }

        public int collidesWithTile(Tile t)
        {
            if(t.getType() == Tile.TileTypes.AIR)
            {
                return 0;
            }
            Vector2 v = new Vector2(t.getPos().X - position.X, t.getPos().Y - position.Y);
            double d = Math.Sqrt(Math.Pow(t.getFrameSize().X + getFrameSize().X, 2) + Math.Pow(t.getFrameSize().Y + getFrameSize().Y, 2));

            if(Collision.magnitude(v) < d)
            {
                Collision.mapSegment[] segs = new Collision.mapSegment[] { new Collision.mapSegment(new Point((int)position.X + 2, (int)position.Y + getFrameSize().Y), new Point((int)position.X - 2 + getFrameSize().X, (int)position.Y + getFrameSize().Y)), new Collision.mapSegment(new Point((int)position.X - 2 + getFrameSize().X, (int)position.Y), new Point((int)position.X + 2, (int)position.Y)), new Collision.mapSegment(new Point((int)position.X + getFrameSize().X, (int)position.Y + getFrameSize().Y - 5), new Point((int)position.X + getFrameSize().X, (int)position.Y + 5)), new Collision.mapSegment(new Point((int)position.X, (int)position.Y + 5), new Point((int)position.X, (int)position.Y - 5 + getFrameSize().Y)) };
                for(int i = 0; i < segs.Length; i++)
                {
                    if(Collision.CheckSegmentRectangleCollision(segs[i], t.collisionRect()))
                    {
                        return i + 1;
                    }
                }
            }
            return 0;
        }

        public Vector2 getCenter()
        {
            return new Vector2(position.X + getFrameSize().X / 2, position.Y + getFrameSize().Y / 2);
        }

        #endregion

    }
}
