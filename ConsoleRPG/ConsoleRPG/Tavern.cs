using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Tavern
    {
        readonly char[,] mySprite;
        NPC myBartender;
        NPC myVillageDrunk;
        Vector2 myOffSet;
        bool myFirstVisit = true;

        public Tavern()
        {
            mySprite = Utilities.ReadFromFile(@"Sprites\Rooms\Tavern.txt", out _);
            myBartender = new NPC("Bartender", @"Dialogues\Villager1.txt");
            myVillageDrunk = null;
            myOffSet = new Vector2(Console.WindowWidth / 2 - mySprite.GetLength(0) / 2, Console.WindowHeight/2 - mySprite.GetLength(1) / 2);
        }

        public void EnterTavern()
        {
            Console.Clear();
            Utilities.DrawSprite(mySprite, myOffSet);

            //Call dialogue
            if (myFirstVisit)
            {
                Talk(myBartender, Quest.EnterTavern);
                myFirstVisit = false;
            }
            Console.ReadLine();
        }

        void Talk(NPC anNPC, Quest aQuest)
        {
            Dialogue dialogue = anNPC.GetDialogue(aQuest);
            string firstLine = dialogue.myNPCStart ? $"\t{anNPC.myName}: " : "\tPlayer: ";
            string secondLine = dialogue.myNPCStart ? "\tPlayer: " : $"\t{anNPC.myName}: ";
            Console.WriteLine("\n\n\n");
            for (int i = 1; i <= dialogue.myLineCountMax; i++)
            {
                Utilities.Color("\t~~~~~~\n\n", ConsoleColor.DarkRed);
                string[] testDialogue = dialogue.GetNextlines(i);
                if (testDialogue[0] != null)
                {
                    Console.WriteLine(firstLine + testDialogue[0]);
                    Console.ReadLine();
                }
                if (testDialogue[1] != null)
                {
                    Console.WriteLine();
                    Console.WriteLine(secondLine + testDialogue[1]);
                    Console.ReadLine();
                }
            }
        }
    }
}
