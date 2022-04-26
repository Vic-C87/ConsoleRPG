using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class BattleManager
    {
        const int NO_BATTLE_ID_FOUND = 99;

        List<int> myTurnListByBattleID;
        Actor myPlayer;
        List<Actor> myEnemies;

        bool myPlayerStillAlive;
        bool myEnemiesStillAlive;


        Stopwatch myStopWatch;

        int myPlayerCoolDownCounter;
        

        public BattleManager()
        {
            myTurnListByBattleID = new List<int>();
            myPlayer = null;
            myEnemies = new List<Actor>();
            myPlayerStillAlive = false;
            myEnemiesStillAlive = false;
            myStopWatch = new Stopwatch();
            myPlayerCoolDownCounter = myPlayer.myCoolDown;
        }

        public void StartBattle(List<Actor> anEnemyList, ref Player aPlayerReference)
        {
            myEnemies = anEnemyList;
            myPlayer = new Actor(aPlayerReference);
            myPlayerStillAlive = true;
            myStopWatch.Start();

            while (myPlayerStillAlive && myEnemiesStillAlive)
            {

                SetTurnList();
                Act();

            }
            myStopWatch.Reset();
            //End by modifying reference Player so the original gets modified!
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
                    myPlayerCoolDownCounter = myPlayer.myCoolDown - timeSinceLastAttack;
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
                    myPlayer.BattleAction();
                    myPlayer.myLastAttackTime = myStopWatch.ElapsedMilliseconds;
                    myTurnListByBattleID.RemoveAt(0);
                }
                else
                {
                    for (int i = 0; i < myEnemies.Count; i++)
                    {
                        if (myEnemies[i].myBattleID  == myTurnListByBattleID[0])
                        {
                            myEnemies[i].BattleAction();
                            myEnemies[i].myLastAttackTime = myStopWatch.ElapsedMilliseconds;
                            myTurnListByBattleID.RemoveAt(0);
                        }
                    }
                }
            }
        }        
    }
}
