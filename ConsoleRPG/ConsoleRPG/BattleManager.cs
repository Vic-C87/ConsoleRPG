using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ConsoleRPG
{
    internal class BattleManager
    {
        const int NO_BATTLE_ID_FOUND = 99;
        const char mySelectorRight = '>';

        List<int> myTurnListByBattleID;
        Actor myPlayer;
        List<Actor> myEnemies;

        readonly char[,] myVerticalFrame;
        readonly char[,] myHorizontalLeftFrame;

        bool myPlayerStillAlive;
        bool myEnemiesStillAlive;
        readonly Stopwatch myStopWatch;

        int myPlayerCoolDownCounter;

        readonly Vector2[] myEnemySpritePositions = new Vector2[3] { new Vector2(60, 9), new Vector2(90, 9), new Vector2(120, 9) };
        readonly Vector2 myPlayerSpritePosition = new Vector2(103, 28);

        readonly Vector2 myFeedbackText = new Vector2(60, Console.WindowHeight / 3);

        readonly Vector2[] myEnemySelectorPositons = new Vector2[3] { new Vector2(13, 12), new Vector2(13, 14), new Vector2(13, 16) };
        Vector2 myEnemyNameOnScreenPosition = new Vector2(14, 12); //Increment by Vector2.Down() x2 foreach

        readonly Vector2[] myActionSelectorPositions = new Vector2[3] { new Vector2(9, 28), new Vector2(9, 30), new Vector2(9, 32) };
        Vector2 myAttackOptionPosition = new Vector2(10, 28);
        Vector2 myMagicOptionPosition = new Vector2(10, 30);
        Vector2 myDefendOptionPosition = new Vector2(10, 32);

        readonly Vector2[] mySpellSelectorPositions = new Vector2[3] { new Vector2(20, 28), new Vector2(20, 30), new Vector2(20, 32) };
        Vector2 mySpellNamePosition = new Vector2(21, 28);

        Vector2 myVertFramePosition = new Vector2(30, 0);
        Vector2 myHoriLeftFramePosition = new Vector2(0, 19);
        Vector2 myCoolDownText = new Vector2(103, 34);
        Vector2 myEndBattleText = new Vector2(85, 22);
        bool mySelectAction = false;
        bool mySelectEnemy = false;
        bool mySelectSpell = false;
        Vector2 myOffSet;

        public BattleManager()
        {
            myTurnListByBattleID = new List<int>();
            myPlayer = null;
            myEnemies = new List<Actor>();
            myPlayerStillAlive = false;
            myEnemiesStillAlive = false;
            myStopWatch = new Stopwatch();
            myVerticalFrame = Utilities.ReadFromFile(@"Sprites\BattleVerticalFrame.txt", out _);
            myHorizontalLeftFrame = Utilities.ReadFromFile(@"Sprites\HorizontalLeftFrame.txt", out _);
            myPlayerCoolDownCounter = 0;
            myOffSet = new Vector2(Console.WindowWidth / 8, Console.WindowHeight / 5);

            for (int i = 0; i < 3; i++)
            {
                myEnemySpritePositions[i].OffSet(myOffSet);
                myEnemySelectorPositons[i].OffSet(myOffSet);
                myActionSelectorPositions[i].OffSet(myOffSet);
                mySpellSelectorPositions[i].OffSet(myOffSet);
            }
            myPlayerSpritePosition.OffSet(myOffSet);
            myFeedbackText.OffSet(myOffSet);
            myEnemyNameOnScreenPosition.OffSet(myOffSet);
            myAttackOptionPosition.OffSet(myOffSet);
            myMagicOptionPosition.OffSet(myOffSet);
            myDefendOptionPosition.OffSet(myOffSet);
            mySpellNamePosition.OffSet(myOffSet);
            myVertFramePosition.OffSet(myOffSet);
            myHoriLeftFramePosition.OffSet(myOffSet);
            myCoolDownText.OffSet(myOffSet);
            myEndBattleText.OffSet(myOffSet);
        }
        public void StartBattle(List<Actor> anEnemyList, Player aPlayerReference, GameManager theGameManager, char[,] aRoomSprite)
        {            
            myPlayer = new Actor(aPlayerReference);
            myPlayerCoolDownCounter = myPlayer.myCoolDown/1000;
            myPlayerStillAlive = true;
            myEnemiesStillAlive = true;
            myTurnListByBattleID.Add(myPlayer.myBattleID);
            for (int i = 0; i < anEnemyList.Count; i++)
            {
                myEnemies.Add(anEnemyList[i]);
                myEnemies[i].myBattleID = i + 1;
                myTurnListByBattleID.Add(myEnemies[i].myBattleID);
            }            
            myStopWatch.Start();
            DrawBattleScene(aRoomSprite);
            while (myPlayerStillAlive && myEnemiesStillAlive)
            {
                SetTurnList();
                Act();
                CheckAlive();
            }
            Utilities.Cursor(myEndBattleText);
            if (myPlayerStillAlive)
            {
                Console.Write("Victory!");
                SoundManager.PlaySound(SoundType.BattleVictory);
            }
            else
            {
                Console.Write("You Loose!");
                Utilities.Cursor(myEndBattleText.Down());
                Console.Write("You will respawn in the village.");
                aPlayerReference.myDeathCounter++;
                SoundManager.PlaySound(SoundType.BattleLost);
            }
            Utilities.Cursor(myEndBattleText.Down(2));
            Utilities.PressEnterToContinue();
            myStopWatch.Reset();
            aPlayerReference.myCurrentHP = myPlayer.myHP;
            aPlayerReference.myCurrentMP = myPlayer.myMP;
            ResetBattleScene();
            theGameManager.EndBattle();
        }

        void DrawBattleScene(char[,] aRoomSprite)
        {
            Vector2 nameOnScreenTemp = myEnemyNameOnScreenPosition;
            Console.Clear();
            for (int i = 0; i < myEnemies.Count; i++)
            {
                myEnemies[i].myBattleNameOnScreenPosition = myEnemyNameOnScreenPosition;
                Utilities.Cursor(myEnemyNameOnScreenPosition);
                Console.WriteLine(myEnemies[i].myName);
                myEnemyNameOnScreenPosition = myEnemyNameOnScreenPosition.Down();
                myEnemyNameOnScreenPosition = myEnemyNameOnScreenPosition.Down();
            }
            myEnemyNameOnScreenPosition = nameOnScreenTemp;
            Utilities.Cursor(myAttackOptionPosition);
            Console.WriteLine("Attack");
            Utilities.Cursor(myMagicOptionPosition);
            Console.WriteLine("Magic");
            Utilities.Cursor(myDefendOptionPosition);
            Console.WriteLine("Defend");
            Vector2 roomWallPosition = new Vector2(50, 0);
            roomWallPosition.OffSet(myOffSet);
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < aRoomSprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + roomWallPosition.X, y + roomWallPosition.Y, aRoomSprite[x, y]);
                }
            }
            Utilities.DrawSprite(myVerticalFrame, myVertFramePosition, ConsoleColor.DarkYellow);
            Utilities.DrawSprite(myHorizontalLeftFrame, myHoriLeftFramePosition, ConsoleColor.DarkYellow);
            int xFrameLenght = (roomWallPosition.X - myHoriLeftFramePosition.X) + aRoomSprite.GetLength(0);
            Vector2 framePos = myOffSet.Left();
            framePos = framePos.Up();
            Utilities.Cursor(framePos);
            for (int i = 0; i < xFrameLenght; i++)
            {
                Utilities.Color("~", ConsoleColor.DarkRed);
            }
            Utilities.Cursor(framePos.Down(38));
            for (int i = 0; i < xFrameLenght; i++)
            {
                Utilities.Color("~", ConsoleColor.DarkRed);
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (int i = 0; i < 39; i++)
            {
                Utilities.Draw(framePos.X, framePos.Down(i).Y, '§');
                Utilities.Draw(framePos.X + xFrameLenght, framePos.Down(i).Y, '§');
            }
            Console.ForegroundColor = ConsoleColor.White;

            DrawEnemies();
            myPlayer.DrawSprite(myPlayerSpritePosition);
            UpdateHPDisplayed(myPlayer);
            DisplayCooldown();
            SoundManager.PlaySound(SoundType.BattleStart);
        }

        void DrawEnemies()
        {
            int numberOfEnemies = myEnemies.Count;

            switch (numberOfEnemies)
            {
                case 1:
                    myEnemies[0].DrawSprite(myEnemySpritePositions[1]);
                    break;
                case 2:
                    myEnemies[0].DrawSprite(myEnemySpritePositions[0]);
                    myEnemies[1].DrawSprite(myEnemySpritePositions[2]);
                    break;
                case 3:
                    myEnemies[0].DrawSprite(myEnemySpritePositions[0]);
                    myEnemies[1].DrawSprite(myEnemySpritePositions[1]);
                    myEnemies[2].DrawSprite(myEnemySpritePositions[2]);
                    break;
                default:
                    break;
            }

            for (int i = 0; i < numberOfEnemies; i++)
            {
                UpdateHPDisplayed(myEnemies[i]);
            }
        }

        void ResetBattleScene()
        {
            myTurnListByBattleID = new List<int>();
            myEnemies = new List<Actor>();
        }

        static void UpdateHPDisplayed(Actor anActor)
        {
            if (!anActor.myIsPlayer)
            {
                Utilities.Cursor(anActor.myBattlePosition.Up());
                Console.Write("        ");
                Utilities.Cursor(anActor.myBattlePosition.Up());
                Console.Write(anActor.myHP + "/" + anActor.myMaxHP + " HP");
            }
            else
            {
                int xPos = anActor.myBattlePosition.X;
                int yPos = anActor.myBattlePosition.Y + anActor.mySprite.GetLength(1) + 1;
                Utilities.CursorPosition(xPos, yPos);
                Console.Write("        ");
                Utilities.CursorPosition(xPos, yPos);
                Console.Write(anActor.myHP + "/" + anActor.myMaxHP + " HP");
                Utilities.CursorPosition(xPos, yPos + 1);
                Console.Write("        ");
                Utilities.CursorPosition(xPos, yPos + 1);
                Console.Write(anActor.myMP + "/" + anActor.myMaxMP + " MP");
            }
        }

        void DisplayCooldown()
        {            
            Utilities.Cursor(myCoolDownText);
            Console.Write("CoolDown: " + myPlayerCoolDownCounter + "s.");
        }

        void SelectAction()
        {
            Actions actionSelected;
            Utilities.Cursor(myActionSelectorPositions[0]);
            Console.Write(mySelectorRight);
            int selectIndex = 0;
            bool goBack = false;
            while (mySelectAction)
            {
                GetSelectionInput(myActionSelectorPositions, 3, FrameType.ActionFrame,ref selectIndex, ref mySelectAction, out _);
            }
            ClearDisplayDamage();
            actionSelected = (Actions)selectIndex;
            if (selectIndex == 0)
            {
                mySelectEnemy = true;
                selectIndex = 0;
                Utilities.Cursor(myEnemySelectorPositons[0]);
                Console.Write(mySelectorRight);
                while (mySelectEnemy)
                {
                    GetSelectionInput(myEnemySelectorPositons, myEnemies.Count, FrameType.EnemyNameFrame,ref selectIndex, ref mySelectEnemy, out _);
                }
            }
            if (actionSelected == Actions.Attack)
            {
                Attack(myPlayer, myEnemies[selectIndex]);
                UpdateHPDisplayed(myEnemies[selectIndex]);
                Thread.Sleep(1000);
            }
            else if (actionSelected == Actions.Magic)
            {
                myPlayer.mySpellbook.OpenSpellBook(mySpellNamePosition);
                mySelectSpell = true;
                selectIndex = 0;
                Utilities.Cursor(mySpellSelectorPositions[0]);
                Console.Write(mySelectorRight);
                while (mySelectSpell)
                {
                    GetSelectionInput(mySpellSelectorPositions, myPlayer.mySpellbook.GetSpellCount(), FrameType.SpellFrame,ref selectIndex, ref mySelectSpell, out goBack);

                }
                int spellIndexToCast = selectIndex;
                int spellCost = myPlayer.mySpellbook.GetSpellCost(spellIndexToCast);
                mySelectEnemy = true;
                selectIndex = 0;
                Utilities.Cursor(myEnemySelectorPositons[0]);
                Console.Write(mySelectorRight);
                while (mySelectEnemy && !goBack && spellCost <= myPlayer.myMP)
                {
                    GetSelectionInput(myEnemySelectorPositons, myEnemies.Count, FrameType.EnemyNameFrame,ref selectIndex, ref mySelectEnemy, out _);
                }
                
                if (myPlayer.myMP >= spellCost && !goBack)
                {
                    myPlayer.mySpellbook.UseSpell(spellIndexToCast, myEnemies[selectIndex]);
                    myPlayer.myMP -= spellCost;
                    UpdateHPDisplayed(myEnemies[selectIndex]);
                    myPlayer.mySpellbook.CloseSpellbook(mySpellNamePosition);
                    Thread.Sleep(1000);
                }
                else
                {
                    Utilities.Cursor(myFeedbackText);
                    Utilities.Color("Not enough MANA", ConsoleColor.Blue);
                    Thread.Sleep(1000);
                    Utilities.Cursor(myFeedbackText);
                    Console.Write("               ");
                    myPlayer.mySpellbook.CloseSpellbook(mySpellNamePosition);
                    mySelectAction = true;
                    Utilities.Cursor(myEnemySelectorPositons[0]);
                    Console.Write(" ");
                    SelectAction();
                }                
            }
            else if (actionSelected == Actions.Defend)
            {
                myPlayer.Heal(5);
                UpdateHPDisplayed(myPlayer);
            }           
        }

        void GetSelectionInput(Vector2[] someOptionPositions, int aMenuOptionsCount, FrameType aFrameType,ref int aSelectIndex, ref bool aSelectTypeBool, out bool aGoBackOption)
        {
            ConsoleKeyInfo selection;
            
            while (Console.KeyAvailable)
                Console.ReadKey(true);

            selection = Console.ReadKey(true);

            aGoBackOption = false;
            if (selection.Key == ConsoleKey.UpArrow)
            {
                Utilities.Cursor(someOptionPositions[aSelectIndex]);
                Console.Write(" ");
                aSelectIndex--;
                if (aSelectIndex < 0)
                {
                    aSelectIndex = aMenuOptionsCount - 1;
                }
                Utilities.Cursor(someOptionPositions[aSelectIndex]);
                Console.Write(mySelectorRight);
            }
            else if (selection.Key == ConsoleKey.DownArrow)
            {
                Utilities.Cursor(someOptionPositions[aSelectIndex]);
                Console.Write(" ");
                aSelectIndex++;
                if (aSelectIndex > aMenuOptionsCount - 1)
                {
                    aSelectIndex = 0;
                }
                Utilities.Cursor(someOptionPositions[aSelectIndex]);
                Console.Write(mySelectorRight);                
            }
            else if (selection.Key == ConsoleKey.Enter)
            {
                if (aFrameType == FrameType.EnemyNameFrame)
                {
                    if (myEnemies[aSelectIndex].myIsAlive)
                    {
                        aSelectTypeBool = false;
                        Utilities.Cursor(someOptionPositions[aSelectIndex]);
                        Console.Write(" ");
                    }
                }
                else
                {
                    aSelectTypeBool = false;
                    Utilities.Cursor(someOptionPositions[aSelectIndex]);
                    Console.Write(" ");
                }
            }
            else if (selection.Key == ConsoleKey.Escape && FrameType.SpellFrame == aFrameType)
            {
                aGoBackOption = true;
                aSelectTypeBool = false;
                Utilities.Cursor(someOptionPositions[aSelectIndex]);
                Console.Write(" ");
            }

            if (!CheckAlive())
            {
                aSelectTypeBool = false;
            }            
        }

        void ClearDisplayDamage()
        {
            Utilities.CursorPosition(myCoolDownText.X, myOffSet.Y + 39);
            Console.Write("                                         ");
        }

        void DisplayDamage(string anAttackersName, int someDamageMade)
        {
            ClearDisplayDamage();
            Utilities.CursorPosition(myCoolDownText.X, myOffSet.Y + 39);
            Console.Write($"{anAttackersName} attacked you for {someDamageMade} damage.");
        }

        void SetTurnList()
        {
            Dictionary<int, int> waitingSinceCooldown = new Dictionary<int, int>();
            if (myPlayer.myLastAttackTime != 0)
            {
                int timeSinceLastAttack = (int)(myStopWatch.ElapsedMilliseconds - myPlayer.myLastAttackTime);
                if (timeSinceLastAttack > myPlayer.myCoolDown)
                {
                    waitingSinceCooldown.Add(myPlayer.myBattleID, timeSinceLastAttack - myPlayer.myCoolDown);
                    myPlayer.myLastAttackTime = 0;
                    myPlayerCoolDownCounter = 0;
                    DisplayCooldown();
                }
                else
                {
                    myPlayerCoolDownCounter = (myPlayer.myCoolDown - timeSinceLastAttack)/1000;
                    DisplayCooldown();
                    //Update cooldowntimer!!!!!
                }
            }
            for (int i = 0; i < myEnemies.Count; i++)
            {
                if (myEnemies[i].myLastAttackTime == 0 || !myEnemies[i].myIsAlive)
                {
                    continue;
                }
                int timeSinceLastAttack = (int)(myStopWatch.ElapsedMilliseconds - myEnemies[i].myLastAttackTime);
                if (timeSinceLastAttack > myEnemies[i].myCoolDown)
                {
                    waitingSinceCooldown.Add(myEnemies[i].myBattleID, timeSinceLastAttack - myEnemies[i].myCoolDown);
                    myEnemies[i].myLastAttackTime = 0;
                }
            }
            
            int highestWaitingTime = 0;
            int highestBattleID = NO_BATTLE_ID_FOUND;
            while (waitingSinceCooldown.Count > 0)
            {
                foreach (KeyValuePair<int, int> unSortedActors in waitingSinceCooldown)
                {
                    if(unSortedActors.Value > highestWaitingTime)
                    {
                        highestWaitingTime = unSortedActors.Value;
                        highestBattleID = unSortedActors.Key;
                    }
                }
                myTurnListByBattleID.Add(highestBattleID);
                waitingSinceCooldown.Remove(highestBattleID);
                highestWaitingTime = 0;
            }
        }

        void Act()
        {
            if (myTurnListByBattleID.Count > 0)
            {
                if (myTurnListByBattleID[0] == NO_BATTLE_ID_FOUND)
                {
                    myTurnListByBattleID.RemoveAt(0);
                }
                if(myTurnListByBattleID[0] == 0)
                {
                    mySelectAction = true;
                    SelectAction();
                    UpdateHPDisplayed(myPlayer);
                    myPlayer.myLastAttackTime = myStopWatch.ElapsedMilliseconds;
                    myTurnListByBattleID.RemoveAt(0);
                }
                else
                {
                    for (int i = 0; i < myEnemies.Count; i++)
                    {
                        if (myTurnListByBattleID.Count > 0) 
                        {
                            if (myEnemies[i].myBattleID  == myTurnListByBattleID[0])
                            {                                                                
                                if (myEnemies[i].myIsAlive)
                                {
                                    Attack(myEnemies[i], myPlayer);
                                    UpdateHPDisplayed(myPlayer);
                                    if (myPlayer.myHP < 1)
                                    {
                                        myPlayerStillAlive = false;
                                        return;
                                    }
                                }
                                
                                myEnemies[i].myLastAttackTime = myStopWatch.ElapsedMilliseconds;
                                myTurnListByBattleID.RemoveAt(0);
                            }
                        }
                    }
                }
            }
        }

        void Attack(Actor anAttacker,Actor aTarget)
        {
            aTarget.TakeDamage(anAttacker.Attack(), out int damageMade);
            if (aTarget.myIsPlayer)
            {
                DisplayDamage(anAttacker.myName, damageMade);
            }
            if (aTarget.myHP < 1 && !aTarget.myIsPlayer)
            {
                aTarget.Die();
            }
            CheckAlive();
        }

        bool CheckAlive()
        {
            bool allEnemiesDead = true;
            for (int i = 0; i < myEnemies.Count; i++)
            {
                if (myEnemies[i].myIsAlive)
                {
                    allEnemiesDead = false;
                }
            }
            myEnemiesStillAlive = !allEnemiesDead;

            return myEnemiesStillAlive;
        }        
    }
}
