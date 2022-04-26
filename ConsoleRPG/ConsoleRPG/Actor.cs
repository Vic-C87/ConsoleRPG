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

        public string myName;

        public int myHP;

        int myMaxHP;
        
        public int myDamage;

        public int myCoolDown;

        public int myBattleID;

        public long myLastAttackTime;

        public bool myIsPlayer;

        public bool myIsAlive;


        public Actor(Actors aType, string aName, int someHP, int aDamage, int aCoolDown,bool aPlayerToggle = false)
        {
            myType = aType;
            myName = aName;
            myHP = someHP;
            myMaxHP = myHP;
            myDamage = aDamage;
            myIsPlayer = aPlayerToggle;
            myCoolDown = aCoolDown;
            myBattleID = 0;
            myLastAttackTime = 0L;
            myIsAlive = true;
        }

        public Actor(Player aPlayer)
        {
            myType = Actors.Player;
            myName = "Player";
            myHP = aPlayer.myBaseHP;
            myMaxHP = myHP;
            myDamage = aPlayer.myBaseDamage;
            myIsPlayer = true;
            myCoolDown = aPlayer.myCoolDown;
            myBattleID = 0;
            myLastAttackTime = 0L;
            myIsAlive = true;
        }

        public void BattleAction()
        {

        }

        public int Attack()
        {
            return myDamage;
        }

        public void TakeDamage(int someDamage)
        {
            myHP -= someDamage;
            if (myHP <= 0)
            {
                myHP = 0;
                Die();
            }
        }

        public void Heal(int aHealAmount)
        {
            myHP += aHealAmount;
            if (myHP > myMaxHP)
            {
                myHP = myMaxHP;
            }
        }

        void Die()
        {
            myIsAlive = false;
        }
        
    }
}
