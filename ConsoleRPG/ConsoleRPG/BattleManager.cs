using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Media;

namespace ConsoleRPG
{
    internal class BattleManager
    {
        const int NO_BATTLE_ID_FOUND = 99;
        const char mySelectorRight = '>';

        List<int> myTurnListByBattleID;
        Actor myPlayer;
        List<Actor> myEnemies;

        bool myPlayerStillAlive;
        bool myEnemiesStillAlive;

        SoundPlayer myBatHitPlayer;
        SoundPlayer myPlayerHitPlayer;
        SoundPlayer myBattleStartPlayer;
        SoundPlayer myHealEffectPlayer;
        SoundPlayer myBatLightningHitPlayer;
        SoundPlayer myBattleVictoryPlayer;
        Stopwatch myStopWatch;

        int myPlayerCoolDownCounter;

        Vector2 myEnemySpritePosition = new Vector2(80, 5);
        Vector2 myPlayerSpritePosition = new Vector2(93, 26);

        Vector2[] myEnemySelectorPositons = new Vector2[3] { new Vector2(9, 5), new Vector2(9, 7), new Vector2(9, 9) };
        Vector2 myEnemyNameOnScreenPosition = new Vector2(10, 5); //Increment by Vector2.Down() x2 foreach

        Vector2[] myActionSelectorPositions = new Vector2[3] { new Vector2(9, 32), new Vector2(9, 34), new Vector2(9, 36) };
        Vector2 myAttackOptionPosition = new Vector2(10, 32);
        Vector2 myMagicOptionPosition = new Vector2(10, 34);
        Vector2 myDefendOptionPosition = new Vector2(10, 36);

        Vector2[] mySpellSelectorPositions = new Vector2[3] { new Vector2(20, 32), new Vector2(20, 34), new Vector2(20, 36) };
        Vector2 mySpellNamePosition = new Vector2(21, 32);

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
            myPlayerCoolDownCounter = 0;
            if (OperatingSystem.IsWindows())
            {
                myBatHitPlayer = new SoundPlayer(@"Audio\batHit.wav");
                myPlayerHitPlayer = new SoundPlayer(@"Audio\playerHit.wav");
                myBattleStartPlayer = new SoundPlayer(@"Audio\battleStart.wav");
                myHealEffectPlayer = new SoundPlayer(@"Audio\healEffect.wav");
                myBatLightningHitPlayer = new SoundPlayer(@"Audio\batLightningHit.wav");
                myBattleVictoryPlayer = new SoundPlayer(@"Audio\battleVictory.wav");
            }
        }

        public void StartBattle(List<Actor> anEnemyList, Player aPlayerReference, GameManager theGameManager)
        {
            myEnemies = new List<Actor>(anEnemyList);
            myPlayer = new Actor(aPlayerReference);
            myPlayerCoolDownCounter = myPlayer.myCoolDown/1000;
            myPlayerStillAlive = true;
            myEnemiesStillAlive = true;
            myTurnListByBattleID.Add(myPlayer.myBattleID);
            for (int i = 0; i < anEnemyList.Count; i++)
            {
                anEnemyList[i].myBattleID = i + 1;
                myTurnListByBattleID.Add(myEnemies[i].myBattleID);
            }
            
            myStopWatch.Start();
            DrawBattleScene();
            while (myPlayerStillAlive && myEnemiesStillAlive)
            {
                SetTurnList();
                Act();
            }
            Utilities.CursorPosition(85, 22);
            Console.Write("Victory! ");
            PlaySound(myBattleVictoryPlayer);
            Utilities.PressEnterToContinue();
            myStopWatch.Reset();
            aPlayerReference.myCurrentHP = myPlayer.myHP;
            theGameManager.EndBattle();
        }

        void DrawBattleScene()
        {
            Vector2 nameOnScreenTemp = myEnemyNameOnScreenPosition;
            Console.Clear();
            for (int i = 0; i < myEnemies.Count; i++)
            {
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

            myEnemies[0].DrawSprite(myEnemySpritePosition);
            UpdateHPDisplayed(myEnemySpritePosition.Up(), myEnemies[0]);
            myPlayer.DrawSprite(myPlayerSpritePosition);
            UpdateHPDisplayed(myPlayerSpritePosition.Up(), myPlayer);
            PlaySound(myBattleStartPlayer);
        }

        void UpdateHPDisplayed(Vector2 aScreenPosition, Actor anActor)
        {
            Utilities.Cursor(aScreenPosition);
            Console.Write("        ");
            Utilities.Cursor(aScreenPosition);
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
                GetSelectionInput(myActionSelectorPositions, 3, ref selectIndex, ref mySelectAction);
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
                    GetSelectionInput(myEnemySelectorPositons, myEnemies.Count, ref selectIndex, ref mySelectEnemy);
                }

            }
            if (actionSelected == Actions.Attack)
            {
                Attack(myPlayer, myEnemies[selectIndex]);
                PlaySound(myBatHitPlayer);
                UpdateHPDisplayed(myEnemySpritePosition.Up(), myEnemies[selectIndex]);
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
                    GetSelectionInput(mySpellSelectorPositions, myPlayer.mySpellbook.GetSpellCount(), ref selectIndex, ref mySelectSpell);
                }
                int spellIndexToCast = selectIndex;
                mySelectEnemy = true;
                selectIndex = 0;
                Utilities.Cursor(myEnemySelectorPositons[0]);
                Console.Write(mySelectorRight);
                while (mySelectEnemy)
                {
                    GetSelectionInput(myEnemySelectorPositons, myEnemies.Count, ref selectIndex, ref mySelectEnemy);
                }

                myPlayer.mySpellbook.UseSpell(spellIndexToCast, myEnemies[selectIndex]);
                PlaySound(myBatLightningHitPlayer);
                UpdateHPDisplayed(myEnemySpritePosition.Up(), myEnemies[selectIndex]);
                CheckAlive(myEnemies[selectIndex]);
                myPlayer.mySpellbook.CloseSpellbook(mySpellNamePosition);
                Thread.Sleep(1000);


            }
            else if (actionSelected == Actions.Defend)
            {
                myPlayer.Heal(5);
                PlaySound(myHealEffectPlayer);
                UpdateHPDisplayed(myPlayerSpritePosition.Up(), myPlayer);
            }
            
        }

        void PlaySound(SoundPlayer aSoundPlayer)
        {
            if (OperatingSystem.IsWindows())
            {
                aSoundPlayer.Play();
            }
        }

        void GetSelectionInput(Vector2[] someOptionPositions, int aMenuOptionsCount,ref int aSelectIndex, ref bool aSelectTypeBool)
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
                aSelectTypeBool = false;
                Utilities.Cursor(someOptionPositions[aSelectIndex]);
                Console.Write(" ");
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
                }
                else
                {
                    myPlayerCoolDownCounter = (myPlayer.myCoolDown - timeSinceLastAttack)/1000;
                    //Update cooldowntimer
                }
            }
            for (int i = 0; i < myEnemies.Count; i++)
            {
                if (myEnemies[i].myLastAttackTime == 0)
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
                                Attack(myEnemies[0], myPlayer);
                                PlaySound(myPlayerHitPlayer);
                                UpdateHPDisplayed(myPlayerSpritePosition.Up(), myPlayer);
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
                myEnemiesStillAlive = false;
                aTarget.ClearSprite(myEnemySpritePosition);
            }
        }

        void CheckAlive(Actor aTarget)
        {
            if (aTarget.myHP < 1)
            {
                myEnemiesStillAlive = false;
                aTarget.ClearSprite(myEnemySpritePosition);
            }
        }
    }
}
