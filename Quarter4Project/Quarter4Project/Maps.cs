using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project
{
    public static class Maps
    {

        static ContentManager content;

        public static int[,] mapDemo;
        public static int[,] mapDemoInverse;

        public static void init(Game1 g)
        {
            content = g.Content;
            content.RootDirectory = "Content";
            mapDemo = generateMap("Test");
            mapDemoInverse = generateMap("TestInverse");
        }

        static int[,] generateMap(String s)
        {
            Texture2D ParseImage = content.Load<Texture2D>(@"Images\Maps\" + s);

            int[,] ints = new int[ParseImage.Height - 1, ParseImage.Width];


            Color[] rawData = new Color[ParseImage.Width * ParseImage.Height];
            ParseImage.GetData<Color>(rawData);
            // Note that this stores the pixel's row in the first index, and the pixel's column in the second,
            // with this setup.
            Color[,] colorGrid = new Color[ParseImage.Height, ParseImage.Width];
            for (int row = 0; row < ParseImage.Height; row++)
            {
                for (int column = 0; column < ParseImage.Width; column++)
                {
                    // Assumes row major ordering of the array.
                    colorGrid[row, column] = rawData[row * ParseImage.Width + column];
                }
            }
            List<Color> colors = new List<Color>();
            Boolean end = false;
            for (int i = 0; i < colorGrid.GetLength(1) && !end; i++)
            {
                if (colorGrid[0, i].A != 255)
                {
                    end = true;
                }
                else
                {
                    colors.Add(colorGrid[0, i]);
                }
            }
            for (int i = 1; i < colorGrid.GetLength(0); i++)
            {
                for (int j = 0; j < colorGrid.GetLength(1); j++)
                {
                    for (int k = 0; k < colors.Count; k++)
                    {
                        if (colorGrid[i, j] == colors[k])
                        {
                            ints[i - 1, j] = k;
                        }
                    }
                }
            }

            return ints;
        }
    }
}
