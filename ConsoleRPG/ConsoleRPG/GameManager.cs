using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Media;


namespace ConsoleRPG
{
    internal class GameManager
    {
        const int myPlayerBaseHP = 50;

        bool myGameRunning = true;

        bool myBattleMode = false;

        bool myMansionAmbiencePlaying = false;

        bool myPortalPlaced = false;
        bool myPortalTrigger = false;
        bool myRoomPortalTrigger = false;
        SoundPlayer myMansionAmbiencePlayer;
        SoundPlayer myVillageAmbiencePlayer;

        Player myPlayer;
        TownPortal myPortal;
        //Door myTownPortal = new Door("", 0, 0, 14);


        Vector2 myDrawRoomOffSet = new Vector2(30, 9);
        Vector2 myPlayerPositionBeforeBattle = new Vector2();

        Dictionary<int, Door> myDoorsByID;
        Dictionary<int, Room> myRoomsByID;

        Dictionary<DoorDirections, bool> myHaveDoors;
        Dictionary<DoorDirections, bool> myDoorTriggerActivated;

        BattleManager myBattleManager;
        GameManager myGameManager;
        SpellFactory mySpellFactory;
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

            if (OperatingSystem.IsWindows())
            {
                myMansionAmbiencePlayer = new SoundPlayer(@"Audio\mansionAmbience.wav");
                myVillageAmbiencePlayer = new SoundPlayer(@"Audio\villageAmbience.wav");
            }
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
            if (!myMansionAmbiencePlaying && OperatingSystem.IsWindows())
            {
                myMansionAmbiencePlayer.PlayLooping();
                myMansionAmbiencePlaying = true;
            }
            if (myPlayer.myCurrentRoom == 0)
            {
                if (OperatingSystem.IsWindows())
                {
                    myVillageAmbiencePlayer.PlayLooping();
                    myMansionAmbiencePlaying = false;
                }
                
                anEntryPoint = DoorDirections.North;
            }
            ResetHasDoors();
            Console.Clear();
            myRoomsByID[myPlayer.myCurrentRoom].DrawRoom(myDrawRoomOffSet);
            SetHasDoors();
            myPlayer.myGameObject.DrawSprite(GetSpawnPointFromDoorDirection(anEntryPoint));
            if (myPortalPlaced && myPlayer.myCurrentRoom == 0)
            {
                //DrawPortalInTown

                myPortal.DrawPortalInTown();
            }
            if (myPortal.myOriginRoomID == myPlayer.myCurrentRoom && myPortal.myIsPlaced)
            {
                myPortal.PlacePortal(myPlayer.myCurrentRoom);
            }
        }

