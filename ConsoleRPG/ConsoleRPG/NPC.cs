using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class NPC
    {
        public string myName;
        readonly Dictionary<Quest, Dialogue> myInteractions = new Dictionary<Quest, Dialogue>();

        public NPC(string aName, string aDialoguePath)
        {
            myName = aName;
            myInteractions.Add(Quest.EnterTavern, Utilities.GetDialogue(aDialoguePath));            
        }

        public void AddDialogue(Quest aQuest, string aPath)
        {
            myInteractions.Add(aQuest, Utilities.GetDialogue(aPath));
        }

        public Dialogue GetDialogue(Quest aQuest)
        {
            if (myInteractions.ContainsKey(aQuest))
            {
                return myInteractions[aQuest];
            }
            return null;
        }
    }
}
