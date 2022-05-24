
namespace ConsoleRPG
{
    internal class Stat
    {
        public int myID;
        public int myHP;
        public int myMP;
        public int myDamage;
        public int myArmor;

        public Stat(int anID, int someHP, int someMP, int someDamage, int someArmor)
        {
            myID = anID;
            myHP = someHP; 
            myMP = someMP; 
            myDamage = someDamage; 
            myArmor = someArmor;
        }

        public Stat(Stat aStat)
        {
            myID = aStat.myID;
            myHP = aStat.myHP;
            myMP = aStat.myMP;
            myDamage = aStat.myDamage;
            myArmor = aStat.myArmor;
        }


        public void Add(Stat aStat)
        {
            myHP += aStat.myHP;
            myMP += aStat.myMP;
            myDamage += aStat.myDamage;
            myArmor += aStat.myArmor;
        }

        public void Remove(Stat aStat)
        {
            myHP -= aStat.myHP;
            myMP -= aStat.myMP;
            myDamage -= aStat.myDamage;
            myArmor -= aStat.myArmor;
        }
    }
}
