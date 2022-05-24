
namespace ConsoleRPG
{
    internal class Spell
    {
        public string mySpellName;
        public SpellType mySpellType;
        public SoundType mySpellSound;
        public int myDamage;
        public int myManaCost;

        public Spell(string aSpellName, SpellType aSpellType, SoundType aSoundType, int someDamage, int aManaCost)
        {
            mySpellName = aSpellName;
            mySpellType = aSpellType;
            mySpellSound = aSoundType;
            myDamage = someDamage;
            myManaCost = aManaCost;
        }
    }
}
