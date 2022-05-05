
namespace ConsoleRPG
{
    internal class TownPortal
    {
        public int myOriginRoomID;
        public char[,] mySprite;
        public bool myIsPlaced;

        public TownPortal()
        {
            myOriginRoomID = 0;
            mySprite = Utilities.ReadFromFile(@"Sprites\Portal.txt", out string aTitle);
            myIsPlaced = false;
        }

        public void PlacePortal(int aRoomID)
        {
            myOriginRoomID = aRoomID;
            myIsPlaced = true;
            Vector2 offSet = new Vector2(107, 11);
            for (int y = 0; y < mySprite.GetLength(1); y++)
            {
                for (int x = 0; x < mySprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + offSet.X, y + offSet.Y, mySprite[x, y]);
                }
            }
        }

        public void DrawPortalInTown()
        {
            Vector2 offSet = new Vector2(31, 15);
            for (int y = 0; y < mySprite.GetLength(1); y++)
            {
                for (int x = 0; x < mySprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + offSet.X, y + offSet.Y, mySprite[x, y]);
                }
            }
        }
    }
}
