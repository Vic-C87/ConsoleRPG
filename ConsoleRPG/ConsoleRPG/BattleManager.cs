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

        readonly Vector2[] myEnemySelectorPositons = new Vector2[3] { new Vector2(13, 12), new Vector2(13, 14), new Vector2(13, 16) };
        Vector2 myEnemyNameOnScreenPosition = new Vector2(14, 12); //Increment by Vector2.Down() x2 foreach

        readonly Vector2[] myActionSelectorPositions = new Vector2[3] { new Vector2(9, 32), new Vector2(9, 34), new Vector2(9, 36) };
        Vector2 myAttackOptionPosition = new Vector2(10, 32);
        Vector2 myMagicOptionPosition = new Vector2(10, 34);
        Vector2 myDefendOptionPosition = new Vector2(10, 36);

        readonly Vector2[] mySpellSelectorPositions = new Vector2[3] { new Vector2(20, 32), new Vector2(20, 34), new Vector2(20, 36) };
        Vector2 mySpellNamePosition = new Vector2(21, 32);

        Vector2 myVertFramePosition = new Vector2(30, 0);
        Vector2 myHoriLeftFramePosition = new Vector2(0, 25);

        bool mySelectAction = false;
        bool mySelectEnemy = false;
        bool mySelectSpell = false;

        public BattleManager()
        {
            myTurnListByBattleID = new List<int>();
            myPlayer = null;
            myEnemies = new List<Actor>();
            myPlayerStillAlive = false;
            myEnemiesStillAlive = false;
            myStopWatch = new Stopwatch();
            myVerticalFrame = Utilities.ReadFromFile(@"Sprites\VerticalFrame.txt", out string aTitle);
            myHorizontalLeftFrame = Utilities.ReadFromFile(@"Sprites\HorizontalLeftFrame.txt", out string bTitle);
            myPlayerCoolDownCounter = 0;

        }
        void ResetBattleScene()
        {
            myTurnListByBattleID = new List<int>();
            myEnemies = new List<Actor>();
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
            Utilities.CursorPosition(85, 22);
            Console.Write("Victory! ");
            SoundManager.PlaySound(SoundType.BattleVictory);
            Utilities.PressEnterToContinue();
            myStopWatch.Reset();
            aPlayerReference.myCurrentHP = myPlayer.myHP;
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
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < aRoomSprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + roomWallPosition.X, y + roomWallPosition.Y, aRoomSprite[x, y]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            DrawSprite(myVertFramePosition, myVerticalFrame);
            DrawSprite(myHoriLeftFramePosition, myHorizontalLeftFrame);
            Console.ForegroundColor = ConsoleColor.White;
            DrawEnemies();
            myPlayer.DrawSprite(myPlayerSpritePosition);
            UpdateHPDisplayed(myPlayer);
            SoundManager.PlaySound(SoundType.BattleStart);
        }

        static void DrawSprite(Vector2 anOffSet, char[,] aSprite)
        {
            for (int y = 0; y < aSprite.GetLength(1); y++)
            {
                for (int x = 0; x < aSprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + anOffSet.X, y + anOffSet.Y, aSprite[x,y]);
                }
            }
        }

        void DisplayCooldown()
        {
            //Utilities.CursorPosition(103, 34);
            //Console.Write(myPlayerCoolDownCounter);
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

        static void UpdateHPDisplayed(Actor anActor)
        {
            Utilities.Cursor(anActor.myBattlePosition.Up());
            Console.Write("        ");
            Utilities.Cursor(anActor.myBattlePosition.Up());
            Console.Write(anActor.myHP + "/" + anActor.myMaxHP + " HP");
        }

        void SelectAction()
        {
            Actions actionSelected;
            Utilities.Cursor(myActionSelectorPositions[0]);
            Console.Write(mySelectorRight);
            int selectIndex = 0;
            while (mySelectAction)
            {
                GetSelectionInput(myActionSelectorPositions, 3, FrameType.ActionFrame,ref selectIndex, ref mySelectAction);
            }
            actionSelected = (Actions)selectIndex;
            if (selectIndex == 0)
            {
                mySelectEnemy = true;
                selectIndex = 0;
                Utilities.Cursor(myEnemySelectorPositons[0]);
                Console.Write(mySelectorRight);
                while (mySelectEnemy)
                {
                    GetSelectionInput(myEnemySelectorPositons, 3, FrameType.EnemyNameFrame,ref selectIndex, ref mySelectEnemy);
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
                    GetSelectionInput(mySpellSelectorPositions, myPlayer.mySpellbook.GetSpellCount(), FrameType.SpellFrame,ref selectIndex, ref mySelectSpell);
                }
                int spellIndexToCast = selectIndex;
                mySelectEnemy = true;
                selectIndex = 0;
                Utilities.Cursor(myEnemySelectorPositons[0]);
                Console.Write(mySelectorRight);
                while (mySelectEnemy)
                {
                    GetSelectionInput(myEnemySelectorPositons, 3, FrameType.EnemyNameFrame,ref selectIndex, ref mySelectEnemy);
                }

                myPlayer.mySpellbook.UseSpell(spellIndexToCast, myEnemies[selectIndex]);
                UpdateHPDisplayed(myEnemies[selectIndex]);
                myPlayer.mySpellbook.CloseSpellbook(mySpellNamePosition);
                Thread.Sleep(1000);
            }
            else if (actionSelected == Actions.Defend)
            {
                myPlayer.Heal(5);
                UpdateHPDisplayed(myPlayer);
            }
            
        }

        void GetSelectionInput(Vector2[] someOptionPositions, int aMenuOptionsCount, FrameType aFrameType,ref int aSelectIndex, ref bool aSelectTypeBool)
        {
            ConsoleKeyInfo selection;

            selection = Console.ReadKey(true);

            

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

            if (!CheckAlive())
            {
                aSelectTypeBool = false;
            }
            
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
                    //Update cooldowntimer
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
                    myPlayer.myLastAttackTime = myStopWatch.ElapsedMilliseconds;
                    myTurnListByBattleID.RemoveAt(0);
                }
                else
                {
                    for (int i = 0; i < myEnemies.Count; i++)
                    {
                        if (myTurnListByBattleID.Count > 0) //ASK WHY I NEED THIS??!
                        {
                            if (myEnemies[i].myBattleID  == myTurnListByBattleID[0])
                            {
                                Thread.Sleep(1000);
                                if (myEnemies[i].myIsAlive)
                                {
                                    Attack(myEnemies[i], myPlayer);
                                    UpdateHPDisplayed(myPlayer);
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
            aTarget.TakeDamage(anAttacker.Attack());
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
            if (allEnemiesDead)
            {
                myEnemiesStillAlive = false;
            }

            return myEnemiesStillAlive;
        }

        
    }
}
