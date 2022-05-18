using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleRPG
{
    internal class GameManager
    {
        const int myPlayerBaseHP = 50;

        readonly int myWindowWidth;
        readonly int myWindowHeight;

        bool myGameRunning = true;

        bool myBattleMode = false;

        bool myDisplayingStats = false;

        bool myMansionAmbiencePlaying = false;
        bool myHasEnteredMansion = false;

        bool myPortalPlaced = false;
        int myPortalRoom;
        bool myPortalTrigger = false;
        bool myRoomPortalTrigger = false;

        bool myTavernDoorTrigger = false;
        bool myHealerDoorTrigger = false;

        bool myChestTrigger = false;
        bool myPickUpText = false;

        bool myGameStart = true;

        Player myPlayer;
        DisplayStats myDisplayStats;
        readonly TownPortal myPortal;

        Vector2 myDrawRoomOffSet = new Vector2();
        Vector2 myPlayerPositionBeforeBattle = new Vector2();

        Dictionary<int, Door> myDoorsByID;
        Dictionary<int, Room> myRoomsByID;
        readonly Dictionary<DoorDirections, bool> myHaveDoors;
        readonly Dictionary<DoorDirections, bool> myDoorTriggerActivated;

        readonly BattleManager myBattleManager;
        readonly GameManager myGameManager;
        readonly SpellFactory mySpellFactory;
        readonly ItemFactory myItemFactory;
        readonly EnemyFactory myEnemyFactory;        

        char[,] myDoorLockSprite;
        readonly char[,] myMansionSprite;

        Vector2 myTextFeedBackPosition = new Vector2();

        Vector2 myBaseRoomSize = new Vector2(105, 25);
        readonly Tavern myTavern;
        readonly FarmScene myFarmScene;
        readonly Healer myHealer;

        public GameManager()
        {
            myWindowWidth = Console.WindowWidth;
            myWindowHeight = Console.WindowHeight;
            myDoorsByID = new Dictionary<int, Door>();
            myRoomsByID = new Dictionary<int, Room>();
            myHaveDoors = new Dictionary<DoorDirections, bool>();
            myDoorTriggerActivated = new Dictionary<DoorDirections, bool>();
            myBattleManager = new BattleManager();
            myGameManager = this;
            mySpellFactory = new SpellFactory();
            myItemFactory = new ItemFactory();
            myEnemyFactory = new EnemyFactory();
            myPortal = new TownPortal();
            myTavern = new Tavern();
            myFarmScene = new FarmScene();
            myHealer = new Healer();
            myMansionSprite = Utilities.ReadFromFile(@"Sprites\Mansion.txt", out _);
            SoundManager.LoadSounds();

            Console.CursorVisible = false;
            myFarmScene.DrawScene();
            Load();
            EnterRoom(DoorDirections.North);
            Console.WriteLine("Move around with the arrow keys\nPlace portal back to Village with F1\nOpen Stats menu with TAB");

            while(myGameRunning)
            {
                if (!myBattleMode && !myDisplayingStats)
                {
                    GetInput(myPlayer.myGameObject);
                    CheckTrigger(myPlayer.myGameObject);
                }
            }
        }

        void Load()
        {
            SetVectors();
            RoomFactory roomFactory = new RoomFactory();
            roomFactory.CreateRooms();
            myRoomsByID = roomFactory.GetRooms();

            DoorFactory doorFactory = new DoorFactory();
            doorFactory.CreateDoors();
            myDoorsByID = doorFactory.GetDoors();
            
            myPlayer = new Player(ReadSpriteFromFile(@"Sprites\Man.txt"), myPlayerBaseHP);
            //myPlayerGold = 0;
            myDisplayStats = new DisplayStats();
            myPlayer.mySpellbook.AddSpell(mySpellFactory.GetSpell(SpellType.LightningBolt));
            myPlayer.mySpellbook.AddSpell(mySpellFactory.GetSpell(SpellType.Fireball));

            myHaveDoors.Add(DoorDirections.West, false);
            myHaveDoors.Add(DoorDirections.North, false);
            myHaveDoors.Add(DoorDirections.East, false);
            myHaveDoors.Add(DoorDirections.South, false);

            myDoorTriggerActivated.Add(DoorDirections.West, false);
            myDoorTriggerActivated.Add(DoorDirections.North, false);
            myDoorTriggerActivated.Add(DoorDirections.East, false);
            myDoorTriggerActivated.Add(DoorDirections.South, false);
            myDoorLockSprite = Utilities.ReadFromFile(@"Sprites\Lock.txt", out _);
            
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
            myRoomPortalTrigger = false;
            myChestTrigger = false;
            myTavernDoorTrigger = false;
            myPortalTrigger = false;
            myHealerDoorTrigger = false;
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

        void EnterRoom(DoorDirections anEntryPoint)
        {
            if (!myHasEnteredMansion && myPlayer.myCurrentRoom == 1)
            {
                myHasEnteredMansion = true;
                myTavern.myFirstKey = true;
            }
            if (!myMansionAmbiencePlaying)
            {
                SoundManager.PlaySound(SoundType.MansionAmbience, true);
                myMansionAmbiencePlaying = true;
            }
            if (myPlayer.myCurrentRoom == 0)
            {
                SoundManager.PlaySound(SoundType.VillageAmbience, true);
                myMansionAmbiencePlaying = false;
                
                anEntryPoint = DoorDirections.North;
                
            }
            ResetHasDoors();
            Console.Clear();
            myRoomsByID[myPlayer.myCurrentRoom].DrawRoom(myDrawRoomOffSet);
            if (myPlayer.myCurrentRoom == 0)
            {
                Utilities.DrawSprite(myMansionSprite, myDrawRoomOffSet.Right(myRoomsByID[myPlayer.myCurrentRoom].myRoomMap.GetLength(0)));
            }
            PlaceLocks();
            SetHasDoors();
            if (myGameStart)
            {
                myGameStart = false;
                myPlayer.myGameObject.DrawSprite(GetSpawnPointFromDoorDirection(DoorDirections.East).Down(6).Left(6));
                myPlayer.myGameObject.MoveToByX(myPlayer.myGameObject.MyPosition.Right(28).Up(3), 2);
                Thread.Sleep(500);
                myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
                myTavern.EnterTavern();
                EnterRoom(myPlayerPositionBeforeBattle);
            }
            else
            {
                myPlayer.myGameObject.DrawSprite(GetSpawnPointFromDoorDirection(anEntryPoint));
            }
            DisplayPortal();
        }

        void DisplayPortal()
        {
            if (myPortalPlaced && myPlayer.myCurrentRoom == 0)
            {
                myPortal.DrawPortalInTown(new Vector2(myDrawRoomOffSet.X + 1, myDrawRoomOffSet.Y + 6));
            }
            if (myPortal.myOriginRoomID == myPlayer.myCurrentRoom && myPortal.myIsPlaced)
            {
                myPortal.PlacePortal(myPlayer.myCurrentRoom, new Vector2(myDrawRoomOffSet.X + 77, myDrawRoomOffSet.Y + 2));
            }
        }

        void PlaceLocks()
        {
            if (CheckIfLockedDoors(out List<DoorDirections> doors))
            {
                foreach (DoorDirections door in doors)
                {
                    switch (door)
                    {
                        case DoorDirections.West:
                            Utilities.CursorPosition(myDrawRoomOffSet.X - 1, myDrawRoomOffSet.Y + 7);
                            Console.Write("    ");
                            Utilities.CursorPosition(myDrawRoomOffSet.X - 1, myDrawRoomOffSet.Y + 8);
                            Console.Write("    ");
                            Utilities.CursorPosition(myDrawRoomOffSet.X - 1, myDrawRoomOffSet.Y + 9);
                            Console.Write("     ");
                            DrawLock(myDrawRoomOffSet.X - 1, myDrawRoomOffSet.Y + 10);
                            break;
                        case DoorDirections.North:
                            DrawLock(myDrawRoomOffSet.X + 49, myDrawRoomOffSet.Y + 4);
                            break;
                        case DoorDirections.East:
                            Utilities.CursorPosition(myDrawRoomOffSet.X + 101, myDrawRoomOffSet.Y + 7);
                            Console.Write("    ");
                            Utilities.CursorPosition(myDrawRoomOffSet.X + 101, myDrawRoomOffSet.Y + 8);
                            Console.Write("    ");
                            Utilities.CursorPosition(myDrawRoomOffSet.X + 101, myDrawRoomOffSet.Y + 9);
                            Console.Write("    ");
                            DrawLock(myDrawRoomOffSet.X + 101, myDrawRoomOffSet.Y + 10);
                            break;
                        case DoorDirections.South:
                            DrawLock(myDrawRoomOffSet.X + 49, myDrawRoomOffSet.Y + 22);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void DrawLock(int anXOffSet, int aYOffSet)
        {
            Utilities.DrawSprite(myDoorLockSprite, anXOffSet, aYOffSet, ConsoleColor.DarkRed);
        }

        void EnterRoom(Vector2 aPlayerSpawnPosition)
        {
            if (!myMansionAmbiencePlaying && myPlayer.myCurrentRoom != 0)
            {
                SoundManager.PlaySound(SoundType.MansionAmbience, true);
                myMansionAmbiencePlaying = true;
            }
            ResetHasDoors();
            Console.Clear();
            myRoomsByID[myPlayer.myCurrentRoom].DrawRoom(myDrawRoomOffSet);
            PlaceLocks();
            SetHasDoors();
            myPlayer.myGameObject.DrawSprite(aPlayerSpawnPosition);
            DisplayPortal();
            if (myPlayer.myCurrentRoom == 0)
            {
                Utilities.DrawSprite(myMansionSprite, myDrawRoomOffSet.Right(myRoomsByID[myPlayer.myCurrentRoom].myRoomMap.GetLength(0)));
            }
        }

        void CheckTrigger(GameObject aGameObject)
        {
            int xPosition = aGameObject.MyPosition.X;
            int yPosition = aGameObject.MyPosition.Y;
            if (myPlayer.myCurrentRoom == 0)
            {
                if (xPosition == myDrawRoomOffSet.X + 97 && (yPosition == myDrawRoomOffSet.Y + 14 || yPosition == myDrawRoomOffSet.Y + 15 || yPosition == myDrawRoomOffSet.Y + 16 || yPosition == myDrawRoomOffSet.Y + 17))
                {
                    myDoorTriggerActivated[DoorDirections.East] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to enter mansion");
                }
                else if (myPortalPlaced && yPosition == myDrawRoomOffSet.Y + 11 && (xPosition == myDrawRoomOffSet.X + 2 || xPosition == myDrawRoomOffSet.X + 3 || xPosition == myDrawRoomOffSet.X + 4 || xPosition == myDrawRoomOffSet.X + 5))
                {
                    myPortalTrigger = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use portal");
                }
                else if (yPosition == myDrawRoomOffSet.Y + 11 && (xPosition == myDrawRoomOffSet.X + 27 || xPosition == myDrawRoomOffSet.X + 28 || xPosition == myDrawRoomOffSet.X + 29 || xPosition == myDrawRoomOffSet.X + 30))
                {
                    myTavernDoorTrigger = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to enter Tavern");
                }
                else if (yPosition == myDrawRoomOffSet.Y + 11 && (xPosition == myDrawRoomOffSet.X + 58 || xPosition == myDrawRoomOffSet.X + 59 || xPosition == myDrawRoomOffSet.X + 60 || xPosition == myDrawRoomOffSet.X + 61))
                {
                    myHealerDoorTrigger = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to enter Healer");

                }
                else //Reset trigger
                {
                    ResetDoorTriggers();                    
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("                              ");
                }
            }
            else
            {
                //Check west door trigger
                if (myHaveDoors[DoorDirections.West] && xPosition == myDrawRoomOffSet.X + 5 && (yPosition == myDrawRoomOffSet.Y + 8 || yPosition == myDrawRoomOffSet.Y + 9 || yPosition == myDrawRoomOffSet.Y + 10))
                {
                    myDoorTriggerActivated[DoorDirections.West] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use \'" + GetNextRoomName(DoorDirections.West) + "\' door");
                }//Check north door trigger
                else if (myHaveDoors[DoorDirections.North] && yPosition == myDrawRoomOffSet.Y + 7 && (xPosition == myDrawRoomOffSet.X + 48 || xPosition == myDrawRoomOffSet.X + 49 || xPosition == myDrawRoomOffSet.X + 50 || xPosition == myDrawRoomOffSet.X + 51 || xPosition == myDrawRoomOffSet.X + 52))
                {
                    myDoorTriggerActivated[DoorDirections.North] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use \'" + GetNextRoomName(DoorDirections.North) + "\' door");
                }//Check east door trigger
                else if (myHaveDoors[DoorDirections.East] && xPosition == myDrawRoomOffSet.X + 97 && (yPosition == myDrawRoomOffSet.Y + 8 || yPosition == myDrawRoomOffSet.Y + 9 || yPosition == myDrawRoomOffSet.Y + 10))
                {
                    myDoorTriggerActivated[DoorDirections.East] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use \'" + GetNextRoomName(DoorDirections.East) + "\' door");
                }//Check south door trigger
                else if (myHaveDoors[DoorDirections.South] && yPosition == myDrawRoomOffSet.Y + 18 && (xPosition == myDrawRoomOffSet.X + 49 || xPosition == myDrawRoomOffSet.X + 50 || xPosition == myDrawRoomOffSet.X + 51 || xPosition == myDrawRoomOffSet.X + 52))
                {
                    myDoorTriggerActivated[DoorDirections.South] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use \'" + GetNextRoomName(DoorDirections.South) + "\' door");
                }
                else if (myPortalPlaced && myPlayer.myCurrentRoom == myPortalRoom && yPosition == myDrawRoomOffSet.Y + 7 && (xPosition == myDrawRoomOffSet.X + 78 || xPosition == myDrawRoomOffSet.X + 79 || xPosition == myDrawRoomOffSet.X + 80 || xPosition == myDrawRoomOffSet.X + 81))
                {
                    myRoomPortalTrigger = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use portal");
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

        int GetKeyForDoor(int aDoorID)
        {
            int keyNeeded = myDoorsByID[aDoorID].myKeyID;

            if (myPlayer.myKeyIDs.ContainsKey(keyNeeded))
            {
                myPlayer.myKeyIDs.Remove(keyNeeded);
                return keyNeeded;
            }
            return 0;

        }

        void UseActionButton()
        {

            if (GetTriggeredDoor(out int doorID, out DoorDirections doorToEnter))
            {
                int keyToUse = GetKeyForDoor(doorID);
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
            if (myPortalTrigger)
            {
                myPlayer.myCurrentRoom = myPortal.myOriginRoomID;
                myPortalPlaced = false;
                myPortal.myIsPlaced = false;
                myPortalTrigger = false;
                EnterRoom(DoorDirections.North);
            }
            if (myRoomPortalTrigger)
            {
                myPlayer.myCurrentRoom = 0;
                myRoomPortalTrigger = false;
                EnterRoom(DoorDirections.North);
            }
            if (myChestTrigger)
            {
                myChestTrigger = false;
                Utilities.Cursor(myTextFeedBackPosition);
                Console.Write("                           ");
                bool hasItem = myItemFactory.GetItem(myRoomsByID[myPlayer.myCurrentRoom].myChest.OpenChest(), out Item item);
                if (myRoomsByID[myPlayer.myCurrentRoom].myChest.myKeyID != 0)
                {
                    myPlayer.myKeyIDs.Add(myRoomsByID[myPlayer.myCurrentRoom].myChest.myKeyID, KeyIDToText(myRoomsByID[myPlayer.myCurrentRoom].myChest.myKeyID));
                    if (myRoomsByID[myPlayer.myCurrentRoom].myChest.myKeyID == 1)
                    {
                        myTavern.myFirstKey = false;
                        myTavern.mySecondKey = true;
                    }
                }
                DisplayPickUpText(myRoomsByID[myPlayer.myCurrentRoom].myChest.myKeyID, hasItem ? item.myTitle : null);                
                myPlayer.PickUpItem(item.myStat);
                if (item.myItemID == 11)
                {
                    myPlayer.myCurrentHP = myPlayer.myBaseHP;
                }
                else if (item.myItemID == 12)
                {
                    myPlayer.myCurrentMP = myPlayer.myMaxMP;
                }
            }
            if (myTavernDoorTrigger)
            {
                myTavernDoorTrigger = false;
                myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
                myTavern.EnterTavern();
                EnterRoom(myPlayerPositionBeforeBattle);
            }
            if (myHealerDoorTrigger)
            {
                myHealerDoorTrigger = false;
                myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
                myPlayer.myCurrentHP = myPlayer.myBaseHP;
                myPlayer.myCurrentMP = myPlayer.myMaxMP;
                myHealer.EnterHealer();
                EnterRoom(myPlayerPositionBeforeBattle);
            }

        }

        string GetNextRoomName(DoorDirections aDirection)
        {
            int doorToCheck = myRoomsByID[myPlayer.myCurrentRoom].GetDoorFromDirection(aDirection);
            int nextRoomID = myDoorsByID[doorToCheck].GetNextRoomID(myPlayer.myCurrentRoom);
            string roomName = myRoomsByID[nextRoomID].myTitle;
            return roomName;
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

        bool CheckIfLockedDoors(out List<DoorDirections> someLockedDoorDirections)
        {
            bool haveLockedDoors = false;
            List<DoorDirections> doorDirections = new List<DoorDirections>();
            foreach (KeyValuePair<int, DoorDirections> door in myRoomsByID[myPlayer.myCurrentRoom].myDoorIDs)
            {
                if (myDoorsByID[door.Key].myLocked)
                {
                    haveLockedDoors = true;
                    doorDirections.Add(door.Value);
                }
            }
            someLockedDoorDirections = doorDirections;
            return haveLockedDoors;
        }

        /// <summary>
        /// Takes in a GameObject that then can be moved across the screen with arrows on the keyboard
        /// </summary>
        /// <param name="aGameObject"></param>
        void GetInput(GameObject aGameObject)
        {
            ConsoleKeyInfo input;

            while (Console.KeyAvailable)
                Console.ReadKey(true);

            input = Console.ReadKey(true);

            if (input.Key == ConsoleKey.Escape)
            {
                Utilities.CursorPosition();
                myGameRunning = false;
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
            else if (input.Key == ConsoleKey.F1 && myPlayer.myCurrentRoom != 0)
            {
                SoundManager.PlaySound(SoundType.PortalCast);
                Thread.Sleep(2000);
                myPortal.PlacePortal(myPlayer.myCurrentRoom, new Vector2(myDrawRoomOffSet.X + 77, myDrawRoomOffSet.Y + 2));
                SoundManager.PlaySound(SoundType.MansionAmbience, true);
                myPortalPlaced = true;
                myPortalRoom = myPlayer.myCurrentRoom;
            }           
            else if (input.Key == ConsoleKey.Tab)
            {
                myDisplayingStats = true;
                myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
                myDisplayStats.ShowStats(myPlayer);
                EnterRoom(myPlayerPositionBeforeBattle);
                myDisplayingStats = false;
            }
            else if (input.Key == ConsoleKey.F2)//Debug
            {
                StartBattle();
            }
            else if (input.Key == ConsoleKey.Spacebar)
            {
                
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

        /// <summary>
        /// Reads in a path of ASCII-art from txt-file and returns a GameObject with the ASCII as its Sprite
        /// </summary>
        /// <param name="aFilePath"></param>
        /// <returns></returns>
        static GameObject ReadSpriteFromFile(string aFilePath)
        {
            char[,] sprite = Utilities.ReadFromFile(aFilePath, out string title);
            GameObject gameObject = new GameObject(new Vector2(sprite.GetLength(0), sprite.GetLength(1)), title, sprite);
            return gameObject;
        }

        void StartBattle()
        {
            Actors enemyOne = Actors.NULL;
            Actors enemyTwo = Actors.NULL;
            Actors enemyThree = Actors.NULL;
            if (myPlayer.myCurrentRoom == 14)
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
                        enemyOne = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetDifficultyLevel());
                        break;
                    case 2:
                        enemyOne = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetDifficultyLevel());
                        enemyTwo = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetDifficultyLevel());
                        break;
                    case 3:
                        enemyOne = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetDifficultyLevel());
                        enemyTwo = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetDifficultyLevel());
                        enemyThree = Utilities.RandomEnemy(myRoomsByID[myPlayer.myCurrentRoom].GetDifficultyLevel());
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
                myBattleManager.StartBattle(battleList, myPlayer, myGameManager, myRoomsByID[myPlayer.myCurrentRoom].myRoomMap);
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

        public void EndBattle()
        {
            myBattleMode = false;
            if(myPlayer.myCurrentHP > 0)
            {
                myRoomsByID[myPlayer.myCurrentRoom].myRoomCleared = true;
                EnterRoom(myPlayerPositionBeforeBattle);
                if (myPlayer.myCurrentRoom == 14)
                {
                    Win();
                }
            }
            else
            {
                myPlayer.myCurrentRoom = 0;
                myPlayer.myCurrentHP = myPlayer.myBaseHP;
                myPlayer.myCurrentMP = myPlayer.myMaxMP;
                EnterRoom(DoorDirections.North);
            }
        }

        void DisplayPickUpText(int aKeyID, string anItemTitle)
        {
            myPickUpText = true;
            Utilities.CursorPosition(myDrawRoomOffSet.X + 52, myDrawRoomOffSet.Y - 3);
            if (aKeyID != 0)
            {
                Utilities.Color("You've picked up: " + KeyIDToText(aKeyID), ConsoleColor.DarkYellow);
                Utilities.CursorPosition(myDrawRoomOffSet.X + 52, myDrawRoomOffSet.Y - 2);
                if (anItemTitle != null)
                {
                    Utilities.Color("You've picked up: " + anItemTitle, ConsoleColor.Blue);
                }
            }
            else
            {
                if (anItemTitle != null)
                {
                    Utilities.Color("You've picked up: " + anItemTitle, ConsoleColor.Blue);
                }
            }
        }

        void HidePickUpText()
        {
            if (myPickUpText)
            {
                myPickUpText = false;
                Utilities.CursorPosition(myDrawRoomOffSet.X + 52, myDrawRoomOffSet.Y - 3);
                for (int i = 0; i < 34; i++)
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

        static string KeyIDToText(int aKeyID)
        {
            string keyName = aKeyID switch
            {
                1 => "Bedroom key",
                2 => "Diningroom key",
                3 => "Library key",
                4 => "Office key",
                _ => " ",
            };
            return keyName;
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
        
        void Win()
        {
            Vector2 position = new Vector2(myDrawRoomOffSet.X + myBaseRoomSize.X / 2, myDrawRoomOffSet.Y + myBaseRoomSize.Y / 2);
            Utilities.Cursor(position);
            Utilities.Color("You won!", ConsoleColor.DarkYellow);
            Utilities.Cursor(position.Down());
            if (myPlayer.myDeathCounter == 1)
            {               
                Utilities.Typewriter("You've died ONE time in the mansion.", 50, ConsoleColor.DarkRed);
            }
            else
            {
                Utilities.Typewriter($"You've died {myPlayer.myDeathCounter} times in the mansion.", 50, ConsoleColor.DarkRed);
            }
            myGameRunning = false;
            Utilities.ActionByInput(() => Console.Clear(), ConsoleKey.Enter);
        }
    }
}
