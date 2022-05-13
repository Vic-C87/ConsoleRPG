using System;
using WindowsInput;
using WindowsInput.Native;

namespace ConsoleRPG
{
    class Program
    {
        [System.Runtime.Versioning.SupportedOSPlatform("windows")]
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;

            InputSimulator inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.F11);

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            GameManager game = new GameManager();
        }       
    }    
}
