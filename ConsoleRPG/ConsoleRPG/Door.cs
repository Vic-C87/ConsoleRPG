
namespace ConsoleRPG
{
    internal class Door
    {
		public string myDoorDescription;
		public int myRoomOneID;
		public int myRoomTwoID;
		public int myDoorID;
		public bool myLocked;
		public int myKeyID;

		public Door()
		{
			myDoorDescription = "";
			myRoomOneID = 0;
			myRoomTwoID = 0;
			myDoorID = 0;
			myLocked = false;
			myKeyID = 0;

		}

		public Door(string aDoorDescription, int aRoomOneID, int aRoomTwoID, int aDoorID, bool isLocked = false, int aKeyID = 0)
		{
			myDoorDescription = aDoorDescription;
			myRoomOneID = aRoomOneID;
			myRoomTwoID = aRoomTwoID;
			myDoorID = aDoorID;			
			myLocked = isLocked;
			myKeyID = aKeyID;

		}

		public int UseDoor(int aCurrentRoomID, out bool unlocked, int aKeyID = 0)
		{
			if (myLocked)
			{
				if (Unlock(aKeyID))
				{
					myLocked = false;
					unlocked = true;
					return GetNextRoomID(aCurrentRoomID);
				}
				else
				{
					unlocked = false;
					return aCurrentRoomID;
				}
			}
			else
			{
				unlocked = true;
				return GetNextRoomID(aCurrentRoomID);
			}
		}


		public bool Unlock(int aKeyID)
		{
			if (aKeyID == myKeyID)
			{
				myLocked = false;
				return true;
			}
			return false;
		}

		public int GetNextRoomID(int aCurrentRoomID)
        {
			return (aCurrentRoomID == myRoomOneID) ? myRoomTwoID : myRoomOneID;
        }
	}
}
