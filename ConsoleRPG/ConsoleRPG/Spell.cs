using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Spell
    {
        public string mySpellName;
        public SpellType mySpellType;
        //public int mySpellID;
        public int myDamage;
        //public char[,] myCastSprite;

        public Spell(string aSpellName, SpellType aSpellType, int someDamage)
        {
            mySpellName = aSpellName;
            mySpellType = aSpellType;
            myDamage = someDamage;
        }
    }
}
