using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class EnemySpawner
    {

        Enemy enemy;
        Vector2 pos;
        public Boolean shouldDelete;
        GameManager myGame;

        public EnemySpawner(Enemy e, Vector2 p, GameManager g)
        {
            enemy = e;
            pos = p;
            myGame = g;
            shouldDelete = false;
        }

        public void Update()
        {
            if (1600 * 1600 > Collision.getDistanceSquared(pos, myGame.player.getPos()))
            {
                if (!shouldDelete)
                {
                    myGame.currentMap.enemyList.Add(enemy);
                    shouldDelete = true;
                }
            }
        }

    }
}
