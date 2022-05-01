using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class RoomFactory
    {
		Dictionary<int, Room> myRooms;

		public RoomFactory()
		{
			myRooms = new Dictionary<int, Room>();
		}

		public void CreateRooms()
		{
			string rootRoomPath = @"Sprites\Rooms\";
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
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Greathall", "a large hall with high ceilling",
					2, false,
					new Dictionary<int, DoorDirections>() { { 1, DoorDirections.South }, { 2, DoorDirections.North }, { 3, DoorDirections.West }, { 7, DoorDirections.East } },
					new List<int>(),
					rootRoomPath + "Room_W-N-E-S.txt");
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Cloakroom", "a small cloakroom",
					3, false,
					new Dictionary<int, DoorDirections>() { { 2, DoorDirections.South } },
					new List<int>(),
					rootRoomPath + "Room_S.txt",
					true);//Key to door 5
			newRoom.AddKeyToChest(1);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Hall", "a hall",
					4, false,
					new Dictionary<int, DoorDirections>() { { 3, DoorDirections.East }, { 4, DoorDirections.West }, { 12, DoorDirections.North } },
						   new List<int>(),
						   rootRoomPath + "Room_W-N-E.txt");
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Sittingroom", "a large sittingroom with ...",
					5, false,
					new Dictionary<int, DoorDirections>() { { 4, DoorDirections.East }, { 5, DoorDirections.West } },
						   new List<int>(),
						   rootRoomPath + "Room_W-E.txt"); //Loot
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Large bedroom", "a grand bedroom",
					6, false,
					new Dictionary<int, DoorDirections>() { { 5, DoorDirections.East }, { 6, DoorDirections.South } },
						   new List<int>(),
						   rootRoomPath + "Room_E-S.txt",
						   true);//Loot and Key to door 7
			newRoom.AddKeyToChest(2);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Bathroom", "a small bathroom",
					7, false,
					new Dictionary<int, DoorDirections>() { { 6, DoorDirections.North } },
						   new List<int>(),
						   rootRoomPath + "Room_N.txt");
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Diningroom", "a big diningroom",
					8, false,
					new Dictionary<int, DoorDirections>() { { 7, DoorDirections.West }, { 8, DoorDirections.South }, { 10, DoorDirections.North }, { 11, DoorDirections.East } },
						   new List<int>(),
						   rootRoomPath + "Room_W-N-E-S.txt");//Loot
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Washroom", "an old and small washroom",
					9, false,
					new Dictionary<int, DoorDirections>() { { 8, DoorDirections.North }, { 9, DoorDirections.East } },
						   new List<int>(),
						   rootRoomPath + "Room_N-E.txt");
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Storageroom", "a small dusty storeroom",
					10, false,
					new Dictionary<int, DoorDirections>() { { 9, DoorDirections.West } },
						   new List<int>(),
						   rootRoomPath + "Room_W.txt");//Loot
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Kitchen", "a big kitchen...",
					11, false,
					new Dictionary<int, DoorDirections>() { { 10, DoorDirections.South } },
						   new List<int>(),
						   rootRoomPath + "Room_S.txt",
						   true);//Loot and Key to door 11
			newRoom.AddKeyToChest(3);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Library", "a huge library filled with tall bookshelfs",
					12, false,
					new Dictionary<int, DoorDirections>() { { 11, DoorDirections.West } },
						   new List<int>(),
						   rootRoomPath + "Room_W.txt",
						   true);//Loot and Key to door 12
			newRoom.AddKeyToChest(4);
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Large study", "a large study with a big desk at the far end of the room",
					13, false,
					new Dictionary<int, DoorDirections>() { { 12, DoorDirections.South }, { 13, DoorDirections.East } },
						   new List<int>(),
						   rootRoomPath + "Room_E-S.txt");//Loot
			myRooms.Add(newRoom.myRoomID, newRoom);

			newRoom = new Room("Master suit", "a wide courtyard",
					14, false,
					new Dictionary<int, DoorDirections>() { { 13, DoorDirections.West } },
						   new List<int>(),
						   rootRoomPath + "Room_W.txt");//Loot
			myRooms.Add(newRoom.myRoomID, newRoom);
		}

		public Dictionary<int, Room> GetRooms()
		{
			return myRooms;
		}
	}
}
