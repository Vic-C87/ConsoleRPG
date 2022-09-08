using System;
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
		public bool myHaveChest;
		public bool myRoomCleared;

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
			myRoomCleared = false;
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
			myRoomMap = Utilities.ReadFromFile(aRoomPath, out myTitle);
			myHaveChest = aHaveChest;
			myNorthBound = aNorthBound;
			mySouthBound = aSouthBound;
			myWestBound = aWestBound;
			myEastBound = anEastBound;

			if (myHaveChest)
            {
				myChest = new Chest();
            }
			myRoomCleared = aRoomID == 0;
		}

		public void AddKeyToChest(int aKeyID)
        {
			myChest.AddKey(aKeyID);
        }

		public void RandomLoot(int aRandomizerValue)
        {
			aRandomizerValue = Utilities.Clamp(aRandomizerValue, 1, 10);
			if (Utilities.GetRandom(10) <= aRandomizerValue)
            {
				myHaveChest = true;
				myChest = new Chest();
				AddItemToChest((UpgradeType)Utilities.GetRandom(5));
            }
        }

		public void AddItemToChest(UpgradeType aType)
        {
			myChest.AddItem(aType);
        }

		public void DrawRoom(Vector2 anOffSet)
        {
			Utilities.DrawSprite(myRoomMap, anOffSet);
			
			if (myHaveChest && myChest != null)
            {
				if (!myChest.myIsOpened)
                {
					myChest.DrawChest(new Vector2(anOffSet.X + 62, anOffSet.Y + 4));
                }
            }
		}

		public int GetDoorFromDirection(DoorDirections aDirection)
        {
			int doorID = 0;
            foreach (KeyValuePair<int, DoorDirections> door in myDoorIDs)
            {
				if (door.Value == aDirection)
                {
					doorID = door.Key;
                }
            }
			return doorID;
        }

		public int GetDifficultyLevel()
        {
			int difficulty;

			if (myRoomID < 4)
            {
				difficulty = 1;
            }
			else if (myRoomID > 4 && myRoomID < 10)
            {
				difficulty = 2;
            }
			else
            {
				difficulty = 3;
            }

			return difficulty;
        }

		public int GetArcadeDifficultyLevel(int aLevelMultiplier = 1)
        {
			int difficulty;

			if (myRoomID < 6)
			{
				difficulty = 1 * aLevelMultiplier;
			}
			else if (myRoomID > 5 && myRoomID < 11)
			{
				difficulty = 2 * aLevelMultiplier;
			}
			else
			{
				difficulty = 3 * aLevelMultiplier;
			}

			return difficulty;
        }
	}

}
