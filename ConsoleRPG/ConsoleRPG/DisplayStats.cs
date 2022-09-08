using System;
using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class DisplayStats
    {
        Vector2 myStatsOffSet;
        Vector2 myKeysOffSet;
        public DisplayStats()
        {
            myStatsOffSet = new Vector2(Console.WindowWidth / 4, Console.WindowHeight / 2 - 5);
            myKeysOffSet = new Vector2(Console.WindowWidth / 2, Console.WindowHeight / 2 - 5);
        }

        public void ShowStats(Player aPlayer)
        {
            Console.Clear();
            Utilities.CursorPosition(myStatsOffSet.X, 0);
            Console.Write("Move around with the arrow keys");
            if (!aPlayer.myArcadeMode)
            {
                Utilities.CursorPosition(myStatsOffSet.X, 1);
                Console.Write("Place portal back to Village with F1");
            }
            else
            {
                Utilities.CursorPosition(myStatsOffSet.X, 1);
                Console.Write("Using \'Defend\' in battle heals you a small amount");
            }
            Utilities.CursorPosition(myStatsOffSet.X, 2);
            Console.Write("Open Stats menu with TAB");
            Utilities.CursorPosition(myStatsOffSet.X, myStatsOffSet.Y);
            Console.Write($"Health:\t{aPlayer.myCurrentHP}/{aPlayer.myStat.myHP}");
            Utilities.CursorPosition(myStatsOffSet.X, myStatsOffSet.Y + 1);
            Console.Write($"Mana:\t{aPlayer.myCurrentMP}/{aPlayer.myStat.myMP}");
            Utilities.CursorPosition(myStatsOffSet.X, myStatsOffSet.Y + 2);
            Console.Write($"Damage:\t{aPlayer.myStat.myDamage}");
            Utilities.CursorPosition(myStatsOffSet.X, myStatsOffSet.Y + 3);
            Console.Write($"Armor:\t{aPlayer.myStat.myArmor}");
            if (!aPlayer.myArcadeMode)
            {
                Utilities.CursorPosition(myStatsOffSet.X, myStatsOffSet.Y + 7);
                Console.Write($"Death count:\t{aPlayer.myDeathCounter}");
            }
            else
            {
                Utilities.CursorPosition(myStatsOffSet.X, myStatsOffSet.Y + 7);
                Console.Write($"Current Room:\t{aPlayer.myCurrentRoom}");
                Utilities.CursorPosition(myStatsOffSet.X, myStatsOffSet.Y + 8);
                Console.Write($"Current Level:\t{aPlayer.myCurrentLevel}");
            }

            if (aPlayer.myKeyIDs.Count > 0)
            {
                Utilities.Cursor(myKeysOffSet.Up());
                Utilities.Color("Keys picked up:", ConsoleColor.DarkYellow);
                int count = 0;
                foreach (KeyValuePair<int, string> key in aPlayer.myKeyIDs)
                {
                    count++;
                    Utilities.CursorPosition(myKeysOffSet.X, myKeysOffSet.Y + count);
                    Utilities.Color(key.Value, ConsoleColor.DarkYellow);
                }
            }
            Utilities.ActionByInput(() => CloseStats(), ConsoleKey.Tab);
        }

        static void CloseStats()
        {
            Console.Clear();
        }
    }
}
