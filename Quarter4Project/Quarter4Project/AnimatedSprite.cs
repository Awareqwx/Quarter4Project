using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

//A basic sprite class that supports multiple animation sets, looping/non-looping animations, and multiple image layers with their own color washes.

namespace Quarter4Project
{
    public class AnimatedSprite
    {

        #region Variables

        public struct AnimationSet
        {
            public string name; //Should be all caps (for example: IDLE, WALK, JUMP)
            public Point frameSize; //The size of each individual frame (px, px)
            public Point sheetSize; //The number of frames in either direction 
            public Point startPos; //The starting point of the animation, the top left frame is (0, 0)
            public int millisPerFrame; //The time each frame should be up, in milliseconds (1000ms = 1s)
            public Boolean doesLoop; //Does the animation loop?
            public AnimationSet(string n, Point fs, Point ss, Point sp, int mpf, Boolean l) //Uses frameSize to determine sprite position
            {
                name = n;
                frameSize = fs;
                sheetSize = ss;
                startPos = new Point(sp.X * fs.X, fs.Y * sp.Y);
                millisPerFrame = mpf;
                doesLoop = l;
            }

            public AnimationSet(string n, Point fs, Point ss, int mpf, Point sp, Boolean l) //Uses absolute position to determine sprite position
            {
                name = n;
                frameSize = fs;
                sheetSize = ss;
                startPos = sp;
                millisPerFrame = mpf;
                doesLoop = l;
            }
        };

        protected List<AnimationSet> sets;
        protected AnimationSet currentSet;

        public Texture2D[] textures;

        protected Vector2 position, prevPosition, prevPrevPosition;
        protected Color[] colors;

        protected Point currentFrame;

        protected Boolean animIsOver;

        public Collision.Circle collisionCircle;

        protected int timeSinceLast;

        #endregion

        #region Constructors

        public AnimatedSprite(Texture2D[] t)
        {
            textures = t;
            currentFrame = Point.Zero;
            sets = new List<AnimationSet>();
            animIsOver = false;
            colors = new Color[t.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.White;
            }
            collisionCircle.R = (currentSet.frameSize.X + currentSet.frameSize.Y) / 4;
            collisionCircle.P.X = (float) (position.X + collisionCircle.R);
            collisionCircle.P.Y = (float) (position.Y + collisionCircle.R);
        }
        public AnimatedSprite(Texture2D t)
        {
            textures = new Texture2D[]{t};
            currentFrame = Point.Zero;
            sets = new List<AnimationSet>();
            animIsOver = false;
            colors = new Color[textures.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.White;
            }
        }

        #endregion

        #region Methods

        public virtual void Update(GameTime gameTime)
        {
            timeSinceLast += gameTime.ElapsedGameTime.Milliseconds;
            if (currentSet.sheetSize == new Point(1, 1))
            {

            }
            else
            {
                if (!animIsOver)
                {
                    if (timeSinceLast >= currentSet.millisPerFrame)
                    {
                        timeSinceLast = 0;
                        currentFrame.X++;
                        if (currentFrame.X >= currentSet.sheetSize.X)
                        {
                            currentFrame.X = 0;
                            currentFrame.Y++;
                            if (currentFrame.Y >= currentSet.sheetSize.Y)
                            {
                                if (currentSet.doesLoop)
                                {
                                    currentFrame.Y = 0;
                                }
                                else
                                {
                                    animIsOver = true;
                                    currentFrame = new Point(currentSet.sheetSize.X - 1, currentSet.sheetSize.Y - 1);
                                }
                            }
                        }
                    }
                }
            }
            prevPrevPosition = prevPosition;
            prevPosition = position;

            collisionCircle.R = (currentSet.frameSize.X + currentSet.frameSize.Y) / 4;
            collisionCircle.P.X = (float)(position.X + collisionCircle.R);
            collisionCircle.P.Y = (float)(position.Y + collisionCircle.R);
        }

        public virtual void addAnimations() { }

        public virtual void setAnimation(string setName)
        {
            if (currentSet.name != setName)
            {
                Boolean loopBreak = false;
                for (int i = 0; i < sets.Count && !loopBreak; i++)
                {
                    if (sets[i].name == setName)
                    {
                        currentSet = sets[i];
                        currentFrame = Point.Zero;
                        loopBreak = true;
                        animIsOver = false;
                    }
                }
            }
        }

        public Vector2 getPos()
        {
            return position;
        }

        public Point getFrameSize()
        {
            return currentSet.frameSize;
        }

        public Rectangle collisionRect()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)currentSet.frameSize.X, (int)currentSet.frameSize.Y);
        }

        public void resetAnimation()
        {
            currentFrame = Point.Zero;
            animIsOver = false;
        }

        #endregion

    }
}