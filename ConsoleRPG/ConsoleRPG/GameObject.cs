using System;
using System.Threading;

namespace ConsoleRPG
{
    internal class GameObject
    {
        public int MyWidth;
        public int MyHeight;

        public string MyTitle;
        public char[,] MySprite;
        public Vector2 MyPosition;

        public GameObject(Vector2 aSize, string aTitle, char[,] aSprite)
        {
            MyWidth = aSize.X;
            MyHeight = aSize.Y;
            MyTitle = aTitle;
            MySprite = new char[MyWidth, MyHeight];
            MySprite = aSprite;
            MyPosition = new Vector2(0, 0);

        }

        /// <summary>
        /// Takes in a Vector2 and draws the Sprite of this GameObject to screen at that given Vector2 position
        /// </summary>
        /// <param name="anOffSet"></param>
        /// <returns></returns>
        public bool DrawSprite(Vector2 anOffSet)
        {
            bool drawCompleted = false;
            MyPosition = anOffSet;

            for (int y = 0; y < MyHeight; y++)
            {
                for (int x = 0; x < MyWidth; x++)
                {
                    Utilities.Draw(x + MyPosition.X, y + MyPosition.Y, MySprite[x, y]);
                    if (y == MyHeight - 1 && x == MyWidth - 1)
                    {
                        drawCompleted = true;
                    }
                }
                Console.WriteLine();
            }
            return drawCompleted;
        }

        public bool ClearSprite()
        {
            bool clearCompleted = false;
            for (int y = 0; y < MyHeight; y++)
            {
                for (int x = 0; x < MyWidth; x++)
                {
                    Utilities.Draw(x + MyPosition.X, y + MyPosition.Y, ' ');
                    if (y == MyHeight - 1 && x == MyWidth - 1)
                    {
                        clearCompleted = true;
                    }
                }
                Console.WriteLine();
            }
            return clearCompleted;
        }

        /// <summary>
        /// Move the Sprite of this GameObject in the given direction, aSpeedModifier should be left at default and is only used when called inside function MoveTo(Vector2,int)
        /// </summary>
        /// <param name="aDirection"></param>
        /// <param name="aSpeedModifier"></param>
        public void Move(Vector2 aDirection, int aSpeedModifier = 10)
        {
            int speed = Utilities.Clamp(100 - (aSpeedModifier * 10), 1, 100);
            ClearSprite();
            DrawSprite(aDirection);
            MyPosition.SetPosition(aDirection);
            Thread.Sleep(speed);
        }

        /// <summary>
        /// Takes in a destination as a Vector2 and moves the Sprite to that location, 
        /// speedModifier should be an int between 0 and 10
        /// </summary>
        /// <param name="aTarget"></param>
        /// <param name="aSpeedModifier"></param>
        public void MoveTo(Vector2 aTarget, int aSpeedModifier)
        {
            if (MyPosition.Match(aTarget))
            {
                return;
            }
            else
            {
                int xSteps = Utilities.Abs(aTarget.X - MyPosition.X);
                int ySteps = Utilities.Abs(aTarget.Y - MyPosition.Y);

                if (ySteps <= xSteps && ySteps != 0)
                {
                    if (MyPosition.Y > aTarget.Y)
                    {
                        for (int y = 0; y < ySteps; y++)
                        {
                            Move(MyPosition.Up(), aSpeedModifier);
                        }

                    }
                    else
                    {
                        for (int y = 0; y < ySteps; y++)
                        {
                            Move(MyPosition.Down(), aSpeedModifier);
                        }
                    }



                    if (MyPosition.X > aTarget.X)
                    {
                        for (int x = 0; x < xSteps; x++)
                        {
                            Move(MyPosition.Left(), aSpeedModifier);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < xSteps; x++)
                        {
                            Move(MyPosition.Right(), aSpeedModifier);
                        }
                    }
                }
                else if (xSteps < ySteps && xSteps != 0)
                {
                    if (MyPosition.X > aTarget.X)
                    {
                        for (int x = 0; x < xSteps; x++)
                        {
                            Move(MyPosition.Left(), aSpeedModifier);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < xSteps; x++)
                        {
                            Move(MyPosition.Right(), aSpeedModifier);
                        }
                    }

                    if (MyPosition.Y > aTarget.Y)
                    {
                        for (int y = 0; y < ySteps; y++)
                        {
                            Move(MyPosition.Up(), aSpeedModifier);
                        }

                    }
                    else
                    {
                        for (int y = 0; y < ySteps; y++)
                        {
                            Move(MyPosition.Down(), aSpeedModifier);
                        }
                    }
                }
            }
        }
    }
}
