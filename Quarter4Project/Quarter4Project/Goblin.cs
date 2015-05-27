using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Goblin : Enemy
    {

        public Goblin(Texture2D[] t, Vector2 p, float s, GameManager g, int h, int l)
            : base(t, p, s, g, h, l)
        {

        }

        public override void setBehavior()
        {
            if (Collision.getDistance(myGame.testPlayer.getPos(), getPos()) > 500 || myGame.myGame.noClip)
            {
                mode = BehaviorMode.IDLE;
            }
            else if (Collision.getDistance(myGame.testPlayer.getPos(), getPos()) > 25)
            {
                mode = BehaviorMode.ADVANCE;
            }
            else if (Collision.getDistance(myGame.testPlayer.getPos(), getPos()) < 5)
            {
                mode = BehaviorMode.FLEE;
            }
            else
            {
                mode = BehaviorMode.ATTACK;
            }
        }

        public override void addAnimations()
        {
            AnimationSet idle = new AnimationSet("IDLE", new Point(80, 80), new Point(1, 1), new Point(0, 0), 1000, false);
            AnimationSet walk = new AnimationSet("WALK", new Point(80, 80), new Point(4, 1), new Point(0, 1), 100, true);
            AnimationSet shoot = new AnimationSet("SHOOT", new Point(95, 80), new Point(1, 3), 150, new Point(320, 0), true);
            AnimationSet hurt = new AnimationSet("HURT", new Point(80, 80), new Point(1, 1), new Point(0, 2), 150, false);
            AnimationSet die = new AnimationSet("DIE", new Point(95, 80), new Point(1, 4), 300, new Point(415, 0), false);
            sets.Add(idle);
            sets.Add(walk);
            sets.Add(shoot);
            sets.Add(hurt);
            sets.Add(die);
            setAnimation("IDLE");
        }

    }
}
