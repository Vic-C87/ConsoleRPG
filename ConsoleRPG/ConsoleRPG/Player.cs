using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class Player
    {
        public int myCurrentRoom;
        public int myBaseHP;
        public int myCurrentHP;
        public int myMaxMP;
        public int myCurrentMP;
        public int myBaseDamage;
        public int myArmor;
        public int myCoolDown;

        public int myDeathCounter;

        public Stat myStat;

        public Actors myType;

        public GameObject myGameObject;

        public Spellbook mySpellbook;

        public Dictionary<int, string> myKeyIDs;

        public Player(GameObject aGameObject, int aBaseHP)
        {
            myGameObject = aGameObject;
            myCurrentRoom = 0;
            myStat = new Stat(0, aBaseHP, 5, 3, 1);
            myBaseHP = myStat.myHP;
            myCurrentHP = myBaseHP;
            myMaxMP = myStat.myMP;
            myCurrentMP = myMaxMP;
            myBaseDamage = myStat.myDamage;
            myArmor = myStat.myArmor;
            myCoolDown = 3000;
            myDeathCounter = 0;
            myType = Actors.Player;
            mySpellbook = new Spellbook();
            myKeyIDs = new Dictionary<int, string>();
        }

        public void PickUpItem(Stat aStat)
        {
            if (aStat == null) return;
            myStat.Add(aStat);
            AddStats(aStat);
        }

        void AddStats(Stat aStat)
        {
            myBaseHP += aStat.myHP;
            myMaxMP += aStat.myMP;
            myBaseDamage += aStat.myDamage;
            myArmor += aStat.myArmor;
        }
    }
}
