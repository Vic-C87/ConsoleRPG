using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Chest
    {
        public List<Item> myItems;
        public int myKeyID;
        char[,] mySprite;
        public bool myIsOpened;
        Vector2 myOffSet;

        public Chest()
        {
            myItems = new List<Item>();
            myKeyID = 0;
            mySprite = Utilities.ReadFromFile(@"Sprites\Chest.txt", out string aTitle);
            myIsOpened = false;
            myOffSet = new Vector2();
        }

        public void AddKey(int aKeyID)
        {
            myKeyID = aKeyID;
        }

        public void DrawChest(Vector2 anOffSet)
        {
            myOffSet = anOffSet;
            for (int y = 0; y < mySprite.GetLength(1); y++)
            {
                for (int x = 0; x < mySprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + myOffSet.X, y + myOffSet.Y, mySprite[x, y]);
                }
            }
        }

        public List<Item> OpenChest()
        {
            myIsOpened = true;
            for (int y = 0; y < mySprite.GetLength(1); y++)
            {
                for (int x = 0; x < mySprite.GetLength(0); x++)
                {
                    if(y == 1 && (x > 0 && x < mySprite.GetLength(0)-1))
                    {
                        Utilities.Draw(x + myOffSet.X, y + myOffSet.Y, ' ');
                    }
                }
            }

            return myItems;
        }

    }
}
