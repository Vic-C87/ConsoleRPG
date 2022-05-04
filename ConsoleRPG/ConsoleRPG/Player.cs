using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class Player
    {
        public int myCurrentRoom;
        public int myBaseHP;
        public int myCurrentHP;
        public int myBaseDamage;
        public int myCoolDown;

        public Actors myType;

        public GameObject myGameObject { get; }

        public Spellbook mySpellbook;

        public List<int> myKeyIDs;

        public Player(GameObject aGameObject, int aBaseHP)
        {
            myGameObject = aGameObject;
            myCurrentRoom = 0;
            myBaseHP = aBaseHP;
            myCurrentHP = myBaseHP;
            myBaseDamage = 3;
            myCoolDown = 3000;
            myType = Actors.Player;
            mySpellbook = new Spellbook();
            myKeyIDs = new List<int>();
        }
    }
}
