using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class DoorFactory
    {
        readonly Dictionary<int, Door> myDoors;

		public DoorFactory()
		{
			myDoors = new Dictionary<int, Door>();
		}

		public void CreateDoors()
		{
			//first
			Door newDoor = new Door("",
					0, 1,
					0);
			myDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					1, 2,
					1);
			myDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					2, 3,
					2);
			myDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					2, 4,
					3);
			myDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					4, 5,
					4);
			myDoors.Add(newDoor.myDoorID, newDoor);


			newDoor = new Door("",
					5, 6,
					5,
					true,
					1);
			myDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					6, 7,
					6);
			myDoors.Add(newDoor.myDoorID, newDoor);


			newDoor = new Door("",
					2, 8,
					7,
					true,
					2);
			myDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					8, 9,
					8);
			myDoors.Add(newDoor.myDoorID, newDoor);


			newDoor = new Door("",
					9, 10,
					9);
			myDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					8, 11,
					10);
			myDoors.Add(newDoor.myDoorID, newDoor);


			newDoor = new Door("",
					8, 12,
					11,
					true,
					3);
			myDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					4, 13,
					12,
					true,
					4);
			myDoors.Add(newDoor.myDoorID, newDoor);


			newDoor = new Door("",
					13, 14,
					13);
			myDoors.Add(newDoor.myDoorID, newDoor);

			//Town Portal?
			/*newDoor = new Door("", 
					1, 2, 
					1);
			myDoors.Add(newDoor.myDoorID, newDoor);*/
		}

		public Dictionary<int, Door> GetDoors()
		{
			return myDoors;
		}
	}
}
