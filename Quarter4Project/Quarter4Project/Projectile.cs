using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Projectile : Attack
    {

        float speed;
        float rotation;

        public Projectile(Texture2D t, Color c, int d, Vector2 p, float s, float r, GameManager g)
            : base(t, c, d, p, g)
        {
            initialize(s, r);
        }

        public Projectile(Texture2D[] t, Color[] c, int d, Vector2 p, float s, float r, GameManager g)
            : base(t, c, d, p, g)
        {
            initialize(s, r);
        }

        private void initialize(float s, float r)
        {
            speed = s;
            rotation = r;
        }

        public override void Update(GameTime gameTime)
        {
            position.X += (float)Math.Cos(rotation) * speed;
            position.Y += (float)Math.Sin(rotation) * speed;
            
            foreach(Tile t in myGame.currentMap)
            {
                if (t.getType() == Tile.TileTypes.WALL)
                {
                    if (t.collisionRect().Intersects(collisionRect()))
                    {
                        deleteMe = true;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i], rotation, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

    }
}
