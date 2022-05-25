using System;
using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class FarmScene
    {
        const char myTorch = '¡';
        readonly int mySpeed;
        int myTextIndex;
        readonly char[,] myScene;
        readonly char[,] myKneelingMan;
        readonly char[,] myFootPrintsA;
        readonly char[,] myFootPrintsB;
        readonly char[,] myFootPrintsC;
        readonly char[,] myVerticalFrame;
        readonly char[,] myBottomFrame;
        readonly char[,] myTopFrame;
        readonly GameObject myPlayer;
        Vector2 myOffSet;
        Vector2 myStartPosition;
        Vector2 myTextPosition;
        Vector2 myEnterToContinuePosition;
        readonly Action myContinueAction;
        readonly List<string> myTextList = new List<string>();
        readonly List<string> myPrologue;
        readonly List<string> myEndPrologue;

        public FarmScene()
        {
            myScene = Utilities.ReadFromFile(@"Sprites/CutScene/FarmHouse.txt", out _);
            mySpeed = 1;
            char[,] playerSprite = Utilities.ReadFromFile(@"Sprites/Man.txt", out string title);
            myKneelingMan = Utilities.ReadFromFile(@"Sprites/KneelingMan.txt", out _);
            myFootPrintsA = Utilities.ReadFromFile(@"Sprites/FootPrintsA.txt", out _);
            myFootPrintsB = Utilities.ReadFromFile(@"Sprites/FootPrintsB.txt", out _);
            myFootPrintsC = Utilities.ReadFromFile(@"Sprites/FootPrintsC.txt", out _);
            myVerticalFrame = Utilities.ReadFromFile(@"Sprites/VerticalFrame.txt", out _);
            myBottomFrame = Utilities.ReadFromFile(@"Sprites/BottomFrame.txt", out _);
            myTopFrame = Utilities.ReadFromFile(@"Sprites/TopFrame.txt", out _);
            myPlayer = new GameObject(new Vector2(playerSprite.GetLength(0), playerSprite.GetLength(1)), title, playerSprite);
            myOffSet = new Vector2(Console.WindowWidth / 2 - myScene.GetLength(0) / 2, Console.WindowHeight / 2 - myScene.GetLength(1) / 2);
            myContinueAction = () => EnterToContinue();
            myPrologue = Utilities.GetPrologue(@"Dialogues/Prologue.txt");
            myEndPrologue = Utilities.GetPrologue(@"Dialogues/EndPrologue.txt");
            myTextList.Add("Mother? Father? Where are you?.... Nooo, Father!");
            myTextList.Add("Father! What have they done to you? *Crying*");
            myTextList.Add("Noo not you as well mother! *Crying* I swear I will AVENGE YOU!");
            myTextList.Add("You can not hide from me! I will find you and make you pay if it's the last thing I do!");
        }

        public void DrawScene()
        {
            Prologue(myPrologue);

            //Initiate player position and set startposition 
            myPlayer.MyPosition  = myStartPosition = new Vector2(myOffSet.X - myPlayer.MyWidth, myOffSet.Y + 24);
            myTextPosition = new Vector2(myOffSet.X + myScene.GetLength(0) / 5 * 2, myOffSet.Y - 4);
            myEnterToContinuePosition = new Vector2(myTextPosition.X + myScene.GetLength(0) / 4, myTextPosition.Y + 2);
            
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
            myTextIndex = 0;
            PrintText(myTextIndex);
            Utilities.ActionByInput(myContinueAction, ConsoleKey.Enter);

            //Move player to first body
            myPlayer.MoveToByX(new Vector2(myStartPosition.X + 50, myStartPosition.Y - 2), mySpeed);
            
            //Change sprite to kneeling sprite
            myPlayer.ClearSprite();
            Utilities.DrawSprite(myKneelingMan, myPlayer.MyPosition.Up());

            //Text!!!
            myTextIndex++;
            PrintText(myTextIndex);
            Utilities.ActionByInput(myContinueAction, ConsoleKey.Enter);

            //Change sprite to standing sprite
            Utilities.ClearSprite(myKneelingMan, myPlayer.MyPosition.Up());
            Utilities.DrawSprite(myPlayer.MySprite, myPlayer.MyPosition);
            
            //Move player to second body
            myPlayer.MoveToByY(new Vector2(myStartPosition.X + 96, myStartPosition.Y - 1), mySpeed + 1);

            //Change sprite to kneeling sprite
            myPlayer.ClearSprite();
            Utilities.DrawSprite(myKneelingMan, myPlayer.MyPosition.Up());
            
            //Text!!!
            myTextIndex++;
            PrintText(myTextIndex);
            Utilities.ActionByInput(myContinueAction, ConsoleKey.Enter);

            //Change sprite to standing sprite
            Utilities.ClearSprite(myKneelingMan, myPlayer.MyPosition.Up());
            Utilities.DrawSprite(myPlayer.MySprite, myPlayer.MyPosition);
            
            //Move player offscreen
            myPlayer.MoveToByY(new Vector2((myStartPosition.X + 147), (myStartPosition.Y - 1)), mySpeed);

            //Text!!!
            myTextIndex++;
            PrintText(myTextIndex);
            Utilities.ActionByInput(myContinueAction, ConsoleKey.Enter);

            Console.Clear();
            Prologue(myEndPrologue);
        }

        void Prologue(List<string> someText)
        {
            Vector2 leftFramePosition = new Vector2(Console.WindowWidth/2 - myTopFrame.GetLength(0)/2 - 2, Console.WindowHeight/2 - myVerticalFrame.GetLength(1)/2);
            Vector2 rightFramePosition = leftFramePosition.Right(89 + myVerticalFrame.GetLength(0));
            Vector2 bottomFramePosition = leftFramePosition.Down(myVerticalFrame.GetLength(1) - 1);
            Vector2 topFramePosition = leftFramePosition.Up(3);
            Vector2 textPosition = new Vector2(leftFramePosition.X + 6, Console.WindowHeight / 2 - someText.Count / 2 - 1);

            Utilities.DrawSprite(myVerticalFrame, leftFramePosition, ConsoleColor.DarkYellow);
            Utilities.DrawSprite(myVerticalFrame, rightFramePosition, ConsoleColor.DarkYellow);
            Utilities.DrawSprite(myBottomFrame, bottomFramePosition, ConsoleColor.DarkYellow);
            Utilities.DrawSprite(myTopFrame, topFramePosition, ConsoleColor.DarkYellow);

            for (int i = 0; i < someText.Count; i++)
            {
                Utilities.Cursor(textPosition);
                Utilities.Typewriter(someText[i], 25);
                textPosition = textPosition.Down();
            }
            textPosition = textPosition.Right(10);
            Utilities.Cursor(textPosition);
            Console.Write("Continue...(Press Enter)");
            Utilities.ActionByInput(() => Console.Clear(), ConsoleKey.Enter);
            Console.Clear();
        }

        void PrintText(int aTextIndex)
        {
            Utilities.Cursor(myTextPosition);
            Utilities.Typewriter(myTextList[aTextIndex], 100);
            Utilities.Cursor(myEnterToContinuePosition);
            Console.Write("Continue...(Press Enter)");
        }

        void EnterToContinue()
        {
            Utilities.Cursor(myTextPosition);
            for (int i = 0; i < myTextList[myTextIndex].Length; i++)
            {
                Console.Write(" ");
            }
            Utilities.Cursor(myEnterToContinuePosition);
            Console.Write("                        ");
        }
    }
}
