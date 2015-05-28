using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Boss : Enemy
    {

        public Boss(Texture2D[] t, Vector2 p, GameManager g) : base(t, p, 0, g, 100, 1)
        {

        }

    }
}
