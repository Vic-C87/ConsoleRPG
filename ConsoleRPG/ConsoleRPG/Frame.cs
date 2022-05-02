using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRPG
{
    internal class Frame
    {
        public FrameType myFrameType;
        public char[,] myBorder;
        public List<BattleOption> myBattleOptions;
        public Vector2 myPosition;

        public Frame()
        {
            myFrameType = FrameType.Frame;
            myBorder = new char[20, 8];
            myBattleOptions = new List<BattleOption>();
            myPosition = new Vector2();
        }

        public Frame(FrameType aFrameType, int aFrameWidth, int aFrameHeight, List<BattleOption> someBattleOptions, Vector2 aPosition)
        {
            myFrameType = aFrameType;
            myBorder = new char[aFrameWidth, aFrameHeight];
            myBattleOptions = someBattleOptions;
            myPosition = aPosition;
        }

        public void SetFrame()
        {

        }

        public void DrawFrame()
        {
            for (int y = 0; y < myBorder.GetLength(1); y++)
            {
                for (int x = 0; x < myBorder.GetLength(0); x++)
                {
                    Utilities.Draw(x + myPosition.X, y + myPosition.Y, myBorder[x, y]);
                }
            }
        }

        public void RemoveOption(BattleOption anOptionToRemove)
        {
            if (myBattleOptions.Contains(anOptionToRemove))
            {
                //Remove  Option and edit position for other options
            }
        }

    }
}
