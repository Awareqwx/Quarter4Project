using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Quarter4Project
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class GameManager : Microsoft.Xna.Framework.DrawableGameComponent
    {

        public enum locs { DEMO };

        public Game1 myGame;

        public Tile[,] currentMap;

        SpriteFont font;

        SpriteBatch spriteBatch;

        Texture2D airTiles;
        Texture2D wallTiles;
        Texture2D facadeTiles;
        Texture2D winTiles;

        Texture2D backgrounds;

        public Vector2 cameraOffset;

        public Cursor cursor;
        Texture2D cursorImage;

        #region Test Variables

        Texture2D testPlayerImage;
        Player testPlayer;
        Tile[,] demo;

        public List<Attack> enemyAttacks, friendlyAttacks;

        #endregion

        public GameManager(Game1 game)
            : base(game)
        {
            myGame = game;
            enemyAttacks = new List<Attack>();
            friendlyAttacks = new List<Attack>();
            Initialize();
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            
            spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            testPlayerImage = myGame.Content.Load<Texture2D>(@"Images\Characters\MaleSprites");
            airTiles = myGame.Content.Load<Texture2D>(@"Images\Test\AirTiles");
            wallTiles = myGame.Content.Load<Texture2D>(@"Images\Tiles\WallTiles");
            facadeTiles = myGame.Content.Load<Texture2D>(@"Images\Tiles\FacadeTiles");
            winTiles = myGame.Content.Load<Texture2D>(@"Images\Test\WinTile");
            font = myGame.Content.Load<SpriteFont>(@"Fonts\Font");
            backgrounds = myGame.Content.Load<Texture2D>(@"Images\Backgrounds\test_tubes");
            cursorImage = myGame.Content.Load<Texture2D>(@"Images\Test\Cursor");
            cursor = new Cursor(cursorImage, this);
            demo = GenerateTiles(Maps.mapDemo);
            currentMap = demo;
            cameraOffset = testPlayer.getPos() - new Vector2(600, 200);
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            cursor.Update(gameTime);
            testPlayer.Update(gameTime);
            foreach(Tile t in currentMap)
            {
                t.Update(gameTime);
            }
            foreach (Attack a in friendlyAttacks)
            {
                a.Update(gameTime);
            }
            foreach (Attack a in enemyAttacks)
            {
                a.Update(gameTime);
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgrounds, new Vector2(Game1.screenSize.X - 2000, Game1.screenSize.Y - 1000), Color.White);
            foreach(Tile t in currentMap)
            {
                t.Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < friendlyAttacks.Count; i++ )
            {
                friendlyAttacks[i].Draw(gameTime, spriteBatch);
                if (friendlyAttacks[i].deleteMe)
                {
                    friendlyAttacks.RemoveAt(i);
                }
            }
            for (int i = 0; i < enemyAttacks.Count; i++)
            {
                enemyAttacks[i].Draw(gameTime, spriteBatch);
                if (enemyAttacks[i].deleteMe)
                {
                    enemyAttacks.RemoveAt(i);
                }
            }
            testPlayer.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(font, testPlayer.getPos().ToString() + "," + testPlayer.getJumps() + "," + testPlayer.getFalling() + "," + testPlayer.getDirection(), new Vector2(10, 10), Color.Black);
            cursor.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }



        public Tile[,] GenerateTiles(int[,] m)
        {
            Tile[,] map = new Tile[m.GetLength(1), m.GetLength(0)];

            for (int i = 0; i < m.GetLength(1); i++ )
            {
                for(int j = 0; j < m.GetLength(0); j++)
                {
                    switch (m[j, i])
                    {
                        default:
                        case 0:
                            map[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            break;
                        case 2:
                            map[i, j] = new Tile(wallTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.WALL, m);
                            break;
                        case 1:
                            testPlayer = new Player(new Texture2D[] { testPlayerImage }, new Vector2(40 * i, 40 * j), 5, this);
                            map[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            break;
                        case 3:
                            map[i, j] = new Tile(facadeTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.FACADE, m);
                            break;
                        case 4:
                            map[i, j] = new Tile(winTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.WIN, m);
                            break;
                    }
                }
            }

                return map;
        }

        public void incrementOffset(Vector2 v)
        {
            cameraOffset += v;
        }

    }
}