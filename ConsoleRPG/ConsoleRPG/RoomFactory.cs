using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class RoomFactory
    {
		readonly Dictionary<int, Room> myRooms;

		public RoomFactory()
		{
			myRooms = new Dictionary<int, Room>();
		}

		public void CreateRooms()
		{
			string rootRoomPath = @"Sprites/Rooms/";
			//first
			Room newRoom = new Room("Village", "a small rustic village",
					0, false,
					new Dictionary<int, DoorDirections>() { { 0, DoorDirections.East } },
					new List<int>(),
					rootRoomPath + "Village.txt",
					false,
					10, 25, 0);
			myRooms.Add(newRoom.myRoomID, newRoom);
			//second
			newRoom = new Room("Foyer", "the first room of the mansion",
					1, false,
					new Dictionary<int, DoorDirections>() { { 0, DoorDirections.West }, { 1, DoorDirections.North } },
					new List<int>(),
					rootRoomPath + "Foyer.txt",
					true);
			newRoom.AddItemToChest(UpgradeType.SwordUpgrade);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Greathall", "a large hall with high ceilling",
					2, false,
					new Dictionary<int, DoorDirections>() { { 1, DoorDirections.South }, { 2, DoorDirections.North }, { 3, DoorDirections.West }, { 7, DoorDirections.East } },
					new List<int>(),
					rootRoomPath + "GreatHall.txt");
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Cloakroom", "a small cloakroom",
					3, false,
					new Dictionary<int, DoorDirections>() { { 2, DoorDirections.South } },
					new List<int>(),
					rootRoomPath + "CloakRoom.txt",
					true);//Key to door 5
			newRoom.AddKeyToChest(1);
			newRoom.AddItemToChest(UpgradeType.HealthUpgrade);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Main Hallway", "a hall",
					4, false,
					new Dictionary<int, DoorDirections>() { { 3, DoorDirections.East }, { 4, DoorDirections.West }, { 12, DoorDirections.North } },
						   new List<int>(),
						   rootRoomPath + "HallwayA.txt");
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Hallway", "a hallway",
					5, false,
					new Dictionary<int, DoorDirections>() { { 4, DoorDirections.East }, { 5, DoorDirections.West } },
						   new List<int>(),
						   rootRoomPath + "HallwayB.txt"); //Loot
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Bedroom", "a grand bedroom",
					6, false,
					new Dictionary<int, DoorDirections>() { { 5, DoorDirections.East }, { 6, DoorDirections.South } },
						   new List<int>(),
						   rootRoomPath + "Bedroom.txt",
						   true);//Loot and Key to door 7
			newRoom.AddKeyToChest(2);
			newRoom.AddItemToChest(UpgradeType.ArmorUpgrade);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Bathroom", "a small bathroom",
					7, false,
					new Dictionary<int, DoorDirections>() { { 6, DoorDirections.North } },
						   new List<int>(),
						   rootRoomPath + "Bathroom.txt",
						   true);
			newRoom.AddItemToChest(UpgradeType.ManaUpgrade);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Diningroom", "a big diningroom",
					8, false,
					new Dictionary<int, DoorDirections>() { { 7, DoorDirections.West }, { 8, DoorDirections.South }, { 10, DoorDirections.North }, { 11, DoorDirections.East } },
						   new List<int>(),
						   rootRoomPath + "DiningHall.txt");//Loot
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Washroom", "an old and small washroom",
					9, false,
					new Dictionary<int, DoorDirections>() { { 8, DoorDirections.North }, { 9, DoorDirections.East } },
						   new List<int>(),
						   rootRoomPath + "Washroom.txt");
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Storageroom", "a small dusty storeroom",
					10, false,
					new Dictionary<int, DoorDirections>() { { 9, DoorDirections.West } },
						   new List<int>(),
						   rootRoomPath + "Storageroom.txt",
						   true);//Loot
			newRoom.AddItemToChest(UpgradeType.SwordUpgrade);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Kitchen", "a big kitchen...",
					11, false,
					new Dictionary<int, DoorDirections>() { { 10, DoorDirections.South } },
						   new List<int>(),
						   rootRoomPath + "Kitchen.txt",
						   true);//Loot and Key to door 11
			newRoom.AddKeyToChest(3);
			newRoom.AddItemToChest(UpgradeType.ArmorUpgrade);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Library", "a huge library filled with tall bookshelfs",
					12, false,
					new Dictionary<int, DoorDirections>() { { 11, DoorDirections.West } },
						   new List<int>(),
						   rootRoomPath + "Library.txt",
						   true);//Loot and Key to door 12
			newRoom.AddKeyToChest(4);
			newRoom.AddItemToChest(UpgradeType.HealthUpgrade);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Office", "a large study with a big desk at the far end of the room",
					13, false,
					new Dictionary<int, DoorDirections>() { { 12, DoorDirections.South }, { 13, DoorDirections.East } },
						   new List<int>(),
						   rootRoomPath + "Office.txt",
						   true);//Loot
			newRoom.AddItemToChest(UpgradeType.ManaUpgrade);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Master suit", "the master suite",
					14, false,
					new Dictionary<int, DoorDirections>() { { 13, DoorDirections.West } },
						   new List<int>(),
						   rootRoomPath + "MasterSuite.txt");//Loot
			myRooms.Add(newRoom.myRoomID, newRoom);
		}

		public Dictionary<int, Room> GetRooms()
		{
			return myRooms;
		}
	}
}
