using System;
using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class Spellbook
    {
        readonly List<Spell> mySpells;

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
            SoundManager.PlaySound(mySpells[aSpellIndex].mySpellSound);
            aTarget.TakeDamage(mySpells[aSpellIndex].myDamage);
        }

        public int GetSpellCost(int aSpellIndex)
        {
            return mySpells[aSpellIndex].myManaCost;
        }

        public void OpenSpellBook(Vector2 aScreenPositionOffset)
        {
            Vector2 printSpellPosition = aScreenPositionOffset;

            for (int i = 0; i < mySpells.Count; i++)
            {
                Utilities.Cursor(printSpellPosition);
                Console.Write(mySpells[i].mySpellName + "(" + mySpells[i].myManaCost + "MP)");
                printSpellPosition = printSpellPosition.Down();
            }

        }

        public void CloseSpellbook(Vector2 aScreenPositionOffset)
        {
            Vector2 printSpellPosition = aScreenPositionOffset;

            for (int i = 0; i < mySpells.Count; i++)
            {
                Utilities.Cursor(printSpellPosition);
                Console.Write("                    ");
                printSpellPosition = printSpellPosition.Down();
            }
        }

        public int GetSpellCount()
        {
            return mySpells.Count;
        }
    }
}
