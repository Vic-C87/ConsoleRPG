using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Dialogue
    {
        public string myTitle;
        public int myDialogueID;
        List<string> myDialogue = new List<string>();

        public Dialogue(string aTitle, int anID, List<string> aDialogueList)
        {
            myTitle = aTitle;
            myDialogueID = anID;
            myDialogue = aDialogueList;
        }

    }
}
