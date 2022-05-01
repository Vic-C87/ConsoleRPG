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
				if (Unlock(aKeyID)) //Add unlock check
				{
					myLocked = false;
					unlocked = true;
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
					unlocked = false;
					return aCurrentRoomID;
				}
			}
			else
			{
				unlocked = true;
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


		public bool Unlock(int aKeyID)
		{
			if (aKeyID == myKeyID)
			{
				myLocked = false;
				return true;
			}
			return false;
		}
	}
}
