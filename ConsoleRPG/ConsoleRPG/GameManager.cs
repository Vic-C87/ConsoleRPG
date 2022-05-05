﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleRPG
{
    internal class GameManager
    {
        const int myPlayerBaseHP = 50;

        bool myGameRunning = true;

        bool myBattleMode = false;

        bool myMansionAmbiencePlaying = false;

        bool myPortalPlaced = false;
        int myPortalRoom;
        bool myPortalTrigger = false;
        bool myRoomPortalTrigger = false;

        bool myChestTrigger = false;
        bool myPickUpText = false;

        Player myPlayer;
        readonly TownPortal myPortal;

        Vector2 myDrawRoomOffSet = new Vector2(30, 9);
        Vector2 myPlayerPositionBeforeBattle = new Vector2();

        Dictionary<int, Door> myDoorsByID;
        Dictionary<int, Room> myRoomsByID;
        readonly Dictionary<DoorDirections, bool> myHaveDoors;
        readonly Dictionary<DoorDirections, bool> myDoorTriggerActivated;

        readonly BattleManager myBattleManager;
        readonly GameManager myGameManager;
        readonly SpellFactory mySpellFactory;

        char[,] myDoorLockSprite;

        Vector2 myTextFeedBackPosition = new Vector2(34, 5);

        public GameManager()
        {
            myDoorsByID = new Dictionary<int, Door>();
            myRoomsByID = new Dictionary<int, Room>();
            myHaveDoors = new Dictionary<DoorDirections, bool>();
            myDoorTriggerActivated = new Dictionary<DoorDirections, bool>();
            myBattleManager = new BattleManager();
            myGameManager = this;
            mySpellFactory = new SpellFactory();
            myPortal = new TownPortal();
            SoundManager.LoadSounds();

            Load();
            EnterRoom(DoorDirections.North);
            
            while(myGameRunning)
            {
                if (!myBattleMode)
                {
                    GetInput(myPlayer.myGameObject);
                    CheckTrigger(myPlayer.myGameObject);
                }
            }
        }

        void Load()
        {
            RoomFactory roomFactory = new RoomFactory();
            roomFactory.CreateRooms();
            myRoomsByID = roomFactory.GetRooms();

            DoorFactory doorFactory = new DoorFactory();
            doorFactory.CreateDoors();
            myDoorsByID = doorFactory.GetDoors();
            
            myPlayer = new Player(ReadSpriteFromFile(@"Sprites\Man.txt"), myPlayerBaseHP);
            myPlayer.mySpellbook.AddSpell(mySpellFactory.GetSpell(SpellType.LightningBolt));

            myHaveDoors.Add(DoorDirections.West, false);
            myHaveDoors.Add(DoorDirections.North, false);
            myHaveDoors.Add(DoorDirections.East, false);
            myHaveDoors.Add(DoorDirections.South, false);

            myDoorTriggerActivated.Add(DoorDirections.West, false);
            myDoorTriggerActivated.Add(DoorDirections.North, false);
            myDoorTriggerActivated.Add(DoorDirections.East, false);
            myDoorTriggerActivated.Add(DoorDirections.South, false);

            myDoorLockSprite = Utilities.ReadFromFile(@"Sprites\Lock.txt", out string lockName);
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
            List<DoorDirections> doors = new List<DoorDirections>();
            if (CheckIfLockedDoors(out doors))
            {
                foreach (DoorDirections door in doors)
                {
                    switch (door)
                    {
                        case DoorDirections.West:
                            Utilities.CursorPosition(29, 16);
                            Console.Write("    ");
                            Utilities.CursorPosition(29, 17);
                            Console.Write("    ");
                            Utilities.CursorPosition(29, 18);
                            Console.Write("     ");
                            DrawLock(29, 19);
                            break;
                        case DoorDirections.North:
                            DrawLock(79, 13);
                            break;
                        case DoorDirections.East:
                            Utilities.CursorPosition(131, 16);
                            Console.Write("    ");
                            Utilities.CursorPosition(131, 17);
                            Console.Write("    ");
                            Utilities.CursorPosition(131, 18);
                            Console.Write("    ");
                            DrawLock(131, 19);
                            break;
                        case DoorDirections.South:
                            DrawLock(79, 31);
                            break;
                        default:
                            break;
                    }
                }
            }
            SetHasDoors();
            myPlayer.myGameObject.DrawSprite(GetSpawnPointFromDoorDirection(anEntryPoint));
            if (myPortalPlaced && myPlayer.myCurrentRoom == 0)
            {
                myPortal.DrawPortalInTown();
            }
            if (myPortal.myOriginRoomID == myPlayer.myCurrentRoom && myPortal.myIsPlaced)
            {
                myPortal.PlacePortal(myPlayer.myCurrentRoom);
            }
        }

        void DrawLock(int anXOffSet, int aYOffSet)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            for (int y = 0; y < myDoorLockSprite.GetLength(1); y++)
            {
                for (int x = 0; x < myDoorLockSprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + anXOffSet, y + aYOffSet, myDoorLockSprite[x, y]);
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
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

        void CheckTrigger(GameObject aGameObject) //Refactor!!!!! Calculate from roomsize!
        {
            int xPosition = aGameObject.MyPosition.X;
            int yPosition = aGameObject.MyPosition.Y;
            if (myPlayer.myCurrentRoom == 0)
            {
                if (xPosition == 127 && (yPosition == 23 || yPosition == 24 || yPosition == 25 || yPosition == 26))
                {
                    myDoorTriggerActivated[DoorDirections.East] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use door");
                }
                else if (myPortalPlaced && yPosition == 20 && (xPosition == 32 || xPosition == 33 || xPosition == 34 || xPosition == 35))
                {
                    myPortalTrigger = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use portal");
                }
                else //Reset trigger
                {
                    ResetDoorTriggers();
                    myPortalTrigger = false;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("                           ");
                }
            }
            else
            {
                //Check west door trigger
                if (myHaveDoors[DoorDirections.West] && xPosition == 35 && (yPosition == 17 || yPosition == 18 || yPosition == 19))
                {
                    myDoorTriggerActivated[DoorDirections.West] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use door");
                }//Check north door trigger
                else if (myHaveDoors[DoorDirections.North] && yPosition == 16 && (xPosition == 78 || xPosition == 79 || xPosition == 80 || xPosition == 81 || xPosition == 82))
                {
                    myDoorTriggerActivated[DoorDirections.North] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use door");
                }//Check east door trigger
                else if (myHaveDoors[DoorDirections.East] && xPosition == 127 && (yPosition == 17 || yPosition == 18 || yPosition == 19))
                {
                    myDoorTriggerActivated[DoorDirections.East] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use door");
                }//Check south door trigger
                else if (myHaveDoors[DoorDirections.South] && yPosition == 27 && (xPosition == 79 || xPosition == 80 || xPosition == 81 || xPosition == 82))
                {
                    myDoorTriggerActivated[DoorDirections.South] = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use door");
                }
                else if (myPortalPlaced && myPlayer.myCurrentRoom == myPortalRoom && yPosition == 16 && (xPosition == 108 || xPosition == 109 || xPosition == 110 || xPosition == 111))
                {
                    myRoomPortalTrigger = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to use portal");
                }
                else if (CanOpenChest() && yPosition == 16 &&(xPosition == 93 || xPosition == 94 || xPosition == 95 || xPosition == 96))
                {
                    myChestTrigger = true;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.WriteLine("Press \'Enter\' to open chest");
                }
                else //Reset trigger
                {
                    ResetDoorTriggers();
                    myRoomPortalTrigger = false;
                    myChestTrigger = false;
                    Utilities.Cursor(myTextFeedBackPosition);
                    Console.Write("                           ");
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

            for (int i = 0; i < myPlayer.myKeyIDs.Count; i++)
            {
                if (keyNeeded == myPlayer.myKeyIDs[i])
                {
                    return myPlayer.myKeyIDs[i];
                }
            }
            return 0;
        }

        void TryEnterDoor()
        {
            int doorID;
            DoorDirections doorToEnter;

            if (GetTriggeredDoor(out doorID, out doorToEnter))
            {
                bool doorOpened;
                int keyToUse = GetKeyForDoor(doorID);
                myPlayer.myCurrentRoom = myDoorsByID[doorID].UseDoor(myPlayer.myCurrentRoom, out doorOpened, keyToUse);
                if (doorOpened)
                {
                    EnterRoom(doorToEnter);
                }
                else
                {
                    Utilities.Cursor(myTextFeedBackPosition.Down());
                    Console.Write("Door is ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Write("LOCKED");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(". You need to find the right ");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("KEY");
                    Console.ForegroundColor = ConsoleColor.White;
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
                List<Item> items = new List<Item>();
                myChestTrigger = false;
                Utilities.Cursor(myTextFeedBackPosition);
                Console.Write("                           ");
                items = myRoomsByID[myPlayer.myCurrentRoom].myChest.OpenChest();
                if(myRoomsByID[myPlayer.myCurrentRoom].myChest.myKeyID != 0)
                {
                    myPlayer.myKeyIDs.Add(myRoomsByID[myPlayer.myCurrentRoom].myChest.myKeyID);
                    DisplayPickUpText(myRoomsByID[myPlayer.myCurrentRoom].myChest.myKeyID);
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
            }
            else if (input.Key == ConsoleKey.RightArrow)
            {
                if (aGameObject.MyPosition.Right().X < myRoomsByID[myPlayer.myCurrentRoom].myEastBound + myDrawRoomOffSet.X - aGameObject.MyWidth)
                    aGameObject.Move(aGameObject.MyPosition.Right());
                HidePickUpText();
            }
            else if (input.Key == ConsoleKey.DownArrow)
            {
                if (aGameObject.MyPosition.Down().Y < myRoomsByID[myPlayer.myCurrentRoom].mySouthBound + myDrawRoomOffSet.Y - aGameObject.MyHeight)
                    aGameObject.Move(aGameObject.MyPosition.Down());
                HidePickUpText();
            }
            else if (input.Key == ConsoleKey.LeftArrow)
            {
                if (aGameObject.MyPosition.Left().X > myRoomsByID[myPlayer.myCurrentRoom].myWestBound + myDrawRoomOffSet.X)
                    aGameObject.Move(aGameObject.MyPosition.Left());
                HidePickUpText();
            }
            else if (input.Key == ConsoleKey.Enter)
            {
                TryEnterDoor();
            }
            else if (input.Key == ConsoleKey.F1 && myPlayer.myCurrentRoom != 0)//Change key?
            {
                SoundManager.PlaySound(SoundType.PortalCast);
                Thread.Sleep(2000);
                myPortal.PlacePortal(myPlayer.myCurrentRoom);
                SoundManager.PlaySound(SoundType.MansionAmbience, true);
                myPortalPlaced = true;
                myPortalRoom = myPlayer.myCurrentRoom;
            }
            else if (input.Key == ConsoleKey.F2)//Debug
            {
                StartBattle();
            }
        }

        static Vector2 GetSpawnPointFromDoorDirection(DoorDirections anEntryPoint)  //Refactor! Calculate from roomsize!
        {
            return anEntryPoint switch
            {
                DoorDirections.West => new Vector2(126, 17),
                DoorDirections.North => new Vector2(81, 25),
                DoorDirections.East => new Vector2(36, 17),
                DoorDirections.South => new Vector2(81, 17),
                _ => new Vector2(81, 25),
            };
        }

        /// <summary>
        /// Reads in a path of ASCII-art from txt-file and returns a GameObject with the ASCII as its Sprite
        /// </summary>
        /// <param name="aFilePath"></param>
        /// <returns></returns>
        static GameObject ReadSpriteFromFile(string aFilePath)
        {
            string title;
            char[,] sprite = Utilities.ReadFromFile(aFilePath, out title);
            GameObject gameObject = new GameObject(new Vector2(sprite.GetLength(0), sprite.GetLength(1)), title, sprite);
            return gameObject;
        }

        void StartBattle()
        {
            Actor bat = new Actor(Actors.Bat, @"Sprites\Bat.txt", 3, 2, 5000);//DEbug
            Actor dragon = new Actor(Actors.Dragon, @"Sprites\Dragon.txt", 3, 2, 7000);
            Actor bat2 = new Actor(Actors.Bat, @"Sprites\Bat.txt", 3, 2, 5000);
            List<Actor> testBattleEnemies = new List<Actor>();//Debug
            testBattleEnemies.Add(bat); //Debug
            testBattleEnemies.Add(dragon);
            testBattleEnemies.Add(bat2);
            myBattleMode = true;
            myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
            myMansionAmbiencePlaying = false;
            myBattleManager.StartBattle(testBattleEnemies, myPlayer, myGameManager, myRoomsByID[myPlayer.myCurrentRoom].myRoomMap);
        }

        public void EndBattle()
        {
            myBattleMode = false;
            EnterRoom(myPlayerPositionBeforeBattle);
        }

        void DisplayPickUpText(int aKeyID)//Change to itemID when inventory is implemented
        {
            myPickUpText = true;
            Utilities.CursorPosition(82, 6);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("You've picked up: " + KeyIDToText(aKeyID));
            Console.ForegroundColor = ConsoleColor.White;
        }

        void HidePickUpText()
        {
            if (myPickUpText)
            {
                myPickUpText = false;
                Utilities.CursorPosition(82, 6);
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
                1 => "West-wing key",
                2 => "East-wing key",
                3 => "Library key",
                4 => "Master suite key",
                _ => " ",
            };
            return keyName;
        }
    }
}
