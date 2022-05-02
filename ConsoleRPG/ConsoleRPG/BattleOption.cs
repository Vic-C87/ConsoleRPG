using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal struct BattleOption
    {
        public string myTitle;
        public int myID;
        public Vector2 myPosition;


        public BattleOption(string aTitle, int anID, Vector2 aPosition)
        {
            myTitle = aTitle;
            myID = anID;
            myPosition = aPosition;
        }

        public void PrintName()
        {
            Utilities.Cursor(myPosition);
            Console.Write(myTitle);
        }

        public void ClearName()
        {
            Utilities.Cursor(myPosition);
            Console.Write("                  ");
        }
    }
}
