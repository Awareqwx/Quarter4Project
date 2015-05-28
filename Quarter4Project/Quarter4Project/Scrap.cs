using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Scrap : Entity
    {

        int value;
        Random r;

        public Scrap(Texture2D[] t, Vector2 p, GameManager g, int v)
            : base(t, p, g)
        {
            value = v;
            type = "Scrap";
            speed = 5;
            r = new Random();
            addAnimations();
        }

        public Scrap(Texture2D t, Vector2 p, GameManager g, int v)
            : base(new Texture2D[] { t }, p, g)
        {
            value = v;
            type = "Scrap";
            speed = 5;
            r = new Random();
            addAnimations();
        }

        public override void Update(GameTime gameTime)
        {
            if(!isFalling)
            {
                direction.X = 0;
            }
            if(collisionRect().Intersects(myGame.testPlayer.collisionRect()) && !deleteMe)
            {
                deleteMe = true;
                myGame.testPlayer.addScrap(value);
            }
            base.Update(gameTime);
        }

        public override void addAnimations()
        {
            sets.Add(new AnimationSet("0", new Point(15, 15), new Point(1, 1), new Point(0, 0), 1000, false));
            sets.Add(new AnimationSet("1", new Point(15, 15), new Point(1, 1), new Point(1, 0), 1000, false));
            sets.Add(new AnimationSet("2", new Point(15, 15), new Point(1, 1), new Point(2, 0), 1000, false));
            sets.Add(new AnimationSet("3", new Point(15, 15), new Point(1, 1), new Point(3, 0), 1000, false));
            sets.Add(new AnimationSet("4", new Point(15, 15), new Point(1, 1), new Point(4, 0), 1000, false));
            sets.Add(new AnimationSet("5", new Point(15, 15), new Point(1, 1), new Point(0, 1), 1000, false));
            sets.Add(new AnimationSet("6", new Point(15, 15), new Point(1, 1), new Point(1, 1), 1000, false));
            sets.Add(new AnimationSet("7", new Point(15, 15), new Point(1, 1), new Point(2, 1), 1000, false));
            sets.Add(new AnimationSet("8", new Point(15, 15), new Point(1, 1), new Point(3, 1), 1000, false));
            sets.Add(new AnimationSet("9", new Point(15, 15), new Point(1, 1), new Point(4, 1), 1000, false));
            sets.Add(new AnimationSet("10", new Point(15, 15), new Point(1, 1), new Point(1, 2), 1000, false));
            setAnimation("" + r.Next(11));
            base.addAnimations();
        }

        public override void collide(int i)
        {
            switch (i)
            {
                case 0:
                    position.Y -= position.Y % 40 - 2 - 25;
                    break;
                case 1:
                    position.Y += 39 - ((position.Y - 1) % 40 + 25);
                    break;
                case 2:
                    position.X -= position.X % 40 - 25;
                    break;
                case 3:
                    position.X += 39 - ((position.X - 1) % 40 + 25);
                    break;
            }
        }

    }
}
