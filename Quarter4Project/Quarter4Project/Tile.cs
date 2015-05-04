using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public class Tile : AnimatedSprite
    {

        public enum TileTypes { AIR, PLAYER, WALL, FACADE, WIN};

        Rectangle r;
        int gridSize;
        TileTypes type;
        int[,] map;
        int frame;
        GameManager myGame;


        public Tile(Texture2D t, Vector2 p, GameManager g, TileTypes y, int[,] m)
            : base(t)
        {
            r = new Rectangle((int)p.X, (int)p.Y, t.Width, t.Height);
            type = y;
            map = m;
            position = p;
            myGame = g;

            gridSize = t.Width / 4;
            Point gridPos = new Point((int)position.X / gridSize, (int)position.Y / gridSize);
            if (gridPos.Y > 0)
            {
                if (map[gridPos.Y - 1, gridPos.X] >= (int)type) frame += 20;
            }
            if (gridPos.Y < map.GetLength(0) - 1)
            {
                if (map[gridPos.Y + 1, gridPos.X] >= (int)type) frame += 10;
            }

            if (gridPos.X > 0)
            {
                if (map[gridPos.Y, gridPos.X - 1] >= (int)type) frame += 2;
            }
            if (gridPos.X < map.GetLength(1) - 1)
            {
                if (map[gridPos.Y, gridPos.X + 1] >= (int)type) frame += 1;
            }
            addAnimations();
        }

        public override void addAnimations()
        {
            sets.Add(new AnimationSet("0", new Point(40, 40), new Point(1, 1), new Point(0, 0), 1000, false));
            sets.Add(new AnimationSet("10", new Point(40, 40), new Point(1, 1), new Point(0, 1), 1000, false));
            sets.Add(new AnimationSet("20", new Point(40, 40), new Point(1, 1), new Point(0, 3), 1000, false));
            sets.Add(new AnimationSet("30", new Point(40, 40), new Point(1, 1), new Point(0, 2), 1000, false));
            sets.Add(new AnimationSet("1", new Point(40, 40), new Point(1, 1), new Point(1, 0), 1000, false));
            sets.Add(new AnimationSet("11", new Point(40, 40), new Point(1, 1), new Point(1, 1), 1000, false));
            sets.Add(new AnimationSet("21", new Point(40, 40), new Point(1, 1), new Point(1, 3), 1000, false));
            sets.Add(new AnimationSet("31", new Point(40, 40), new Point(1, 1), new Point(1, 2), 1000, false));
            sets.Add(new AnimationSet("2", new Point(40, 40), new Point(1, 1), new Point(3, 0), 1000, false));
            sets.Add(new AnimationSet("12", new Point(40, 40), new Point(1, 1), new Point(3, 1), 1000, false));
            sets.Add(new AnimationSet("22", new Point(40, 40), new Point(1, 1), new Point(3, 3), 1000, false));
            sets.Add(new AnimationSet("32", new Point(40, 40), new Point(1, 1), new Point(3, 2), 1000, false));
            sets.Add(new AnimationSet("3", new Point(40, 40), new Point(1, 1), new Point(2, 0), 1000, false));
            sets.Add(new AnimationSet("13", new Point(40, 40), new Point(1, 1), new Point(2, 1), 1000, false));
            sets.Add(new AnimationSet("23", new Point(40, 40), new Point(1, 1), new Point(2, 3), 1000, false));
            sets.Add(new AnimationSet("33", new Point(40, 40), new Point(1, 1), new Point(2, 2), 1000, false));
            setAnimation("" + frame);
            base.addAnimations();
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < textures.Length; i++)
            {
                spriteBatch.Draw(textures[i], position - myGame.cameraOffset, new Rectangle(currentSet.frameSize.X * currentFrame.X + currentSet.startPos.X, currentSet.frameSize.Y * currentFrame.Y + currentSet.startPos.Y, currentSet.frameSize.X, currentSet.frameSize.Y), colors[i]);
            }
        }

        public int getFrame()
        {
            return frame;
        }

        public TileTypes getType()
        {
            return type;
        }

        public void setTint(Color c)
        {
            colors[0] = c;
        }
    }
}
