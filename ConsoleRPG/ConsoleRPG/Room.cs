﻿using System;
using System.Collections.Generic;

namespace ConsoleRPG
{

	internal class Room
	{
		public string myTitle;
		public string myDescription;
		public int myRoomID;
		public bool myVisited;
		public Dictionary<int, DoorDirections> myDoorIDs;
		public List<int> myLootableIDs;
		public char[,] myRoomMap;

		public int myNorthBound;
		public int mySouthBound;
		public int myWestBound;
		public int myEastBound;
		public bool myPortalPlaced;

		public Chest myChest;
		Vector2 myChestPosition = new Vector2(92, 13);
		public bool myHaveChest;

		public Room()
		{
			myTitle = "";
			myDescription = "";
			myRoomID = -1;
			myVisited = false;
			myDoorIDs = new Dictionary<int, DoorDirections>();
			myLootableIDs = new List<int>();
			myRoomMap = new char[105, 25];
			myPortalPlaced = false;
			myChest = null;
			myHaveChest = false;
		}

		public Room(string aTitle, string aDescription, int aRoomID, bool aVisited,
				Dictionary<int, DoorDirections> someDoorIDs, List<int> someLootableIDs, string aRoomPath, bool aHaveChest = false,int aNorthBound = 6, int aSouthBound = 22, int aWestBound = 4, int anEastBound = 101)
		{
			myTitle = aTitle;
			myDescription = aDescription;
			myRoomID = aRoomID;
			myVisited = aVisited;
			myDoorIDs = someDoorIDs;
			myLootableIDs = someLootableIDs;
			myRoomMap = Utilities.ReadFromFile(aRoomPath, out string mapTitle);
			myHaveChest = aHaveChest;
			myNorthBound = aNorthBound;
			mySouthBound = aSouthBound;
			myWestBound = aWestBound;
			myEastBound = anEastBound;

			if (myHaveChest)
            {
				myChest = new Chest();
            }
		}

		public void DrawRoom(Vector2 anOffSet)
        {
			for (int y = 0; y < myRoomMap.GetLength(1); y++)
			{
				for (int x = 0; x < myRoomMap.GetLength(0); x++)
				{
					Utilities.Draw(x + anOffSet.X, y + anOffSet.Y, myRoomMap[x, y]);
				}
				Console.WriteLine();
			}
			
			if (myHaveChest && myChest != null)
            {
				if (!myChest.myIsOpened)
                {
					myChest.DrawChest(myChestPosition);
                }
            }
		}
	}

}
