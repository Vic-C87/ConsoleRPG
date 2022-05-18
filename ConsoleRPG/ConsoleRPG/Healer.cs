using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Healer
    {
        readonly char[,] mySprite;
        readonly Vector2 myOffSet;
        readonly Vector2 myTextPosition;
        readonly string myHealText;

        public Healer()
        {
            mySprite = Utilities.ReadFromFile(@"Sprites\Rooms\Healer.txt", out _);
            myOffSet = new Vector2(Console.WindowWidth/2 - mySprite.GetLength(0)/2, Console.WindowHeight/2 - mySprite.GetLength(1)/2);
            myHealText = "You have regained your HP and MP.";
            myTextPosition = new Vector2(Console.WindowWidth / 2 - myHealText.Length / 2, myOffSet.Y - 2);
        }

        public void EnterHealer()
        {
            Console.Clear();
            Utilities.DrawSprite(mySprite, myOffSet);
            Utilities.Cursor(myTextPosition);
            Utilities.Color(myHealText, ConsoleColor.Green);
            Utilities.ActionByInput(() => Console.Clear(), ConsoleKey.Enter);

        }
    }
}
