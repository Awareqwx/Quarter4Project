using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    class HealthPack : Entity
    {

        int level;

        public HealthPack(Texture2D[] t, Vector2 p, GameManager g, int l) : base(t, p, g)
        {
            level = l;
        }

        public override void addAnimations()
        {
            //sets.Add(new AnimationSet("IDLE", ));
        }

    }
}
