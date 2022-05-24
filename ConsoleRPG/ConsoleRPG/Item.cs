
namespace ConsoleRPG
{
    internal class Item
    {
        public string myTitle;
        public int myItemID;
        public Stat myStat;

        public Item()
        {
            myTitle = "";
            myItemID = 0;
            myStat = null;
        }

        public Item(string aTitle, Stat aStat)
        {
            myTitle = aTitle;
            myStat = aStat;
            myItemID = aStat.myID;
        }

        public Item(Item anItem)
        {
            myTitle = anItem.myTitle;
            myStat = anItem.myStat;
            myItemID = anItem.myItemID;
        }
    }
}
