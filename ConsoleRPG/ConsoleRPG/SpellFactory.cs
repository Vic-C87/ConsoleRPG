using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class SpellFactory
    {
        List<Spell> mySpellList;

        public SpellFactory()
        {
            mySpellList = new List<Spell>();

            Spell newSpell;

            newSpell = new Spell("Lightning Bolt", SpellType.LightningBolt, 10);
            mySpellList.Add(newSpell);

            newSpell = new Spell("Fire Ball", SpellType.Fireball, 10);
            mySpellList.Add(newSpell);

            newSpell = new Spell("Ice Blast", SpellType.Iceblast, 10);
            mySpellList.Add((newSpell));
        }

        public Spell GetSpell(SpellType aSpellType)
        {
            for (int i = 0; i < mySpellList.Count; i++)
            {
                if (mySpellList[i].mySpellType == aSpellType)
                {
                    return mySpellList[i];
                }
            }
            return null;
        }
    }
}
