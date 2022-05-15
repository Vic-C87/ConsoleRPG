using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class FarmScene
    {
        const char myTorch = '¡';
        string myTitle;
        int mySpeed;
        char[,] myScene;
        char[,] myKneelingMan;
        char[,] myFootPrintsA;
        char[,] myFootPrintsB;
        char[,] myFootPrintsC;
        GameObject myPlayer;
        Vector2 myOffSet;
        Vector2 myStartPosition;
        Action myAction;


        public FarmScene()
        {
            myScene = Utilities.ReadFromFile(@"Sprites\CutScene\FarmHouse.txt", out myTitle);
            string title;
            mySpeed = 1;
            char[,] playerSprite = Utilities.ReadFromFile(@"Sprites\Man.txt", out title);
            myKneelingMan = Utilities.ReadFromFile(@"Sprites\KneelingMan.txt", out _);
            myFootPrintsA = Utilities.ReadFromFile(@"Sprites\FootPrintsA.txt", out _);
            myFootPrintsB = Utilities.ReadFromFile(@"Sprites\FootPrintsB.txt", out _);
            myFootPrintsC = Utilities.ReadFromFile(@"Sprites\FootPrintsC.txt", out _);
            myPlayer = new GameObject(new Vector2(playerSprite.GetLength(0), playerSprite.GetLength(1)), title, playerSprite);
            myAction = () => PrintText();
        }

        public void DrawScene()
        {
            SoundManager.PlaySound(SoundType.VillageAmbience, true);//EDIT!!!

            //Initiate player position and set offset and startposition
            myOffSet = new Vector2(Console.WindowWidth/2 - myScene.GetLength(0)/2, Console.WindowHeight/2 - myScene.GetLength(1)/2);
            myPlayer.MyPosition  = myStartPosition = new Vector2(myOffSet.X - myPlayer.MyWidth, myOffSet.Y + 24);
            
            //Draw Scene Sprite
            Utilities.DrawSprite(myScene, myOffSet);
            
            //Blood pool next to first body
            Utilities.CursorPosition(myOffSet.X + 55, myOffSet.Y + 21);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            
            //Bloody footprints from first body to the second
            Utilities.DrawSprite(myFootPrintsA, myOffSet.X + 56, myOffSet.Y + 21, ConsoleColor.DarkRed);
            Utilities.DrawSprite(myFootPrintsB, myOffSet.X + 59, myOffSet.Y + 22, ConsoleColor.DarkRed);
            
            //Blood pool next to second body
            Utilities.CursorPosition(myOffSet.X + 99, myOffSet.Y + 22);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.Write(" ");
            Console.BackgroundColor = ConsoleColor.Black;
            
            //Bloody footprints from second body leeding offscreen
            Utilities.DrawSprite(myFootPrintsC, myOffSet.X + 100, myOffSet.Y + 22, ConsoleColor.DarkRed);
            
            //Burning candles on each side of farmhouse door
            Utilities.CursorPosition(myOffSet.X + 59, myOffSet.Y + 14);
            Utilities.Color(myTorch.ToString(), ConsoleColor.DarkYellow);
            Utilities.CursorPosition(myOffSet.X + 66, myOffSet.Y + 16);
            Utilities.Color(myTorch.ToString(), ConsoleColor.DarkYellow);
            
            //Draw player at start position
            Utilities.DrawSprite(myPlayer.MySprite, myStartPosition);
            
            //Text!!!
            Utilities.Validate(Utilities.ActionByInput(myAction, ConsoleKey.Enter));
            
            //Move player to first body
            myPlayer.MoveToByX(new Vector2(myStartPosition.X + 50, myStartPosition.Y - 2), mySpeed);
            
            //Change sprite to kneeling sprite
            myPlayer.ClearSprite();
            Utilities.DrawSprite(myKneelingMan, myPlayer.MyPosition.Up());
            
            //Text!!!
            Utilities.Validate(Utilities.ActionByInput(myAction, ConsoleKey.Enter));
            
            //Change sprite to standing sprite
            Utilities.ClearSprite(myKneelingMan, myPlayer.MyPosition.Up());
            Utilities.DrawSprite(myPlayer.MySprite, myPlayer.MyPosition);
            
            //Move player to second body
            myPlayer.MoveToByY(new Vector2(myStartPosition.X + 96, myStartPosition.Y - 1), mySpeed + 1);

            //Change sprite to kneeling sprite
            myPlayer.ClearSprite();
            Utilities.DrawSprite(myKneelingMan, myPlayer.MyPosition.Up());
            
            //Text!!!
            Utilities.Validate(Utilities.ActionByInput(myAction, ConsoleKey.Enter));

            //Change sprite to standing sprite
            Utilities.ClearSprite(myKneelingMan, myPlayer.MyPosition.Up());
            Utilities.DrawSprite(myPlayer.MySprite, myPlayer.MyPosition);
            
            //Move player offscreen
            myPlayer.MoveToByY(new Vector2((myStartPosition.X + 147), (myStartPosition.Y - 1)), mySpeed);
            
            //Text!!!
            Utilities.Validate(Utilities.ActionByInput(myAction, ConsoleKey.Enter));
            
        }

        void PrintText()
        {
            //Console.WriteLine("Yooo");
        }
    }
}
