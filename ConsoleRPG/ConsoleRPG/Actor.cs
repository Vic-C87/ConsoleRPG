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

        public int myMaxHP;
        
        public int myDamage;

        public int myCoolDown;

        public int myBattleID;

        public long myLastAttackTime;

        public bool myIsPlayer;

        public bool myIsAlive;

        public char[,] mySprite;

        public Spellbook mySpellbook;


        public Actor(Actors aType, string aSpritePath, int someHP, int aDamage, int aCoolDown,bool aPlayerToggle = false)
        {
            myType = aType;
            myHP = someHP;
            myMaxHP = myHP;
            myDamage = aDamage;
            myIsPlayer = aPlayerToggle;
            myCoolDown = aCoolDown;
            myBattleID = 0;
            myLastAttackTime = 0L;
            myIsAlive = true;
            mySprite = Utilities.ReadFromFile(aSpritePath, out myName);
            mySpellbook = new Spellbook();
        }

        public Actor(Player aPlayer)
        {
            myType = Actors.Player;
            myName = aPlayer.myGameObject.MyTitle;
            myHP = aPlayer.myCurrentHP;
            myMaxHP = aPlayer.myBaseHP;
            myDamage = aPlayer.myBaseDamage;
            myIsPlayer = true;
            myCoolDown = aPlayer.myCoolDown;
            myBattleID = 0;
            myLastAttackTime = 0L;
            myIsAlive = true;
            mySprite = aPlayer.myGameObject.MySprite;
            mySpellbook = aPlayer.mySpellbook;
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

        public void DrawSprite(Vector2 anOffSet)
        {
            for (int y = 0; y < mySprite.GetLength(1); y++)
            {
                for (int x = 0; x < mySprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + anOffSet.X, y + anOffSet.Y, mySprite[x, y]);
                }
            }
        }

        public void ClearSprite(Vector2 anOffSet)
        {
            for (int y = 0; y < mySprite.GetLength(1); y++)
            {
                for (int x = 0; x < mySprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + anOffSet.X, y + anOffSet.Y, ' ');
                }
            }
        }
        
    }
}
