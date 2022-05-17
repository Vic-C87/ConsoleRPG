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
        readonly NPC myBartender;
        Vector2 myOffSet;
        bool myFirstVisit = true;
        public bool myFirstKey = false;
        public bool mySecondKey = false;
        string myLineToPrint;
        string mySpeaker;
        readonly Action myLineAction;

        public Tavern()
        {
            mySprite = Utilities.ReadFromFile(@"Sprites\Rooms\Tavern.txt", out _);
            myBartender = new NPC("Bartender", @"Dialogues\Villager1.txt");
            myBartender.AddDialogue(Quest.FirstKey, @"Dialogues\FirstKey.txt");
            myOffSet = new Vector2(Console.WindowWidth / 2 - mySprite.GetLength(0) / 2, Console.WindowHeight/2 - mySprite.GetLength(1) / 2);
            myLineAction = () => PrintLine();
        }

        public void EnterTavern()
        {
            Console.Clear();
            Utilities.DrawSprite(mySprite, myOffSet);

            if (!myFirstVisit && mySecondKey)
            {
                //Dialogue!!!
            }
            if (!myFirstVisit && myFirstKey)
            {
                Talk(myBartender, Quest.FirstKey);
            }
            if (myFirstVisit)
            {
                Talk(myBartender, Quest.EnterTavern);
                //Screen with story of barman
                myFirstVisit = false;
            }
            Utilities.ActionByInput(() => EmptyMethod(), ConsoleKey.Enter);
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
                    mySpeaker = firstLine;
                    myLineToPrint = testDialogue[0];
                    Utilities.ActionByInput(myLineAction, ConsoleKey.Enter);
                }
                if (testDialogue[1] != null)
                {
                    Console.WriteLine();
                    mySpeaker = secondLine;
                    myLineToPrint = testDialogue[1];
                    Utilities.ActionByInput(myLineAction, ConsoleKey.Enter);
                }
                Console.WriteLine();
            }
        }

        static void EmptyMethod()
        {

        }

        void PrintLine()
        {
            Console.Write(mySpeaker);
            Utilities.Typewriter(myLineToPrint, 50);
            Console.WriteLine();
        }
    }
}
