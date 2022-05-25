using System;
using System.Collections.Generic;

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
            mySprite = Utilities.ReadFromFile(@"Sprites/Rooms/Tavern.txt", out _);
            myBartender = new NPC("Bartender", @"Dialogues/Villager1.txt");
            myBartender.AddDialogue(Quest.FirstKey, @"Dialogues/FirstKey.txt");
            myOffSet = new Vector2(Console.WindowWidth / 2 - mySprite.GetLength(0) / 2, Console.WindowHeight/2 - mySprite.GetLength(1) / 2);
            myLineAction = () => PrintLine();
        }

        public void EnterTavern()
        {
            Console.Clear();
            if (myFirstVisit)
            {
                Utilities.Cursor(myOffSet.Up(5));
                Utilities.Typewriter("After arriving at the village the soldier decides to rest up in the tavern before continuing on towards the mansion.", 50);
                Utilities.Cursor(myOffSet.Up(3));
                Console.Write("(Press \'Enter\' to continue)");
                Utilities.ActionByInput(() => EmptyMethod(), ConsoleKey.Enter);
                Console.Clear();
            }
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
                Utilities.ActionByInput(() => EmptyMethod(), ConsoleKey.Enter);
                Console.Clear();
                List<string> tavernOne = Utilities.GetPrologue(@"Dialogues/TavernOne.txt");
                PrintRollingText(tavernOne, myOffSet);
                
                myFirstVisit = false;
            }
            Utilities.ActionByInput(() => EmptyMethod(), ConsoleKey.Enter);
        }

        static void PrintRollingText(List<string> someText, Vector2 anOffSet)
        {
            for (int i = 0; i < someText.Count; i++)
            {
                Utilities.Cursor(anOffSet.Down(i));
                Utilities.Typewriter(someText[i], 50);
            }
            Utilities.ActionByInput(() => EmptyMethod(), ConsoleKey.Enter);
            Console.Clear();
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
