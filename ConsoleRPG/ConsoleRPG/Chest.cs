using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Chest
    {
        public UpgradeType myItem;
        public int myKeyID;
        readonly char[,] mySprite;
        public bool myIsOpened;
        Vector2 myOffSet;

        public Chest()
        {
            myItem = UpgradeType.Null;
            myKeyID = 0;
            mySprite = Utilities.ReadFromFile(@"Sprites\Chest.txt", out _);
            myIsOpened = false;
            myOffSet = new Vector2();
        }

        public void AddKey(int aKeyID)
        {
            myKeyID = aKeyID;
        }

        public void AddItem(UpgradeType aType)
        {
            myItem = aType;
        }

        public void DrawChest(Vector2 anOffSet)
        {
            myOffSet = anOffSet;
            Utilities.DrawSprite(mySprite, anOffSet, ConsoleColor.Yellow);
        }

        public UpgradeType OpenChest()
        {
            myIsOpened = true;
            SoundManager.PlaySound(SoundType.OpenChest, false, true);
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
            SoundManager.PlaySound(SoundType.GetKey, false, true);
            
            SoundManager.PlaySound(SoundType.MansionAmbience, true);

            return myItem;
        }

        int GetGold()
        {
            return 0;
        }

    }
}
