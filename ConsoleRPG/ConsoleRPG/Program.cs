using System;

namespace ConsoleRPG
{
    class Program
    {
        const int WINDOW_WIDTH = 170;
        const int WINDOW_HEIGHT = 44;

        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT);
            //Set buffert
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            GameManager game = new GameManager(WINDOW_WIDTH, WINDOW_HEIGHT);                      
        }       
    }    
}
