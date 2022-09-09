using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class MainMenu
    {
        readonly int myWindowHeight;
        readonly int myWindowWidth;

        bool myGameIsRunning = true;

        string[] myMenuChoices = new string[4];
        Vector2[] myMenuPositions = new Vector2[4];

        int myPreviousMenuIndex = 0;
        int myCurrentMenuIndex = 0;

        HighScore myHighScore;

        public MainMenu()
        {
            myWindowWidth = Console.WindowWidth;
            myWindowHeight = Console.WindowHeight;

            Load();
            DrawMenu();
            SetSelected(myCurrentMenuIndex, myPreviousMenuIndex);
            while (myGameIsRunning)
            {
                Console.Clear();
                DrawMenu();
                SetSelected(myCurrentMenuIndex, myPreviousMenuIndex);
                SelectMenuItem();
            }
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
                MenuChoice((EMainMenuChoices)myCurrentMenuIndex);
            }
        }

        void Load()
        {
            myMenuChoices[0] = "Story Mode";
            myMenuChoices[1] = "Arcade Mode";
            myMenuChoices[2] = "Highscore";
            myMenuChoices[3] = "Quit game";

            for (int i = 0; i < myMenuPositions.Length; i++)
            {
                myMenuPositions[i] = new Vector2(myWindowWidth / 2 - myMenuChoices[i].Length / 2, myWindowHeight / 2 + i * 2);
            }

            myHighScore = new HighScore();
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

        void MenuChoice(EMainMenuChoices aChoice)
        {
            switch (aChoice)
            {
                case EMainMenuChoices.StoryMode:
                    _ = new GameManager();
                    break;
                case EMainMenuChoices.ArcadeMode:
                    _ = new ArcadeManager(myHighScore);
                    break;
                case EMainMenuChoices.HighScore:
                    myHighScore.ShowHighScore();
                    break;
                case EMainMenuChoices.Quit:
                    Utilities.CursorPosition();
                    myGameIsRunning = false;
                    break;
                default:
                    break;
            }
        }

        enum EMainMenuChoices
        {
            StoryMode,
            ArcadeMode,
            HighScore,
            Quit
        }
    }
}
