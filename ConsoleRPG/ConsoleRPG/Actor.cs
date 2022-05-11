using System;

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
            mySpellbook = aPlayer.mySpellbook;
            myBattlePosition = new Vector2();
            myBattleNameOnScreenPosition = new Vector2();
            myMaxMP = aPlayer.myStat.myMP;
            myMP = aPlayer.myCurrentMP;
            myArmor = aPlayer.myStat.myArmor;
        }


        public int Attack()
        {
            if (myIsPlayer)
            {
                SoundManager.PlaySound(SoundType.EnemyHurt);
            }
            else
            {
                SoundManager.PlaySound(SoundType.PlayerHurt);
            }
            return myDamage;
        }

        public void TakeDamage(int someDamage)
        {
            if (myIsPlayer)
            {
                myHP -= (someDamage - myArmor);
            }
            else
            {
                myHP -= someDamage;
            }
            if (myHP <= 0)
            {
                myHP = 0;
                Die();
            }
        }

        public void Heal(int aHealAmount)
        {
            SoundManager.PlaySound(SoundType.Heal);
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
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(myName);
            Console.ForegroundColor = ConsoleColor.White;
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
        
    }
}
