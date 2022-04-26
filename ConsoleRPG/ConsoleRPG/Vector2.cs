

namespace ConsoleRPG
{
    internal struct Vector2
    {
        public int X;
        public int Y;

        public Vector2(int anXPosition, int aYPosition)
        {
            X = anXPosition;
            Y = aYPosition;
        }

        public Vector2(Vector2 aCopy)
        {
            X = aCopy.X;
            Y = aCopy.Y;
        }

        public Vector2 Up()
        {
            return new Vector2(X, Y - 1);
        }

        public Vector2 Right()
        {
            return new Vector2(X + 1, Y);
        }

        public Vector2 Down()
        {
            return new Vector2(X, Y + 1);
        }

        public Vector2 Left()
        {
            return new Vector2(X - 1, Y);
        }

        public void SetPosition(Vector2 aTarget)
        {
            X = aTarget.X;
            Y = aTarget.Y;
        }

        public bool Match(Vector2 aTarget)
        {
            if (aTarget.X == X && aTarget.Y == Y)
            {
                return true;
            }
            return false;
        }
    }
}
