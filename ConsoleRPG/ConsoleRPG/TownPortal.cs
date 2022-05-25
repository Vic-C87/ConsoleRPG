
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
            mySprite = Utilities.ReadFromFile(@"Sprites/Portal.txt", out _);
            myIsPlaced = false;
        }

        public void PlacePortal(int aRoomID, Vector2 anOffSet)
        {
            myOriginRoomID = aRoomID;
            myIsPlaced = true;
            for (int y = 0; y < mySprite.GetLength(1); y++)
            {
                for (int x = 0; x < mySprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + anOffSet.X, y + anOffSet.Y, mySprite[x, y]);
                }
            }
        }

        public void DrawPortalInTown(Vector2 anOffSet)
        {
            for (int y = 0; y < mySprite.GetLength(1); y++)
            {
                for (int x = 0; x < mySprite.GetLength(0); x++)
                {
                    Utilities.Draw(x + anOffSet.X, y + anOffSet.Y, mySprite[x, y]);
                }
            }
        }
    }
}
