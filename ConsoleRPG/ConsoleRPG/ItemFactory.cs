using System.Collections.Generic;

namespace ConsoleRPG
{
    internal class ItemFactory
    {
        readonly Dictionary<UpgradeType, Item> myItems = new Dictionary<UpgradeType, Item>();

        public ItemFactory()
        {
            myItems.Add(UpgradeType.HealthUpgrade, new Item("Health upgrade", new Stat(11, 10, 0, 0, 0)));

            myItems.Add(UpgradeType.ManaUpgrade, new Item("Mana upgrade", new Stat(12, 0, 5, 0, 0)));

            myItems.Add(UpgradeType.SwordUpgrade, new Item("Sword upgrade", new Stat(13, 0, 0, 5, 0)));

            myItems.Add(UpgradeType.ArmorUpgrade, new Item("Armor upgrade", new Stat(14, 0, 0, 0, 5)));

            myItems.Add(UpgradeType.FullUpgrade, new Item("Full upgrade", new Stat(15, 5, 2, 2, 2)));
        }

        public bool GetItem(UpgradeType aType, out Item anItem)
        {
            bool typeExist = aType switch
            {
                UpgradeType.SwordUpgrade => true,
                UpgradeType.ArmorUpgrade => true,
                UpgradeType.HealthUpgrade => true,
                UpgradeType.ManaUpgrade => true,
                UpgradeType.FullUpgrade => true,
                _ => false,
            };
            anItem = typeExist ? myItems[aType] : new Item();
            return typeExist;
        }
    }
}