        void EnterRoom(Vector2 aPlayerSpawnPosition)
        {
            if (!myMansionAmbiencePlaying && OperatingSystem.IsWindows())
            {
                myMansionAmbiencePlayer.PlayLooping();
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
                    Utilities.CursorPosition();
                    Console.WriteLine("Press \'Enter\' to use door");
                }
                else if (myPortalPlaced && yPosition == 21 && (xPosition == 32 || xPosition == 33 || xPosition == 34 || xPosition == 35))
                {
                    myPortalTrigger = true;
                    Utilities.CursorPosition();
                    Console.WriteLine("Press \'Enter\' to use portal");
                }
                else //Reset trigger
                {
                    ResetDoorTriggers();
                    myPortalTrigger = false;
                    Utilities.CursorPosition();
                    Console.WriteLine("                           ");
                }
            }
            else
            {
                //Check west door trigger
                if (myHaveDoors[DoorDirections.West] && xPosition == 35 && (yPosition == 16 || yPosition == 17 || yPosition == 18))
                {
                    myDoorTriggerActivated[DoorDirections.West] = true;
                    Utilities.CursorPosition();
                    Console.WriteLine("Press \'Enter\' to use door");
                }//Check north door trigger
                else if (myHaveDoors[DoorDirections.North] && yPosition == 16 && (xPosition == 79 || xPosition == 80 || xPosition == 81 || xPosition == 82))
                {
                    myDoorTriggerActivated[DoorDirections.North] = true;
                    Utilities.CursorPosition();
                    Console.WriteLine("Press \'Enter\' to use door");
                }//Check east door trigger
                else if (myHaveDoors[DoorDirections.East] && xPosition == 127 && (yPosition == 16 || yPosition == 17 || yPosition == 18))
                {
                    myDoorTriggerActivated[DoorDirections.East] = true;
                    Utilities.CursorPosition();
                    Console.WriteLine("Press \'Enter\' to use door");
                }//Check south door trigger
                else if (myHaveDoors[DoorDirections.South] && yPosition == 26 && (xPosition == 79 || xPosition == 80 || xPosition == 81 || xPosition == 82))
                {
                    myDoorTriggerActivated[DoorDirections.South] = true;
                    Utilities.CursorPosition();
                    Console.WriteLine("Press \'Enter\' to use door");
                }
                else if (myPortalPlaced && yPosition == 16 && (xPosition == 108 || xPosition == 109 || xPosition == 110 || xPosition == 111))
                {
                    myRoomPortalTrigger = true;
                    Utilities.CursorPosition();
                    Console.WriteLine("Press \'Enter\' to use portal");
                }
                else //Reset trigger
                {
                    ResetDoorTriggers();
                    myRoomPortalTrigger = false;
                    Utilities.CursorPosition();
                    Console.WriteLine("                           ");
                }

            }
        }

        void TryEnterDoor()
        {
            int doorID;
            DoorDirections doorToEnter;

            if (GetTriggeredDoor(out doorID, out doorToEnter))
            {
                myPlayer.myCurrentRoom = myDoorsByID[doorID].UseDoor(myPlayer.myCurrentRoom);
                EnterRoom(doorToEnter);
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
            }
            else if (input.Key == ConsoleKey.DownArrow)
            {
                if (aGameObject.MyPosition.Down().Y < myRoomsByID[myPlayer.myCurrentRoom].mySouthBound + myDrawRoomOffSet.Y - aGameObject.MyHeight)
                    aGameObject.Move(aGameObject.MyPosition.Down());
            }
            else if (input.Key == ConsoleKey.LeftArrow)
            {
                if (aGameObject.MyPosition.Left().X > myRoomsByID[myPlayer.myCurrentRoom].myWestBound + myDrawRoomOffSet.X)
                    aGameObject.Move(aGameObject.MyPosition.Left());
            }
            else if (input.Key == ConsoleKey.Enter)
            {
                TryEnterDoor();
            }
            else if (input.Key == ConsoleKey.F1)//Debug
            {
                StartBattle();

            }
            else if (input.Key == ConsoleKey.F2)//Change key?
            {
                myPortal.PlacePortal(myPlayer.myCurrentRoom);
                myPortalPlaced = true;
            }
        }

        Vector2 GetSpawnPointFromDoorDirection(DoorDirections anEntryPoint)  //Refactor! Calculate from roomsize!
        {
            switch (anEntryPoint)
            {
                case DoorDirections.West:
                    return new Vector2(126, 17);                   
                case DoorDirections.North:
                    return new Vector2(81, 25);                    
                case DoorDirections.East:
                    return new Vector2(36, 17);                   
                case DoorDirections.South:
                    return new Vector2(81, 17);
                default:
                    return new Vector2(81, 25);
            }
        }

        /// <summary>
        /// Reads in a path of ASCII-art from txt-file and returns a GameObject with the ASCII as its Sprite
        /// </summary>
        /// <param name="aFilePath"></param>
        /// <returns></returns>
        GameObject ReadSpriteFromFile(string aFilePath)
        {
            string title;
            char[,] sprite = Utilities.ReadFromFile(aFilePath, out title);
            GameObject gameObject = new GameObject(new Vector2(sprite.GetLength(0), sprite.GetLength(1)), title, sprite);
            return gameObject;
        }

        void StartBattle()
        {
            Actor bat = new Actor(Actors.Bat, @"Sprites\Bat.txt", 15, 2, 5000);//DEbug
            //Actor dragon = new Actor(Actors.Dragon, @"Sprites\Dragon.txt", 15, 2, 7000);
            List<Actor> testBattleEnemies = new List<Actor>();//Debug
            testBattleEnemies.Add(bat); //Debug
            //testBattleEnemies.Add(dragon);
            myBattleMode = true;
            myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
            myMansionAmbiencePlaying = false;
            myBattleManager.StartBattle(testBattleEnemies, myPlayer, myGameManager);
        }

        public void EndBattle()
        {
            myBattleMode = false;
            EnterRoom(myPlayerPositionBeforeBattle);
        }
    }
}
