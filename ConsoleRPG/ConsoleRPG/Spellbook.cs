using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Spellbook
    {
        List<Spell> mySpells;

        public Spellbook()
        {
            mySpells = new List<Spell>();
        }

        public void AddSpell(Spell aSpell)
        {
            mySpells.Add(aSpell);
        }

        public void UseSpell(int aSpellIndex, Actor aTarget)
        {
            aTarget.TakeDamage(mySpells[aSpellIndex].myDamage);
        }

        public void OpenSpellBook(Vector2 aScreenPositionOffset)
        {
            Vector2 printSpellPosition = aScreenPositionOffset;

            for (int i = 0; i < mySpells.Count; i++)
            {
                Utilities.Cursor(printSpellPosition);
                Console.Write(mySpells[i].mySpellName);
                printSpellPosition = printSpellPosition.Down();
            }

        }

        public void CloseSpellbook(Vector2 aScreenPositionOffset)
        {
            Vector2 printSpellPosition = aScreenPositionOffset;

            for (int i = 0; i < mySpells.Count; i++)
            {
                Utilities.Cursor(printSpellPosition);
                Console.Write("                   ");
                printSpellPosition = printSpellPosition.Down();
            }
        }

        public int GetSpellCount()
        {
            return mySpells.Count;
        }
    }
}
