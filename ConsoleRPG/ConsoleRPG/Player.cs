
namespace ConsoleRPG
{
    internal class Player
    {
        public int myCurrentRoom;
        public int myBaseHP;
        public int myBaseDamage;
        public int myCoolDown;

        public Actors myType;

        public GameObject myGameObject { get; }

        public Player(GameObject aGameObject, int aBaseHP)
        {
            myGameObject = aGameObject;
            myCurrentRoom = 0;
            myBaseHP = aBaseHP;
            myBaseDamage = 10;
            myCoolDown = 3000;
            myType = Actors.Player;
        }
    }
}
