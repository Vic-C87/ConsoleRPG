using System;
using WindowsInput;
using WindowsInput.Native;

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

            InputSimulator inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyDown(VirtualKeyCode.F11);

            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            _ = new MainMenu();
        }       
    }    
}
