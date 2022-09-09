using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleRPG
{
    internal class ArcadeManager
    {
        const int myPlayerBaseHP = 50;

        readonly int myWindowWidth;
        readonly int myWindowHeight;

        bool myArcadeModeIsRunning = true;
        bool myDisplayingStats = false;
        bool myBattleMode = false;
        bool myChestTrigger = false;
        bool myPickUpText = false;
        bool myMansionAmbiencePlaying = false;
        bool myEnteredNewHighScore = false;

        string myHighScoreName = "";

        Player myPlayer;
        PauseGameMenu myPauseMenu;
        DisplayStats myDisplayStats;
        RoomFactory myRoomFactory;

        readonly ArcadeManager myArcadeManager;
        readonly BattleManager myBattleManager;
        readonly SpellFactory mySpellFactory;
        readonly ItemFactory myItemFactory;
        readonly EnemyFactory myEnemyFactory;

        Vector2 myTextFeedBackPosition = new Vector2();
        Vector2 myBaseRoomSize = new Vector2(105, 25);
        Vector2 myDrawRoomOffSet = new Vector2();
        Vector2 myPlayerPositionBeforeBattle = new Vector2();

        readonly Dictionary<DoorDirections, bool> myHaveDoors;
        readonly Dictionary<DoorDirections, bool> myDoorTriggerActivated;

        Dictionary<int, Door> myDoorsByID;
        Dictionary<int, Room> myRoomsByID;

        public ArcadeManager(HighScore aHighScoreReference)
        {
            Console.Clear();
            myWindowWidth = Console.WindowWidth;
            myWindowHeight = Console.WindowHeight;
            myDoorsByID = new Dictionary<int, Door>();
            myRoomsByID = new Dictionary<int, Room>();
            myHaveDoors = new Dictionary<DoorDirections, bool>();
            myDoorTriggerActivated = new Dictionary<DoorDirections, bool>();
            myBattleManager = new BattleManager();
            mySpellFactory = new SpellFactory();
            myItemFactory = new ItemFactory();
            myEnemyFactory = new EnemyFactory();
            myPauseMenu = new PauseGameMenu();
            myArcadeManager = this;
            SoundManager.LoadSounds();
            SetVectors();
            
            myRoomFactory = new RoomFactory();
            myRoomFactory.CreateArcadeRooms(10);
            myRoomsByID = myRoomFactory.GetArcadeRooms();
            DoorFactory doorFactory = new DoorFactory();
            doorFactory.CreateArcadeDoors();
            myDoorsByID = doorFactory.GetArcadeDoors();

            myPlayer = new Player(ReadSpriteFromFile(@"Sprites/Man.txt"), myPlayerBaseHP);
            myPlayer.myArcadeMode = true;
            myDisplayStats = new DisplayStats();
            myPlayer.mySpellbook.AddSpell(mySpellFactory.GetSpell(SpellType.LightningBolt));

            myHaveDoors.Add(DoorDirections.West, false);
            myHaveDoors.Add(DoorDirections.North, false);
            myHaveDoors.Add(DoorDirections.East, false);
            myHaveDoors.Add(DoorDirections.South, false);

            myDoorTriggerActivated.Add(DoorDirections.West, false);
            myDoorTriggerActivated.Add(DoorDirections.North, false);
            myDoorTriggerActivated.Add(DoorDirections.East, false);
            myDoorTriggerActivated.Add(DoorDirections.South, false);

            myPlayer.myCurrentRoom = 1;

            EnterRoom(DoorDirections.North);
            DisplayPickUpText("Press \'TAB\' to check your stats", false);

            while (myArcadeModeIsRunning)
            {
                if (!myBattleMode && !myDisplayingStats)
                {
                    GetInput(myPlayer.myGameObject);
                    CheckTrigger(myPlayer.myGameObject);
                }
            }

            if (myEnteredNewHighScore)
            {
                aHighScoreReference.NewHighScore(myHighScoreName, myPlayer.myScore);
                aHighScoreReference.ShowHighScore();
            }

        }

        static GameObject ReadSpriteFromFile(string aFilePath)
        {
            char[,] sprite = Utilities.ReadFromFile(aFilePath, out string title);
            GameObject gameObject = new GameObject(new Vector2(sprite.GetLength(0), sprite.GetLength(1)), title, sprite);
            return gameObject;
        }

        void GetInput(GameObject aGameObject)
        {
            ConsoleKeyInfo input;

            while (Console.KeyAvailable)
                Console.ReadKey(true);

            input = Console.ReadKey(true);

            if (input.Key == ConsoleKey.Escape)
            {
                myDisplayingStats = true;
                myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
                myPauseMenu.PauseIt(out myArcadeModeIsRunning);
                if (myArcadeModeIsRunning)
                {
                    EnterRoom(myPlayerPositionBeforeBattle);
                    myDisplayingStats = false;
                }
                //Utilities.CursorPosition();

            }
            else if (input.Key == ConsoleKey.UpArrow)
            {
                if (aGameObject.MyPosition.Up().Y > myRoomsByID[myPlayer.myCurrentRoom].myNorthBound + myDrawRoomOffSet.Y)
                    aGameObject.Move(aGameObject.MyPosition.Up());
                CheckEncounter();
            }
            else if (input.Key == ConsoleKey.RightArrow)
            {
                if (aGameObject.MyPosition.Right().X < myRoomsByID[myPlayer.myCurrentRoom].myEastBound + myDrawRoomOffSet.X - aGameObject.MyWidth)
                    aGameObject.Move(aGameObject.MyPosition.Right());
                HidePickUpText();
                CheckEncounter();
            }
            else if (input.Key == ConsoleKey.DownArrow)
            {
                if (aGameObject.MyPosition.Down().Y < myRoomsByID[myPlayer.myCurrentRoom].mySouthBound + myDrawRoomOffSet.Y - aGameObject.MyHeight)
                    aGameObject.Move(aGameObject.MyPosition.Down());
                HidePickUpText();
                CheckEncounter();
            }
            else if (input.Key == ConsoleKey.LeftArrow)
            {
                if (aGameObject.MyPosition.Left().X > myRoomsByID[myPlayer.myCurrentRoom].myWestBound + myDrawRoomOffSet.X)
                    aGameObject.Move(aGameObject.MyPosition.Left());
                HidePickUpText();
                CheckEncounter();
            }
            else if (input.Key == ConsoleKey.Enter)
            {
                UseActionButton();
            }
            else if (input.Key == ConsoleKey.Tab)
            {
                myDisplayingStats = true;
                myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
                myDisplayStats.ShowStats(myPlayer);
                EnterRoom(myPlayerPositionBeforeBattle);
                myDisplayingStats = false;
            }
            else if (input.Key == ConsoleKey.Spacebar)
            {

            }
        }

        void EnterRoom(Vector2 aPlayerSpawnPosition)
        {
            if (!myMansionAmbiencePlaying)
            {
                SoundManager.PlaySound(SoundType.MansionAmbience, true);
                myMansionAmbiencePlaying = true;
            }
            ResetHasDoors();
            Console.Clear();
            myRoomsByID[myPlayer.myCurrentRoom].DrawRoom(myDrawRoomOffSet);
            SetHasDoors();
            myPlayer.myGameObject.DrawSprite(aPlayerSpawnPosition);
        }

        void EnterRoom(DoorDirections anEntryPoint)
        {
            if (!myMansionAmbiencePlaying)
            {
                SoundManager.PlaySound(SoundType.MansionAmbience, true);
                myMansionAmbiencePlaying = true;
            }
            ResetHasDoors();
            Console.Clear();
            myRoomsByID[myPlayer.myCurrentRoom].DrawRoom(myDrawRoomOffSet);
            SetHasDoors();
            myPlayer.myGameObject.DrawSprite(GetSpawnPointFromDoorDirection(anEntryPoint));
            Utilities.CursorPosition();
        }

        void ResetHasDoors()
        {
            for (int i = 0; i < myHaveDoors.Count; i++)
            {
                myHaveDoors[(DoorDirections)i] = false;
            }
        }

        void ResetDoorTriggers()
        {
            for (int i = 0; i < myDoorTriggerActivated.Count; i++)
            {
                myDoorTriggerActivated[(DoorDirections)i] = false;
            }
            myChestTrigger = false;
        }

        void SetHasDoors()
        {
            if (myRoomsByID[myPlayer.myCurrentRoom].myDoorIDs.ContainsValue(DoorDirections.West))
            {
                myHaveDoors[DoorDirections.West] = true;
            }
            if (myRoomsByID[myPlayer.myCurrentRoom].myDoorIDs.ContainsValue(DoorDirections.North))
            {
                myHaveDoors[DoorDirections.North] = true;
            }
            if (myRoomsByID[myPlayer.myCurrentRoom].myDoorIDs.ContainsValue(DoorDirections.East))
            {
                myHaveDoors[DoorDirections.East] = true;
            }
            if (myRoomsByID[myPlayer.myCurrentRoom].myDoorIDs.ContainsValue(DoorDirections.South))
            {
                myHaveDoors[DoorDirections.South] = true;
            }
        }

        Vector2 GetSpawnPointFromDoorDirection(DoorDirections anEntryPoint)
        {
            return anEntryPoint switch
            {
                DoorDirections.West => new Vector2(myDrawRoomOffSet.X + 96, myDrawRoomOffSet.Y + 8),
                DoorDirections.North => new Vector2(myDrawRoomOffSet.X + 51, myDrawRoomOffSet.Y + 16),
                DoorDirections.East => new Vector2(myDrawRoomOffSet.X + 6, myDrawRoomOffSet.Y + 8),
                DoorDirections.South => new Vector2(myDrawRoomOffSet.X + 51, myDrawRoomOffSet.Y + 8),
                _ => new Vector2(myDrawRoomOffSet.X + 51, myDrawRoomOffSet.Y + 16),
            };
        }

        void SetVectors()
        {
            //X pos
            int x = (myWindowWidth / 2) - (myBaseRoomSize.X / 2);
            //Y pos
            int y = (myWindowHeight / 2) - (myBaseRoomSize.Y / 2);
            myDrawRoomOffSet = new Vector2(x, y);

            myTextFeedBackPosition = new Vector2(myDrawRoomOffSet.X + 4, myDrawRoomOffSet.Y - 4);
        }

        void DisplayPickUpText(string anItemTitle, bool anIsanItem = true)
        {
            myPickUpText = true;
            Utilities.CursorPosition(myDrawRoomOffSet.X + 52, myDrawRoomOffSet.Y - 3);
            if (anIsanItem)
            {
                Utilities.Color("You've picked up: " + anItemTitle, ConsoleColor.Blue);
            }
            else
            {
                Utilities.Color(anItemTitle, ConsoleColor.DarkGreen);
            }

        }

        void HidePickUpText()
        {
            if (myPickUpText)
            {
                myPickUpText = false;
                Utilities.CursorPosition(myDrawRoomOffSet.X + 52, myDrawRoomOffSet.Y - 3);
                for (int i = 0; i < 54; i++)
                {
                    Console.Write(" ");
                }
                Utilities.CursorPosition(myDrawRoomOffSet.X + 52, myDrawRoomOffSet.Y - 2);
                for (int i = 0; i < 34; i++)
                {
                    Console.Write(" ");
                }
            }
        }

        void CheckEncounter()
        {
            if (!myRoomsByID[myPlayer.myCurrentRoom].myRoomCleared)
            {
                int random = Utilities.GetRandom(10);
                if (random == 1)
                {
                    StartBattle();
                }
            }
        }

        bool CanOpenChest()
        {
            bool canOpen = false;
            if (myRoomsByID[myPlayer.myCurrentRoom].myHaveChest && !myRoomsByID[myPlayer.myCurrentRoom].myChest.myIsOpened)
            {
                canOpen = true;
            }

            return canOpen;
        }

        void CheckTrigger(GameObject aGameObject)
        {
            int xPosition = aGameObject.MyPosition.X;
            int yPosition = aGameObject.MyPosition.Y;

            
            //Check west door trigger
            if (myHaveDoors[DoorDirections.West] && xPosition == myDrawRoomOffSet.X + 5 && (yPosition == myDrawRoomOffSet.Y + 8 || yPosition == myDrawRoomOffSet.Y + 9 || yPosition == myDrawRoomOffSet.Y + 10))
            {
                myDoorTriggerActivated[DoorDirections.West] = true;
                Utilities.Cursor(myTextFeedBackPosition);
                Console.WriteLine("Press \'Enter\' to use \'West\' door");
            }//Check north door trigger
            else if (myHaveDoors[DoorDirections.North] && yPosition == myDrawRoomOffSet.Y + 7 && (xPosition == myDrawRoomOffSet.X + 48 || xPosition == myDrawRoomOffSet.X + 49 || xPosition == myDrawRoomOffSet.X + 50 || xPosition == myDrawRoomOffSet.X + 51 || xPosition == myDrawRoomOffSet.X + 52))
            {
                myDoorTriggerActivated[DoorDirections.North] = true;
                Utilities.Cursor(myTextFeedBackPosition);
                Console.WriteLine("Press \'Enter\' to use \'North\' door");
            }//Check east door trigger
            else if (myHaveDoors[DoorDirections.East] && xPosition == myDrawRoomOffSet.X + 97 && (yPosition == myDrawRoomOffSet.Y + 8 || yPosition == myDrawRoomOffSet.Y + 9 || yPosition == myDrawRoomOffSet.Y + 10))
            {
                myDoorTriggerActivated[DoorDirections.East] = true;
                Utilities.Cursor(myTextFeedBackPosition);
                Console.WriteLine("Press \'Enter\' to use \'East\' door");
            }//Check south door trigger
            else if (myHaveDoors[DoorDirections.South] && yPosition == myDrawRoomOffSet.Y + 18 && (xPosition == myDrawRoomOffSet.X + 49 || xPosition == myDrawRoomOffSet.X + 50 || xPosition == myDrawRoomOffSet.X + 51 || xPosition == myDrawRoomOffSet.X + 52))
            {
                myDoorTriggerActivated[DoorDirections.South] = true;
                Utilities.Cursor(myTextFeedBackPosition);
                Console.WriteLine("Press \'Enter\' to use \'South\' door");
            }
            else if (CanOpenChest() && yPosition == myDrawRoomOffSet.Y + 7 && (xPosition == myDrawRoomOffSet.X + 63 || xPosition == myDrawRoomOffSet.X + 64 || xPosition == myDrawRoomOffSet.X + 65 || xPosition == myDrawRoomOffSet.X + 66))
            {
                myChestTrigger = true;
                Utilities.Cursor(myTextFeedBackPosition);
                Console.WriteLine("Press \'Enter\' to open chest");
            }
            else //Reset trigger
            {
                ResetDoorTriggers();
                Utilities.Cursor(myTextFeedBackPosition);
                Console.Write("                                               ");
                Utilities.Cursor(myTextFeedBackPosition.Down());
                Console.Write("                                               ");
            }           
        }

        void StartBattle()
        {
            Actors enemyOne = Actors.NULL;
            Actors enemyTwo = Actors.NULL;
            Actors enemyThree = Actors.NULL;
            if (myPlayer.myCurrentRoom == 16 && myPlayer.myCurrentLevel == 1)
            {
                enemyOne = Actors.EvilLord;
            }
            else if (myPlayer.myCurrentRoom == 16 && myPlayer.myCurrentLevel == 2)
            {
                enemyOne = Actors.EliteDragon;
                enemyTwo = Actors.EvilLord;
            }
            else if (myPlayer.myCurrentRoom == 16 && myPlayer.myCurrentLevel > 2)
            {
                enemyOne = Actors.EliteDragon;
                enemyTwo = Actors.EvilLord;
                enemyThree = Actors.EliteDragon;
            }
            else
            {
                int numberOfEnemies = myPlayer.myCurrentRoom == 1 ? 1 : Utilities.GetRandom(3);

                switch (numberOfEnemies)
                {
                    case 1:
                        enemyOne = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetArcadeDifficultyLevel());
                        break;
                    case 2:
                        enemyOne = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetArcadeDifficultyLevel());
                        enemyTwo = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetArcadeDifficultyLevel());
                        break;
                    case 3:
                        enemyOne = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetArcadeDifficultyLevel());
                        enemyTwo = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetArcadeDifficultyLevel());
                        enemyThree = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetArcadeDifficultyLevel());
                        break;
                    default:
                        break;
                }
            }

            List<Actor> battleList = myEnemyFactory.GetBattleList(enemyOne, enemyTwo, enemyThree);
            if (battleList != null)
            {
                myBattleMode = true;
                myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
                myMansionAmbiencePlaying = false;
                myBattleManager.StartBattle(battleList, myPlayer, myArcadeManager, myRoomsByID[myPlayer.myCurrentRoom].myRoomMap);
            }
        }

        void UseActionButton()
        {
            if (GetTriggeredDoor(out int doorID, out DoorDirections doorToEnter))
            {
                int keyToUse = 0;
                myPlayer.myCurrentRoom = myDoorsByID[doorID].UseDoor(myPlayer.myCurrentRoom, out bool doorOpened, keyToUse);
                if (doorOpened)
                {
                    EnterRoom(doorToEnter);
                }
                else
                {
                    Utilities.Cursor(myTextFeedBackPosition.Down());
                    Console.Write("Door is ");
                    Utilities.Color("LOCKED", ConsoleColor.DarkRed);
                    Console.Write(". You need to find the right ");
                    Utilities.Color("KEY", ConsoleColor.DarkYellow);
                }
            }

            if (myChestTrigger)
            {
                myChestTrigger = false;
                Utilities.Cursor(myTextFeedBackPosition);
                Console.Write("                           ");
                bool hasItem = myItemFactory.GetItem(myRoomsByID[myPlayer.myCurrentRoom].myChest.OpenChest(), out Item item);
                DisplayPickUpText(hasItem ? item.myTitle : null);
                myPlayer.PickUpItem(item.myStat);
                if (item.myItemID == 11)
                {
                    myPlayer.myCurrentHP = myPlayer.myBaseHP;
                }
                else if (item.myItemID == 12)
                {
                    myPlayer.myCurrentMP = myPlayer.myMaxMP;
                }
                else if (item.myItemID == 15)
                {
                    myPlayer.myCurrentHP = myPlayer.myBaseHP;
                    myPlayer.myCurrentMP = myPlayer.myMaxMP;
                }
            }
        }

        bool GetTriggeredDoor(out int aDoorID, out DoorDirections aDoorDirection)
        {
            aDoorID = myPlayer.myCurrentRoom;
            aDoorDirection = DoorDirections.South;
            foreach (KeyValuePair<DoorDirections, bool> trigger in myDoorTriggerActivated)
            {
                if (trigger.Value)
                {
                    aDoorID = myRoomsByID[myPlayer.myCurrentRoom].myDoorIDs.FirstOrDefault(x => x.Value == trigger.Key).Key;
                    aDoorDirection = trigger.Key;
                    return true;
                }
            }
            return false;
        }

        public void EndBattle()
        {
            myBattleMode = false;
            if (myPlayer.myCurrentHP > 0)
            {
                myRoomsByID[myPlayer.myCurrentRoom].myRoomCleared = true;
                EnterRoom(myPlayerPositionBeforeBattle);
                if ((myPlayer.myCurrentRoom == 15 || myPlayer.myCurrentRoom == 12) && myPlayer.myCurrentLevel < 3)
                {
                    myPlayer.FullHeal();
                    DisplayPickUpText("You feel rejuvenated, your health and mana is restored", false);
                }
                //Respawn level if last room in level(room 16)
                if (myPlayer.myCurrentRoom == 16)
                {
                    int randomLootValue = Utilities.Clamp(10 - myPlayer.myCurrentLevel, 1, 10);
                    myRoomsByID = myRoomFactory.RespawnRooms(randomLootValue);
                    myPlayer.myCurrentRoom = 1;
                    myPlayer.myCurrentLevel++;
                    myEnemyFactory.LevelUpEnemies();
                    myPlayer.FullHeal();
                    EnterRoom(DoorDirections.North);
                }
            }
            else
            {
                Console.Clear();
                //Loosing sprite
                myHighScoreName = Utilities.EnterName(new Vector2(myWindowWidth/3, myWindowHeight/3));
                myEnteredNewHighScore = true;
                myArcadeModeIsRunning = false;
            }
        }
    }
}
