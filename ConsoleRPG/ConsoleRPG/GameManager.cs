using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleRPG
{
    internal class GameManager
    {
        const char BLANK = ' ';

        const int myPlayerBaseHP = 50;


        char[,] myGameArray;
        char[,] myOverlayArray;

        bool myGameRunning = true;

        int myWindowWidth;
        int myWindowHeight;

        bool myBattleMode = false;

        Player myPlayer;
        Door myTownPortal = new Door("", 0, 0, 14);

        Vector2 myDrawRoomOffSet = new Vector2(30, 9);
        Vector2 myPlayerPositionBeforeBattle = new Vector2();

        Dictionary<int, Door> myDoorsByID;
        Dictionary<int, Room> myRoomsByID;

        Dictionary<DoorDirections, bool> myHaveDoors;
        Dictionary<DoorDirections, bool> myDoorTriggerActivated;

        BattleManager myBattleManager;
        GameManager myGameManager;
        public GameManager(int aWindowWidth, int aWindowHeight)
        {
            myWindowWidth = aWindowWidth;
            myWindowHeight = aWindowHeight;
            myDoorsByID = new Dictionary<int, Door>();
            myRoomsByID = new Dictionary<int, Room>();
            myHaveDoors = new Dictionary<DoorDirections, bool>();
            myDoorTriggerActivated = new Dictionary<DoorDirections, bool>();
            myBattleManager = new BattleManager();
            myGameManager = this;

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

            myHaveDoors.Add(DoorDirections.West, false);
            myHaveDoors.Add(DoorDirections.North, false);
            myHaveDoors.Add(DoorDirections.East, false);
            myHaveDoors.Add(DoorDirections.South, false);

            myDoorTriggerActivated.Add(DoorDirections.West, false);
            myDoorTriggerActivated.Add(DoorDirections.North, false);
            myDoorTriggerActivated.Add(DoorDirections.East, false);
            myDoorTriggerActivated.Add(DoorDirections.South, false);
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
            ResetHasDoors();
            Console.Clear();
            myRoomsByID[myPlayer.myCurrentRoom].DrawRoom(myDrawRoomOffSet);
            SetHasDoors();
            myPlayer.myGameObject.DrawSprite(GetSpawnPointFromDoorDirection(anEntryPoint));
        }

        void EnterRoom(Vector2 aPlayerSpawnPosition)
        {
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
            else //Reset trigger
            {
                ResetDoorTriggers();
                Utilities.CursorPosition();
                Console.WriteLine("                           ");
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
                if (aGameObject.MyPosition.Up().Y > 15)
                    aGameObject.Move(aGameObject.MyPosition.Up());
            }
            else if (input.Key == ConsoleKey.RightArrow)
            {
                if (aGameObject.MyPosition.Right().X < 131 - aGameObject.MyWidth)
                    aGameObject.Move(aGameObject.MyPosition.Right());
            }
            else if (input.Key == ConsoleKey.DownArrow)
            {
                if (aGameObject.MyPosition.Down().Y < 31 - aGameObject.MyHeight)
                    aGameObject.Move(aGameObject.MyPosition.Down());
            }
            else if (input.Key == ConsoleKey.LeftArrow)
            {
                if (aGameObject.MyPosition.Left().X > 34)
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
        /// Initiate 2D-Array and fill it with blank spaces as deafault
        /// </summary>
        /// <param name="aWidth"></param>
        /// <param name="aHeight"></param>
        /// <param name="aSymbol"></param>
        /// <returns></returns>
        char[,] SetArray(int aWidth, int aHeight, char aSymbol = BLANK)
        {
            char[,] anArray = new char[aWidth, aHeight];

            for (int y = 0; y < aHeight; y++)
            {
                for (int x = 0; x < aWidth; x++)
                {
                    anArray[x, y] = aSymbol;
                }
            }
            return anArray;
        }

        /// <summary>
        /// Initiate overlay 2D-Array for forexampel menues
        /// </summary>
        /// <param name="anOverlayArray"></param>
        /// <param name="anXOffSet"></param>
        /// <param name="aYOffSet"></param>
        void SetOverlay(char[,] anOverlayArray, int anXOffSet, int aYOffSet)
        {
            myOverlayArray = new char[myWindowWidth, myWindowHeight];
            Array.Copy(myGameArray, myOverlayArray, myGameArray.Length);

            for (int y = 0; y < anOverlayArray.GetLength(1); y++)
            {
                for (int x = 0; x < anOverlayArray.GetLength(0); x++)
                {
                    myOverlayArray[x + anXOffSet, y + aYOffSet] = anOverlayArray[x, y];
                }
            }
        }

        /// <summary>
        /// Draws a 2D-array to screen, starting at top left(0,0)
        /// </summary>
        /// <param name="anArray"></param>
        void DrawScreen(char[,] anArray)
        {
            Utilities.CursorPosition();
            for (int y = 0; y < anArray.GetLength(1); y++)
            {
                for (int x = 0; x < anArray.GetLength(0); x++)
                {
                    Utilities.Draw(x, y, anArray[x, y]);
                }
                Console.WriteLine("");
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
            Actor bat = new Actor(Actors.Bat, "Bat", 15, 2, 5000);
            Actor spider = new Actor(Actors.Spider, "Spider", 15, 2, 7000);
            List<Actor> testBattleEnemies = new List<Actor>();
            testBattleEnemies.Add(bat);
            //testBattleEnemies.Add(spider);
            myBattleMode = true;
            myPlayerPositionBeforeBattle = myPlayer.myGameObject.MyPosition;
            myBattleManager.StartBattle(testBattleEnemies, myPlayer, myGameManager);
        }

        public void EndBattle()
        {
            myBattleMode = false;
            EnterRoom(myPlayerPositionBeforeBattle);
        }
    }
}
