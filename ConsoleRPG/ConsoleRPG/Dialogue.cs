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
        public bool myNPCStart;

        public Dictionary<int, string> myNPCLines = new Dictionary<int, string>();
        public Dictionary<int, string> myPlayerLines = new Dictionary<int, string>();

        public Dialogue(string aTitle, int anID, Dictionary<int, string> aNPCDialogue, Dictionary<int, string> aPlayerDialogue, bool aNPCStart)
        {
            myTitle = aTitle;
            myDialogueID = anID;
            myNPCLines = aNPCDialogue;
            myPlayerLines = aPlayerDialogue;
            myNPCStart = aNPCStart;
        }

    }
}
