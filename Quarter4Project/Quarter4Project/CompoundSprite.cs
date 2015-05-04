using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//A basic sprite class that supports multiple image layers with their own color washes, but doesn't support animations.

namespace Quarter4Project
{
    public class CompoundSprite
    {

        protected Texture2D[] textures;
        protected Vector2 position;
        protected Point frameSize;
        protected Color[] colors;

        protected KeyboardState keyboardState;

        public float speed;

        #region Constructors

        public CompoundSprite(Texture2D[] t, Vector2 p, Point fs, Color[] c)
        {
            textures = t;
            position = p;
            frameSize = fs;
            colors = c;
        }
        
        public CompoundSprite(Texture2D[] t, Vector2 p, Point fs)
        {
            keyboardState = new KeyboardState();
            textures = t;
            position = p;
            frameSize = fs;
            colors = new Color[] { Color.White };
        }

        public CompoundSprite(Texture2D[] t, Point fs)
        {
            keyboardState = new KeyboardState();
            textures = t;
            frameSize = fs;
            position = Vector2.Zero;
            colors = new Color[] { Color.White };
        }

        public CompoundSprite(Texture2D[] t)
        {
            keyboardState = new KeyboardState();
            textures = t;
            frameSize = Point.Zero;
            position = Vector2.Zero;
            colors = new Color[] { Color.White };
        }

        #endregion

        public virtual void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                spriteBatch.Draw(textures[i], position, new Rectangle(0, 0, frameSize.X, frameSize.Y), colors[i]);
            }
        }

        public Rectangle collisionRect()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)frameSize.X, (int)frameSize.Y);
        }

        public Vector2 getPos()
        {
            return position;
        }

        public Point getFrameSize()
        {
            return frameSize;
        }
    }
}