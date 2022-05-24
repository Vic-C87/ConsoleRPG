using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class Dialogue
    {
        public string myTitle;
        public int myDialogueID;        
        public bool myNPCStart;
        public int myLineCountMax;

        public Dictionary<int, string> myNPCLines = new Dictionary<int, string>();
        public Dictionary<int, string> myPlayerLines = new Dictionary<int, string>();

        public Dialogue(string aTitle, int anID, Dictionary<int, string> aNPCDialogue, Dictionary<int, string> aPlayerDialogue, bool aNPCStart)
        {
            myTitle = aTitle;
            myDialogueID = anID;
            myNPCLines = aNPCDialogue;
            myPlayerLines = aPlayerDialogue;
            myNPCStart = aNPCStart;
            myLineCountMax = MaxLines();
        }

        int MaxLines()
        {
            return myNPCLines.Count >= myPlayerLines.Count ? myNPCLines.Count : myPlayerLines.Count;
        }

        public string[] GetNextlines(int anIndex)
        {
            string[] lines = new string[2];
            if (!myNPCLines.ContainsKey(anIndex) && !myPlayerLines.ContainsKey(anIndex))
            {
                return null;
            }

            int lineIndexNPC = myNPCStart ? 0 : 1;
            int lineIndexPlayer = myNPCStart ? 1 : 0;

           
            if (myNPCLines.ContainsKey(anIndex))
            {
                lines[lineIndexNPC] = myNPCLines[anIndex];
            }
            else
            {
                lines[lineIndexNPC] = null;
            }

            if (myPlayerLines.ContainsKey(anIndex))
            {
                lines[lineIndexPlayer] = myPlayerLines[anIndex];
            }
            else
            {
                lines[lineIndexPlayer] = null;
            }

            return lines;
            
        }

    }
}
