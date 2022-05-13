using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class FarmScene
    {
        string myTitle;
        char[,] myScene;
        GameObject myPlayer;


        public FarmScene()
        {
            myScene = Utilities.ReadFromFile(@"Sprites\CutScene\FarmHouse.txt", out myTitle);
            string title;
            char[,] playerSprite = Utilities.ReadFromFile(@"Sprites\Man.txt", out title);
            myPlayer = new GameObject(new Vector2(playerSprite.GetLength(0), playerSprite.GetLength(1)), title, playerSprite);
        }

        public void DrawScene()
        {
            Vector2 sceneOffSet = new Vector2(Console.WindowWidth/2 - myScene.GetLength(0)/2, Console.WindowHeight/2 - myScene.GetLength(1)/2);
            Utilities.DrawSprite(myScene, sceneOffSet);
            Console.ReadLine();
        }
    }
}
