using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;

namespace GuildWarsInterface.Datastructures.Items
{
        public sealed class Bag : InventoryPage
        {
                internal Bag(StorageType bagType, Item bag, byte size)
                        : base(InventoryType.Bag, bagType, size, bag)
                {
                        Debug.Requires(bag != null);
                        Debug.Requires(size % 5 == 0);
                        Debug.Requires(bagType == StorageType.Backpack ||
                                       bagType == StorageType.BeltPouch ||
                                       bagType == StorageType.Bag1 ||
                                       bagType == StorageType.Bag2 ||
                                       bagType == StorageType.EquipmentPack);
                }
        }
}