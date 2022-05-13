using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class EnemyFactory
    {
        Dictionary<Actors, Actor> myEnemies = new Dictionary<Actors, Actor>();


        public EnemyFactory()
        {
            Actor enemy = new Actor(Actors.Bat, @"Sprites\Bat.txt", 10, 3, 5000);
            enemy.AddHitSprite(@"Sprites\BatDown.txt");
            myEnemies.Add(enemy.myType, enemy);

            enemy = new Actor(Actors.Spider, @"Sprites\Spider.txt", 12, 5, 5000);
            enemy.AddHitSprite(@"Sprites\SpiderHit.txt");
            myEnemies.Add(enemy.myType, enemy);

            enemy = new Actor(Actors.Dragon, @"Sprites\Dragon.txt", 15, 10, 7000);
            enemy.AddHitSprite(@"Sprites\DragonHit.txt");
            myEnemies.Add(enemy.myType, enemy);

            enemy = new Actor(Actors.EliteDragon, @"Sprites\EliteDragon.txt", 20, 10, 7000);
            enemy.AddHitSprite(@"Sprites\EliteDragonHit.txt");
            myEnemies.Add(enemy.myType, enemy);

            enemy = new Actor(Actors.EvilLord, @"Sprites\Dracula.txt", 30, 20, 10000);
            //Add hit sprite
            myEnemies.Add(enemy.myType, enemy);
        }

        public Actor GetEnemy(Actors anEnemy)
        {
            Actor enemy = myEnemies.ContainsKey(anEnemy) ? new Actor(myEnemies[anEnemy]) : null;
            return enemy;
        }

        public List<Actor> GetBattleList(Actors aFirstEnemy, Actors aSecondEnemy = Actors.NULL, Actors aThirdEnemy = Actors.NULL)
        {
            List<Actor> enemyList = new List<Actor>();
            if (GetEnemy(aFirstEnemy) != null)
            {
                enemyList.Add(GetEnemy(aFirstEnemy));
            }
            
            if (aSecondEnemy != Actors.NULL && GetEnemy(aSecondEnemy) != null)
            {
                enemyList.Add(GetEnemy(aSecondEnemy));
            }

            if (aThirdEnemy != Actors.NULL && GetEnemy(aThirdEnemy) != null)
            {
                enemyList.Add(GetEnemy(aThirdEnemy));
            }

            return enemyList.Count > 0 ? enemyList : null;
        }

        public void AddNewEnemy(Actors anEnemyTypeForSprite, int someHP, int aDamage, int aCoolDown, bool aUseSameTypeTitle = true)
        {
            Actors type = aUseSameTypeTitle ? anEnemyTypeForSprite : Actors.Enemy;
            string path = GetEnemy(anEnemyTypeForSprite) != null ? GetEnemy(anEnemyTypeForSprite).mySpritePath : @"Sprites\Enemy.txt";
            Actor newEnemy = new Actor(type, path, someHP, aDamage, aCoolDown);
            if (type != Actors.Enemy)
            {
                newEnemy.myHitSprite = GetEnemy(anEnemyTypeForSprite).myHitSprite;
            }

            if (myEnemies.ContainsKey(Actors.Enemy))
            {
                myEnemies[Actors.Enemy] = newEnemy;
            }
            else
            {
                myEnemies.Add(Actors.Enemy, newEnemy);
            }
        }
    }
}
