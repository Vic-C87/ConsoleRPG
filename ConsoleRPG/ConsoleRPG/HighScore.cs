using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class HighScore
    {
        Dictionary<int, string> myScores = new Dictionary<int, string>();
        string highScorePath;
        public HighScore()
        {
            highScorePath = @"HighScore/HighScores.txt";
            myScores = Utilities.GetHighScoreListFromFile(highScorePath);
        }

        public void ShowHighScore()
        {
            SortHighScore();
            Console.Clear();
            int xName = Console.WindowWidth / 3;
            int xScore = xName + 25;
            int y = Console.WindowHeight / 4;
            Utilities.CursorPosition(Console.WindowWidth/2, Console.WindowHeight/4);
            foreach (KeyValuePair<int, string> highscore in myScores)
            {
                Utilities.CursorPosition(xName, y);
                Console.WriteLine(highscore.Value);
                Utilities.CursorPosition(xScore, y++);
                Console.WriteLine(highscore.Key);
            }
            y++;
            Utilities.CursorPosition(xName, y++);
            Utilities.PressEnterToContinue();
        }

        public void NewHighScore(string aName, int somePoints)
        {
            myScores.Add(somePoints, aName);
            SortHighScore();
            WriteHighScoreToFile();
        }

        public void SortHighScore()
        {
            Dictionary<int , string> tempScores = new Dictionary<int , string>();
            tempScores = myScores;
            myScores = new Dictionary<int, string>();

            foreach(KeyValuePair<int,string> score in tempScores.OrderByDescending( order => order.Key))
            {
                myScores.Add(score.Key, score.Value);
            }
        }

        public void WriteHighScoreToFile()
        {
            Utilities.SaveHighScore(myScores, highScorePath);
        }
    }
}
