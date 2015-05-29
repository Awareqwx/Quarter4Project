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

        public struct Map
        {

            public Tile[,] tiles;
            public List<Enemy> enemyList;
            public List<Scrap> scrapList;
            public Vector2 playerLoc;
            public Texture2D background;

            public Map(Tile[,] t, List<Enemy> e, List<Scrap> s, Vector2 v, Texture2D b)
            {
                tiles = t;
                enemyList = e;
                scrapList = s;
                playerLoc = v;
                background = b;
            }

        };

        public enum locs { DEMO, LEVEL1, LEVEL2, LEVEL3, BOSS };

        public Game1 myGame;

        public Map currentMap;

        public List<EnemySpawner> spawnerList;

        public SoundEffectInstance Walk;
        public SoundEffect Fire;
        public SoundEffectInstance Death;
        //  public SoundEffect MMAtk;
        public SoundEffect EnemyDeath;
        //  public SoundEffect Explosion;
        public SoundEffect Jump;
        public SoundEffectInstance FinalBoss;
        public SoundEffectInstance MainLvl;
        public SoundEffectInstance nowPlaying;
        public SoundEffectInstance LVLEND;
        public SoundEffect LVLSTART;
        public List<SoundEffectInstance> effects;

        int level;

        SpriteFont font;

        SpriteBatch spriteBatch;

        Texture2D airTiles;
        Texture2D wallTiles;
        Texture2D facadeTiles;
        Texture2D winTiles;

        Texture2D testTubeBG;
        Texture2D monsterPitBG;
        Texture2D tubeBossBG;
        Texture2D bossBG;

        public Vector2 cameraOffset;

        public GameCursor cursor;

        public Boolean gameOver;

        Texture2D cursorImage;
        Texture2D[] spiderTextures;
        Texture2D[] goblinTextures;
        Texture2D[] bossTextures;

        public List<Attack> enemyAttacks, friendlyAttacks;

        KeyboardState keyboard, keyboardPrev;

        #region Test Variables

        Texture2D[] testPlayerImage;
        public Player player;

        Texture2D[] scrapImage;

        #endregion

        Map Level1;
        Map Level2;
        Map Level3;
        Map bossMap;

        public Boolean won;
        int wonTime;

        public GameManager(Game1 game)
            : base(game)
        {
            myGame = game;
            enemyAttacks = new List<Attack>();
            friendlyAttacks = new List<Attack>();
            won = false;
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
            keyboard = keyboardPrev = Keyboard.GetState();
            spriteBatch = new SpriteBatch(myGame.GraphicsDevice);
            level = 1;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Sound Files (added by Michael)
            Walk = myGame.Content.Load<SoundEffect>(@"Audio\FootSteps").CreateInstance();
            Fire = myGame.Content.Load<SoundEffect>(@"Audio\Laser");
            Jump = myGame.Content.Load<SoundEffect>(@"Audio\Jump");
            LVLEND = myGame.Content.Load<SoundEffect>(@"Audio\LvlEnd").CreateInstance();
            LVLSTART = myGame.Content.Load<SoundEffect>(@"Audio\LvlStart");
            EnemyDeath = myGame.Content.Load<SoundEffect>(@"Audio\RMonDeath");
            //  Explosion = myGame.Content.Load<SoundEffect>(@"Audio\Explosion"); melee enemy death || rocket launcher
            MainLvl = myGame.Content.Load<SoundEffect>(@"Audio\Music\GameMusic").CreateInstance();
            MainLvl.IsLooped = true;
            FinalBoss = myGame.Content.Load<SoundEffect>(@"Audio\Music\BossMusic").CreateInstance();
            FinalBoss.IsLooped = true;
            Death = myGame.Content.Load<SoundEffect>(@"Audio\Death").CreateInstance();

            effects = new List<SoundEffectInstance>();

            testPlayerImage = new Texture2D[] { myGame.Content.Load<Texture2D>(@"Images\Characters\PlayerBase"), myGame.Content.Load<Texture2D>(@"Images\Characters\PlayerChest"), myGame.Content.Load<Texture2D>(@"Images\Characters\PlayerLegs"), myGame.Content.Load<Texture2D>(@"Images\Characters\PlayerLShoulder"), myGame.Content.Load<Texture2D>(@"Images\Characters\PlayerRShoulder"), myGame.Content.Load<Texture2D>(@"Images\Characters\PlayerGloves") };
            airTiles = myGame.Content.Load<Texture2D>(@"Images\Test\AirTiles");
            wallTiles = myGame.Content.Load<Texture2D>(@"Images\Tiles\WallTiles");
            facadeTiles = myGame.Content.Load<Texture2D>(@"Images\Tiles\FacadeTiles");
            winTiles = myGame.Content.Load<Texture2D>(@"Images\Test\WinTile");
            font = myGame.Content.Load<SpriteFont>(@"Fonts\Font");

            testTubeBG = myGame.Content.Load<Texture2D>(@"Images\Backgrounds\test_tubes");
            bossBG = myGame.Content.Load<Texture2D>(@"Images\Backgrounds\boss_bg");
            monsterPitBG = myGame.Content.Load<Texture2D>(@"Images\Backgrounds\monster_pit");
            tubeBossBG = myGame.Content.Load<Texture2D>(@"Images\Backgrounds\test_tube_singleedit");

            cursorImage = myGame.Content.Load<Texture2D>(@"Images\cursor");
            spiderTextures = new Texture2D[] { myGame.Content.Load<Texture2D>(@"Images\Enemies\SpiderBase"), myGame.Content.Load<Texture2D>(@"Images\Enemies\SpiderLights") };
            goblinTextures = new Texture2D[] { myGame.Content.Load<Texture2D>(@"Images\Enemies\GoblinBase"), myGame.Content.Load<Texture2D>(@"Images\Enemies\GoblinLights") };
            bossTextures = new Texture2D[] { myGame.Content.Load<Texture2D>(@"Images\Enemies\BossSprites"), myGame.Content.Load<Texture2D>(@"Images\Enemies\InvisSprites") };
            scrapImage = new Texture2D[] { myGame.Content.Load<Texture2D>(@"Images\Scrap\ScrapBase"), myGame.Content.Load<Texture2D>(@"Images\Scrap\ScrapLights") };
            cursor = new GameCursor(cursorImage, this);
            player = new Player(testPlayerImage, Vector2.Zero, 5, this);
            Level1 = GenerateTiles(Maps.mapLevel1, tubeBossBG);
            Level2 = GenerateTiles(Maps.mapLevel2, testTubeBG);
            Level3 = GenerateTiles(Maps.mapLevel3, monsterPitBG);
            bossMap = GenerateTiles(Maps.mapBossLevel, bossBG);
            switchMap(Level1);
            cameraOffset = player.getPos() - new Vector2(600, 200);
            prepGame();
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (!Enabled)
            {
                return;
            }

            if(gameOver)
            {
                myGame.SetCurrentLevel(Game1.GameLevels.LOSE);
            }

            if(won)
            {
                wonTime += gameTime.ElapsedGameTime.Milliseconds;
                if(wonTime >= 4000)
                {
                    myGame.SetCurrentLevel(Game1.GameLevels.WIN);
                }
            }

            // TODO: Add your update code here
            keyboard = Keyboard.GetState();
            cursor.Update(gameTime);
            player.Update(gameTime);
            foreach (Tile t in currentMap.tiles)
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
            for (int i = 0; i < currentMap.enemyList.Count; i++)
            {
                currentMap.enemyList[i].Update(gameTime);
            }
            foreach (Scrap s in currentMap.scrapList)
            {
                s.Update(gameTime);
            }

            if (keyboard.IsKeyDown(Keys.F10) && keyboardPrev.IsKeyUp(Keys.F10))
            {
                myGame.showDebug = !myGame.showDebug;
            }
            if (keyboard.IsKeyDown(Keys.F11) && keyboardPrev.IsKeyUp(Keys.F11))
            {
                myGame.noClip = !myGame.noClip;
            }
            if (keyboard.IsKeyDown(Keys.F9) && keyboardPrev.IsKeyUp(Keys.F9))
            {
                myGame.SetCurrentLevel(Game1.GameLevels.MENU);
            }

            if (keyboard.IsKeyDown(Keys.D1) && keyboardPrev.IsKeyUp(Keys.D1))
            {
                switchMap(Level1);
            }
            if (keyboard.IsKeyDown(Keys.D2) && keyboardPrev.IsKeyUp(Keys.D2))
            {
                switchMap(Level2);
            }
            if (keyboard.IsKeyDown(Keys.D3) && keyboardPrev.IsKeyUp(Keys.D3))
            {
                switchMap(Level3);
            }
            if (keyboard.IsKeyDown(Keys.D4) && keyboardPrev.IsKeyUp(Keys.D4))
            {
                switchMap(bossMap);
            }
            if (keyboard.IsKeyDown(Keys.N) && keyboardPrev.IsKeyUp(Keys.N))
            {
                currentMap.scrapList.Add(new Scrap(scrapImage, cursor.getPos(), this, 1));
            }
            if (keyboard.IsKeyDown(Keys.M) && keyboardPrev.IsKeyUp(Keys.M))
            {
                currentMap.enemyList.Add(new Enemy(spiderTextures, cursor.getPos(), 2, this, 10, player.getLevel()));
            }
            for (int i = 0; i < spawnerList.Count; i++)
            {
                spawnerList[i].Update();
                if (spawnerList[i].shouldDelete)
                {
                    spawnerList.RemoveAt(i);
                }
            }

            keyboardPrev = keyboard;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!Visible)
            {
                return;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(currentMap.background, new Vector2(Game1.screenSize.X - 2000, Game1.screenSize.Y - 1000), Color.White);
            foreach (Tile t in currentMap.tiles)
            {
                t.Draw(gameTime, spriteBatch);
            }
            for (int i = 0; i < friendlyAttacks.Count; i++)
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
            for (int i = 0; i < currentMap.enemyList.Count; i++)
            {
                currentMap.enemyList[i].Draw(gameTime, spriteBatch);
                if (currentMap.enemyList[i].deleteMe)
                {
                    currentMap.enemyList.RemoveAt(i);
                }
            }
            for (int i = 0; i < currentMap.scrapList.Count; i++)
            {
                currentMap.scrapList[i].Draw(gameTime, spriteBatch);
                if (currentMap.scrapList[i].deleteMe)
                {
                    currentMap.scrapList.RemoveAt(i);
                }
            }
            player.Draw(gameTime, spriteBatch);

            if (myGame.showDebug)
            {
                spriteBatch.DrawString(font, player.getPos().ToString() + "," + player.hp + "," + player.getFalling() + "," + player.getDirection(), new Vector2(10, 10), Color.Black);
            }
            cursor.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            keyboardPrev = keyboard;

            base.Draw(gameTime);
        }



        public GameManager.Map GenerateTiles(int[,] m, Texture2D b)
        {
            Tile[,] tiles = new Tile[m.GetLength(1), m.GetLength(0)];
            List<Enemy> enemies = new List<Enemy>();
            List<Scrap> scrap = new List<Scrap>();
            Vector2 playerLoc = Vector2.Zero;

            for (int i = 0; i < m.GetLength(1); i++)
            {
                for (int j = 0; j < m.GetLength(0); j++)
                {
                    switch (m[j, i])
                    {
                        default:
                        case 0:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            break;
                        case 2:
                            tiles[i, j] = new Tile(wallTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.WALL, m);
                            break;
                        case 1:
                            playerLoc = new Vector2(40 * i, 40 * j);
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            break;
                        case 3:
                            tiles[i, j] = new Tile(facadeTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.FACADE, m);
                            break;
                        case 4:
                            tiles[i, j] = new Tile(winTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.WIN, m);
                            break;
                        case 5:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Enemy(spiderTextures, new Vector2(40 * i, 40 * j), 2, this, 10, 1));
                            break;
                        case 6:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Goblin(goblinTextures, new Vector2(40 * i, 40 * j), 3, this, 15, 1));
                            break;
                        case 7:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Enemy(spiderTextures, new Vector2(40 * i, 40 * j), 2, this, 10, 2));
                            break;
                        case 8:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Goblin(goblinTextures, new Vector2(40 * i, 40 * j), 3, this, 15, 2));
                            break;
                        case 9:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Enemy(spiderTextures, new Vector2(40 * i, 40 * j), 2, this, 10, 3));
                            break;
                        case 10:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Goblin(goblinTextures, new Vector2(40 * i, 40 * j), 3, this, 15, 3));
                            break;
                        case 11:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Enemy(spiderTextures, new Vector2(40 * i, 40 * j), 2, this, 10, 4));
                            break;
                        case 12:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Goblin(goblinTextures, new Vector2(40 * i, 40 * j), 3, this, 15, 4));
                            break;
                        case 13:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Enemy(spiderTextures, new Vector2(40 * i, 40 * j), 2, this, 10, 5));
                            break;
                        case 14:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Goblin(goblinTextures, new Vector2(40 * i, 40 * j), 3, this, 15, 5));
                            break;
                        case 15:
                            tiles[i, j] = new Tile(airTiles, new Vector2(40 * i, 40 * j), this, Tile.TileTypes.AIR, m);
                            enemies.Add(new Boss(bossTextures, new Vector2(40 * i, 40 * j), this, 500));
                            break;
                    }
                }
            }

            return new Map(tiles, enemies, scrap, playerLoc, b);

        }

        public void incrementOffset(Vector2 v)
        {
            cameraOffset += v;
        }

        public List<EnemySpawner> createSpawners(List<Enemy> e)
        {
            List<EnemySpawner> spawners = new List<EnemySpawner>();

            for (int i = 0; i < e.Count; i++)
            {
                spawners.Add(new EnemySpawner(e[i], e[i].getPos(), this));
            }

            return spawners;
        }

        public void switchMap(Map m)
        {
            spawnerList = createSpawners(m.enemyList);
            m.enemyList = new List<Enemy>();
            player.setPos(m.playerLoc);
            cameraOffset = player.getPos() - new Vector2(600, 300);
            currentMap = m;
            effects.Add(LVLSTART.CreateInstance());
            effects[effects.Count - 1].Play();
        }

        public Texture2D[] getScrapImage(int i)
        {
            return scrapImage;
        }

        public void stopSound()
        {
            for (int i = 0; i < effects.Count; i++)
            {
                effects[i].Stop();
            }
            Walk.Stop();
            Death.Stop();
            LVLEND.Stop();
            nowPlaying.Stop();
        }

        public void startSound()
        {
            nowPlaying.Play();
        }

        public void prepGame()
        {
            nowPlaying = MainLvl;
        }

        public void prepBoss()
        {
            nowPlaying = FinalBoss;
        }

        public void advanceLevel()
        {
            player.hp = player.hpMax;
            level++;
            switch(level)
            {
                case 1:
                    switchMap(Level1);
                    break;
                case 2:
                    switchMap(Level2);
                    break;
                case 3:
                    switchMap(Level3);
                    break;
                case 4:
                    switchMap(bossMap);
                    break;
            }
        }

        public void resetMaps()
        {
            level = 1;
            Level1 = GenerateTiles(Maps.mapLevel1, tubeBossBG);
            Level2 = GenerateTiles(Maps.mapLevel2, testTubeBG);
            Level3 = GenerateTiles(Maps.mapLevel3, monsterPitBG);
            bossMap = GenerateTiles(Maps.mapBossLevel, bossBG);
            wonTime = 0;
            won = false;
            switchMap(Level1);
            player.setLevel(1);
        }

    }
}
