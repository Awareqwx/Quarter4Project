using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Attack : AnimatedSprite
    {

        public Boolean deleteMe;
        int damage;
        protected GameManager myGame;

        public Attack(Texture2D[] t, Color[] c, int d, Vector2 p, GameManager g) : base(t)
        {
            colors = c;
            damage = d;
            position = p;
            myGame = g;
            deleteMe = false;
            addAnimations();
        }

        public Attack(Texture2D t, Color c, int d, Vector2 p, GameManager g) : base(t)
        {
            colors = new Color[] { c };
            damage = d;
            position = p;
            myGame = g;
            deleteMe = false;
            addAnimations();
        }

        public override void addAnimations()
        {

            sets.Add(new AnimationSet("IDLE", new Point(textures[0].Width, textures[0].Height), new Point(1, 1), Point.Zero, 1000, false));
            setAnimation("IDLE");
            
            base.addAnimations();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i]);
            }
        }

    }
}
