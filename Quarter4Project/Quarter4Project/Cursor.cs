using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Cursor : AnimatedSprite
    {

        MouseState mouse;

        GameManager myGame;

        public Cursor(Texture2D t, GameManager g) : base(t)
        {
            myGame = g;
            addAnimations();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            mouse = Mouse.GetState();
            position = new Vector2((float)(mouse.X * 1.05), (float)(mouse.Y)) + myGame.cameraOffset;
            base.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i]);
            }
        }

        public override void addAnimations()
        {

            sets.Add(new AnimationSet("IDLE", new Point(10, 10), new Point(1, 1), new Point(0, 0), 1000, false));
            setAnimation("IDLE");
            base.addAnimations();
        }
    }
}
