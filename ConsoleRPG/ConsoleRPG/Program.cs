using System;

namespace ConsoleRPG
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            if (OperatingSystem.IsWindows())
            {
                Console.BufferWidth = Console.WindowWidth;
                Console.BufferHeight = Console.WindowHeight;
            }
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Utilities.Typewriter("Please maximize your console window for best game experience.", 50, ConsoleColor.DarkRed);
            Console.WriteLine("\n\n\n");
            Utilities.Typewriter("Press \'Enter\' when screen is maximized to continue", 50, ConsoleColor.DarkGreen);
            Utilities.ActionByInput(() => ContinueToGame(), ConsoleKey.Enter);
        }   
        
        static void ContinueToGame()
        {
            Console.Clear();
            _ = new GameManager();
        }
    }    
}
