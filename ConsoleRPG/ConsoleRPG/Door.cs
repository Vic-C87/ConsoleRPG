using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Door
    {
		public string myDoorDescription;
		public int myRoomOneID;
		public int myRoomTwoID;
		public int myDoorID;
		public int myLockDifficulty;
		public bool myLocked;
		public bool myPickable;
		public bool myBreakable;

		public Door()
		{
			myDoorDescription = "";
			myRoomOneID = 0;
			myRoomTwoID = 0;
			myDoorID = 0;
			myLockDifficulty = 0;
			myLocked = false;
			myPickable = false;
			myBreakable = false;
		}

		public Door(string aDoorDescription, int aRoomOneID, int aRoomTwoID, int aDoorID, int aLockDifficulty = 0, bool isLocked = false, bool isPickable = false, bool isBreakable = false)
		{
			myDoorDescription = aDoorDescription;
			myRoomOneID = aRoomOneID;
			myRoomTwoID = aRoomTwoID;
			myDoorID = aDoorID;
			myLockDifficulty = aLockDifficulty;
			myLocked = isLocked;
			myPickable = isPickable;
			myBreakable = isBreakable;

		}

		public int UseDoor(int aCurrentRoomID, int aDexterity = 0)
		{
			if (myLocked)
			{
				if (Unlock(myLockDifficulty +1)) //Add unlock check
				{
					if (aCurrentRoomID == myRoomOneID)
					{
						return myRoomTwoID;
					}
					else
					{
						return myRoomOneID;
					}
				}
				else
				{
					return aCurrentRoomID;
				}
			}
			else
			{
				if (aCurrentRoomID == myRoomOneID)
				{
					return myRoomTwoID;
				}
				else
				{
					return myRoomOneID;
				}
			}
		}


		public bool Unlock(int aDifficultyCheck)
		{
			if (aDifficultyCheck > myLockDifficulty)
			{
				myLocked = false;
				return true;
			}
			return false;
		}
	}
}
