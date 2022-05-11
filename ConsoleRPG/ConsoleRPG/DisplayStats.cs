using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class DisplayStats
    {
        bool myActive = false;
        Vector2 myDisplayOffSet;
        public DisplayStats()
        {
            myDisplayOffSet = new Vector2(Console.WindowWidth/4, Console.WindowHeight/2 - 5);
        }

        public void ShowStats(Player aPlayer)
        {
            myActive = true;
            Console.Clear();
            Utilities.CursorPosition(myDisplayOffSet.X, myDisplayOffSet.Y);
            Console.Write($"Health:\t{aPlayer.myCurrentHP}/{aPlayer.myStat.myHP}");
            Utilities.CursorPosition(myDisplayOffSet.X, myDisplayOffSet.Y + 1);
            Console.Write($"Mana:\t{aPlayer.myCurrentMP}/{aPlayer.myStat.myMP}");
            Utilities.CursorPosition(myDisplayOffSet.X, myDisplayOffSet.Y + 2);
            Console.Write($"Damage:\t{aPlayer.myStat.myDamage}");
            Utilities.CursorPosition(myDisplayOffSet.X, myDisplayOffSet.Y + 3);
            Console.Write($"Armor:\t{aPlayer.myStat.myArmor}");

            ConsoleKeyInfo input;
            while (Console.KeyAvailable)
                Console.ReadKey(false);

            input = Console.ReadKey(true);
            while (myActive)
            {

                if (input.Key == ConsoleKey.Tab || input.Key == ConsoleKey.Escape)
                {
                    myActive = false;
                }

            }
            
            Console.Clear();
        }
    }
}
