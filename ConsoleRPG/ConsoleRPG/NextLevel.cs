using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class NextLevel
    {
        int mySpeed;
        char[,] myMansionSprite;
        char[,] myStairsSprite;
        GameObject myCharacter;

        Vector2 myMansionPosition;
        Vector2 myStairsPosition;
        
        Vector2 myPlayerStartPositionMansion;
        Vector2 myPlayerStartPositionStairs;

        Vector2 myPlayerEndPositionMansion;
        Vector2 myPlayerEndPositionStairs;
        
        public NextLevel()
        {
            mySpeed = 1;
            myMansionSprite = Utilities.ReadFromFile(@"Sprites/Mansion.txt", out _);
            myStairsSprite = Utilities.ReadFromFile(@"Sprites/ClimbStairs.txt", out _);

            char[,] playerSprite = Utilities.ReadFromFile(@"Sprites/Man.txt", out _);

            myCharacter = new GameObject(new Vector2(playerSprite.GetLength(0), playerSprite.GetLength(1)), "Player", playerSprite);

            myMansionPosition = new Vector2(Console.WindowWidth / 2, Console.WindowHeight / 3);

            myPlayerStartPositionMansion = new Vector2(Console.WindowWidth / 3, Console.WindowHeight / 2 + Console.WindowHeight / 5);
            myPlayerEndPositionMansion = new Vector2(myMansionPosition.X + 15, myMansionPosition.Y + 14);

            myStairsPosition = new Vector2(Console.WindowWidth/2 - myStairsSprite.GetLength(0) / 2, Console.WindowHeight / 2 - myStairsSprite.GetLength(1) / 2);
            myPlayerStartPositionStairs = new Vector2(myStairsPosition.X + 9, myStairsPosition.Y + 15);
            myPlayerEndPositionStairs = new Vector2(myStairsPosition.X + 59, myStairsPosition.Y + 15);
        }

        public void EnterMansion()
        {
            Utilities.DrawSprite(myMansionSprite, myMansionPosition);

            myCharacter.DrawSprite(myPlayerStartPositionMansion);
            myCharacter.MoveToByX(myPlayerEndPositionMansion, mySpeed);
            Utilities.PressEnterToContinue("Press \'Enter\' to enter the Mansion...");
            Console.Clear();
            WalkStairs();
        }

        public void WalkStairs(int aLevelToEnter = 1)
        {
            Utilities.CursorPosition(Console.WindowWidth / 2, 5);
            Console.Write("Entering level " + aLevelToEnter);
            Utilities.DrawSprite(myStairsSprite, myStairsPosition);
            myCharacter.DrawSprite(myPlayerStartPositionStairs);
            myCharacter.MoveToByX(myPlayerEndPositionStairs, mySpeed);

            for (int i = 0; i < 8; i++)
            {
                myCharacter.MoveToByY(new Vector2(myCharacter.MyPosition.X + 4, myCharacter.MyPosition.Y - 1), mySpeed);
            }
            myCharacter.MoveToByY(new Vector2(myCharacter.MyPosition.X + 10, myCharacter.MyPosition.Y - 1), mySpeed);
        }
    }
}
