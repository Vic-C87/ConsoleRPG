using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class PauseGameMenu
    {
        readonly int myWindowHeight;
        readonly int myWindowWidth;

        string[] myMenuChoices = new string[2];
        Vector2[] myMenuPositions = new Vector2[2];

        int myPreviousMenuIndex = 0;
        int myCurrentMenuIndex = 0;

        bool myGameIsPaused;
        bool myContinuePlaying = true;

        public PauseGameMenu()
        {
            myWindowWidth = Console.WindowWidth;
            myWindowHeight = Console.WindowHeight;

            myMenuChoices[0] = "Resume Game";
            myMenuChoices[1] = "Quit to Main Menu";
            

            for (int i = 0; i < myMenuPositions.Length; i++)
            {
                myMenuPositions[i] = new Vector2(myWindowWidth / 2 - myMenuChoices[i].Length / 2, myWindowHeight / 2 + i * 2);
            }
        }

        public void PauseIt(out bool aGameRunningToggle)
        {
            myGameIsPaused = true;
            Console.Clear();
            while (myGameIsPaused)
            {
                DrawMenu();
                SetSelected(myCurrentMenuIndex, myPreviousMenuIndex);
                SelectMenuItem();
            }
            
            aGameRunningToggle = myContinuePlaying;
        }

        void SelectMenuItem()
        {
            ConsoleKeyInfo selection;

            while (Console.KeyAvailable)
                Console.ReadKey(true);

            selection = Console.ReadKey(true);

            if (selection.Key == ConsoleKey.UpArrow)
            {
                myPreviousMenuIndex = myCurrentMenuIndex;
                myCurrentMenuIndex--;
                SetSelected(myCurrentMenuIndex, myPreviousMenuIndex);
            }
            else if (selection.Key == ConsoleKey.DownArrow)
            {
                myPreviousMenuIndex = myCurrentMenuIndex;
                myCurrentMenuIndex++;
                SetSelected(myCurrentMenuIndex, myPreviousMenuIndex);
            }
            else if (selection.Key == ConsoleKey.Enter)
            {
                MenuChoice((EPauseChoices)myCurrentMenuIndex);
            }

        }
        void DrawMenu()
        {
            //Draw Title Screen
            for (int i = 0; i < myMenuChoices.Length; i++)
            {
                DrawMenuOption(i);
            }

        }

        void SetSelected(int aNewIndex, int aPreviousIndex)
        {
            DrawMenuOption(aPreviousIndex);
            if (aNewIndex >= myMenuChoices.Length)
            {
                aNewIndex = 0;
            }
            else if (aNewIndex < 0)
            {
                aNewIndex = myMenuChoices.Length - 1;
            }
            DrawMenuOption(aNewIndex, false);
            myCurrentMenuIndex = aNewIndex;
            ResetColor();
        }

        void DrawMenuOption(int anIndex, bool aReset = true)
        {
            if (aReset)
            {
                ResetColor();
            }
            else
            {
                SetSelectedColor();
            }
            Utilities.Cursor(myMenuPositions[anIndex]);
            Console.Write(myMenuChoices[anIndex]);
        }

        void SetSelectedColor()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        void ResetColor()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        void MenuChoice(EPauseChoices aChoice)
        {
            switch (aChoice)
            {
                case EPauseChoices.Resume:
                    myGameIsPaused = false;
                    break;
                case EPauseChoices.Quit:
                    myGameIsPaused = false;
                    myContinuePlaying = false;
                    SoundManager.StopPlaying();
                    break;
                default:
                    break;
            }
        }

        enum EPauseChoices
        {
            Resume,
            Quit
        }
    }
}
