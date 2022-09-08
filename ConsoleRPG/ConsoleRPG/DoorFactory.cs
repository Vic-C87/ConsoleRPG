using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class DoorFactory
    {
        readonly Dictionary<int, Door> myDoors;
		Dictionary<int, Door> myArcadeDoors;

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

		public void CreateArcadeDoors()
        {
			myArcadeDoors = new Dictionary<int, Door>();

			Door newDoor = new Door("",
					1, 2,
					1);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					1, 5,
					2);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					2, 3,
					3);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					2, 6,
					4);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					3, 4,
					5);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					3, 7,
					6);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					4, 8,
					7);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					5, 6,
					8);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					5, 9,
					9);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					6, 7,
					10);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					6, 10,
					11);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					7, 8,
					12);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					7, 11,
					13);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					8, 12,
					14);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					9, 10,
					15);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					9, 13,
					16);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					10, 11,
					17);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					10, 14,
					18);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					11, 12,
					19);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					11, 15,
					20);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					12, 16,
					21);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					13, 14,
					22);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					14, 15,
					23);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			newDoor = new Door("",
					15, 16,
					24);
			myArcadeDoors.Add(newDoor.myDoorID, newDoor);

			//Add door 25
		}

		public Dictionary<int, Door> GetArcadeDoors()
        {
			return myArcadeDoors;
        }

		public Dictionary<int, Door> GetDoors()
		{
			return myDoors;
		}
	}
}
