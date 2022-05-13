using System;
using System.Threading;

namespace ConsoleRPG
{
    internal class Actor
    {
        public Actors myType;

        public string myName;

        public int myHP;

        public int myMaxHP;

        public int myMP = 0;

        public int myMaxMP = 0;
        
        public int myDamage;

        public int myArmor = 0;

        public int myCoolDown;

        public int myBattleID;

        public long myLastAttackTime;

        public bool myIsPlayer;

        public bool myIsAlive;

        public char[,] mySprite;

        public char[,] myHitSprite = null;

        public string mySpritePath;

        public Spellbook mySpellbook;

        public Vector2 myBattlePosition;

        public Vector2 myBattleNameOnScreenPosition;


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
            mySpritePath = aSpritePath;
            mySpellbook = new Spellbook();
            myBattlePosition = new Vector2();
            myBattleNameOnScreenPosition = new Vector2();
        }

        public Actor(Actor aCopy)
        {
            myType = aCopy.myType;
            myName = aCopy.myName;
            myHP = aCopy.myHP;
            myMaxHP = aCopy.myMaxHP;
            myDamage = aCopy.myDamage;
            myIsPlayer = aCopy.myIsPlayer;
            myCoolDown = aCopy.myCoolDown;
            myBattleID = aCopy.myBattleID;
            myLastAttackTime = aCopy.myLastAttackTime;
            myIsAlive = aCopy.myIsAlive;
            mySprite = aCopy.mySprite;
            myHitSprite = aCopy.myHitSprite;
            mySpritePath = aCopy.mySpritePath;
            mySpellbook = new Spellbook();
            myBattlePosition = new Vector2();
            myBattleNameOnScreenPosition = new Vector2();
        }

        public Actor(Player aPlayer)
        {
            myType = Actors.Player;
            myName = aPlayer.myGameObject.MyTitle;
            myHP = aPlayer.myCurrentHP;
            myMaxHP = aPlayer.myBaseHP;
            myDamage = aPlayer.myStat.myDamage;
            myIsPlayer = true;
            myCoolDown = aPlayer.myCoolDown;
            myBattleID = 0;
            myLastAttackTime = 0L;
            myIsAlive = true;
            mySprite = aPlayer.myGameObject.MySprite;
            mySpritePath = null;
            mySpellbook = aPlayer.mySpellbook;
            myBattlePosition = new Vector2();
            myBattleNameOnScreenPosition = new Vector2();
            myMaxMP = aPlayer.myStat.myMP;
            myMP = aPlayer.myCurrentMP;
            myArmor = aPlayer.myStat.myArmor;
        }

        public void AddHitSprite(string aPath)
        {
            myHitSprite = Utilities.ReadFromFile(aPath, out _);
        }

        public int Attack()
        {
            if (myIsPlayer)
            {
                SoundManager.PlaySound(SoundType.EnemyHurt);
            }
            else
            {
                SoundManager.PlaySound(SoundType.PlayerHurt, false, true);
            }
            return myDamage;
        }

        public void TakeDamage(int someDamage)
        {
            if (myIsPlayer)
            {

                myHP -= TryBlock(someDamage);
            }
            else
            {
                myHP -= someDamage;
                AnimateHurt();

            }
            if (myHP <= 0)
            {
                myHP = 0;
                Die();
            }
        }

        int TryBlock(int someDamage)
        {
            float damagePoints = (float)someDamage;
            float armor = (float)myArmor;
            if (armor >= damagePoints * .75f)
            {
                return someDamage / 2;
            }
            else
            {
                return someDamage;
            }
        }

        public void Heal(int aHealAmount)
        {
            SoundManager.PlaySound(SoundType.Heal, false, true);
            myHP += aHealAmount;
            if (myHP > myMaxHP)
            {
                myHP = myMaxHP;
            }
        }

        public void Die()
        {
            myIsAlive = false;
            ClearSprite(myBattlePosition);
            Utilities.Cursor(myBattleNameOnScreenPosition);
            if (!myIsPlayer)
            {
                Utilities.Color(myName, ConsoleColor.DarkRed);
            }
        }

        public void DrawSprite(Vector2 anOffSet)
        {
            Utilities.DrawSprite(mySprite, anOffSet);
            myBattlePosition = anOffSet;
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

        void AnimateHurt()
        {
            if (myHitSprite != null)
            {
                ClearSprite(myBattlePosition);
                Utilities.DrawSprite(myHitSprite, myBattlePosition);
                Thread.Sleep(300);
                ClearSprite(myBattlePosition);
                Utilities.DrawSprite(mySprite, myBattlePosition);
            }
        }
        
    }
}
