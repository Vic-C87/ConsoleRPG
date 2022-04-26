using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Actor
    {
        public Actors myType;

        public int myHP;
        public int myDamage;

        public int myCoolDown;

        public int myBattleID;

        public long myLastAttackTime;

        public bool myIsPlayer;


        public Actor(int someHP, int aDamage, int aCoolDown,bool aPlayerToggle = false)
        {
            myHP = someHP;
            myDamage = aDamage;
            myIsPlayer = aPlayerToggle;
            myCoolDown = aCoolDown;
            myBattleID = 0;
            myLastAttackTime = 0L;
        }

        public Actor(Player aPlayer)
        {
            myHP = aPlayer.myBaseHP;
            myDamage = aPlayer.myBaseDamage;
            myIsPlayer = true;
            myCoolDown = aPlayer.myCoolDown;
            myBattleID = 0;
            myLastAttackTime = 0L;
        }

        public void BattleAction()
        {

        }
        
    }
}
