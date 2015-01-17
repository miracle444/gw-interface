using System;
using System.Linq;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

namespace GuildWarsInterface.Datastructures.Items
{
        public sealed class Inventory
        {
                public readonly Equipment Equipment = new Equipment();

                private readonly Bag[] _bags = new Bag[5];

                internal Inventory()
                {
                }

                public Bag Backpack
                {
                        get { return _bags[0]; }
                }

                public Bag Beltpouch
                {
                        get { return _bags[1]; }
                }

                public Bag Bag1
                {
                        get { return _bags[2]; }
                }

                public Bag Bag2
                {
                        get { return _bags[3]; }
                }

                public Bag EquipmentPack
                {
                        get { return _bags[4]; }
                }

                internal void LoadItems()
                {
                        Equipment.LoadItems();

                        foreach (Bag bag in _bags.Where(bag => bag != null))
                        {
                                bag.LoadItems();
                        }

                        Equipment.LoadWeaponsets();
                }

                public void SetBag(Item bag, byte bagSlot)
                {
                        Debug.Requires(bag != null);
                        Debug.Requires(bagSlot <= 4);

                        Bag equippedBag = _bags.FirstOrDefault(entry => entry != null && entry.Bag == bag);

                        if (equippedBag == null)
                        {
                                if (_bags[bagSlot] != null)
                                {
                                        Debug.ThrowException(new Exception("bagslot already occupied"));
                                }

                                _bags[bagSlot] = new Bag((StorageType) bagSlot, bag, 20);

                                if (Game.State == GameState.Playing)
                                {
                                        _bags[bagSlot].Create();

                                        if (InventoryPage.Items.ContainsKey(bag))
                                        {
                                                InventoryPage.Items.Remove(bag);

                                                Network.GameServer.Send(GameServerMessage.EquipBag, bagSlot, IdManager.GetId(bag));
                                        }
                                }
                        }
                        else
                        {
                                var currentBagSlot = (byte) Array.IndexOf(_bags, equippedBag);

                                if (currentBagSlot != bagSlot)
                                {
                                        MoveBag(currentBagSlot, bagSlot);
                                }
                        }
                }

                private void MoveBag(byte sourceBagSlot, byte targetBagSlot)
                {
                        Debug.Requires(sourceBagSlot <= 4);
                        Debug.Requires(targetBagSlot <= 4);
                        Debug.Requires(_bags[sourceBagSlot] != null);

                        Bag sourceBag = _bags[sourceBagSlot];
                        Bag targetBag = _bags[targetBagSlot];

                        _bags[targetBagSlot] = sourceBag;
                        _bags[sourceBagSlot] = targetBag;

                        if (Game.State == GameState.Playing)
                        {
                                Network.GameServer.Send(GameServerMessage.MoveBag, (ushort) 1, sourceBagSlot, targetBagSlot);
                        }
                }

                private bool TryGetBagSlot(Bag bag, out byte bagSlot)
                {
                        for (bagSlot = 0; bagSlot < _bags.Length; bagSlot++)
                        {
                                if (_bags[bagSlot] == bag)
                                {
                                        return true;
                                }
                        }

                        return false;
                }

                private void UnEquipBag(Bag bag, InventoryPage targetPage, byte targetSlot)
                {
                        Debug.Requires(bag != null);
                        Debug.Requires(targetPage != null);
                        Debug.Requires(targetSlot < targetPage.Size);
                        Debug.Requires(bag != targetPage);
                        Debug.Requires(InventoryPage.Items.All(entry => entry.Value.Key != bag));

                        byte bagSlot;
                        if (!TryGetBagSlot(bag, out bagSlot))
                        {
                                Debug.ThrowException(new ArgumentException("bag not equipped"));
                        }

                        _bags[bagSlot] = null;

                        targetPage.PutItem(bag.Bag, targetSlot);

                        if (Game.State == GameState.Playing)
                        {
                                Network.GameServer.Send(GameServerMessage.UnEquipBag, (ushort) 1, IdManager.GetId(bag.Bag), IdManager.GetId(targetPage), targetSlot);
                        }
                }

                private void UnEquipItem(EquipmentSlot equipmentSlot, InventoryPage targetPage, byte targetSlot)
                {
                        Debug.Requires(targetPage != null);
                        Debug.Requires(targetSlot < targetPage.Size);
                        Item currentlyEquippedItem;
                        Debug.Requires(Equipment.TryGet(equipmentSlot, out currentlyEquippedItem));

                        targetPage.MoveItem(currentlyEquippedItem, targetSlot);

                        Equipment.EquipmentChanged(null, equipmentSlot);
                }

                private void EquipItem(Item item, EquipmentSlot slot)
                {
                        Debug.Requires(item != null);
                        Debug.Requires(InventoryPage.Items.ContainsKey(item));
                        Debug.Requires(InventoryPage.Items[item].Key != Equipment);

                        Item equippedItem;
                        if (!Equipment.TryGet(slot, out equippedItem))
                        {
                                Equipment.MoveItem(item, slot);
                        }
                        else
                        {
                                Equipment.SwitchItem(item, equippedItem);
                        }
                }

                internal bool TryGetFreeBagSlot(out byte bagSlot)
                {
                        for (bagSlot = 0; bagSlot < _bags.Length; bagSlot++)
                        {
                                if (_bags[bagSlot] == null)
                                {
                                        return true;
                                }
                        }

                        return false;
                }

                public bool TryGetFreeSlot(out Bag bag, out byte slot)
                {
                        foreach (Bag b in _bags)
                        {
                                if (b != null && b.TryGetFreeSlot(out slot))
                                {
                                        bag = b;
                                        return true;
                                }
                        }

                        bag = null;
                        slot = 0;
                        return false;
                }

                public void SetItem(Item item, Equipment equipment, EquipmentSlot slot)
                {
                        Bag equippedBag = _bags.FirstOrDefault(entry => entry != null && entry.Bag == item);

                        if (equippedBag == null)
                        {
                                if (!InventoryPage.Items.ContainsKey(item))
                                {
                                        equipment.CreateItem(item, slot);
                                }
                                else
                                {
                                        InventoryPage currentPage = InventoryPage.Items[item].Key;

                                        if (_bags.Contains(currentPage))
                                        {
                                                EquipItem(item, slot);
                                        }
                                        else
                                        {
                                                Debug.ThrowException(new Exception("cannot equip item which is not present in any active bag"));
                                        }
                                }
                        }
                        else
                        {
                                Debug.ThrowException(new Exception("cannot put an equipped bag into equipment"));
                        }
                }

                public void SetItem(Item item, Bag bag, byte slot)
                {
                        Bag equippedBag = _bags.FirstOrDefault(entry => entry != null && entry.Bag == item);

                        if (equippedBag == null)
                        {
                                if (!InventoryPage.Items.ContainsKey(item))
                                {
                                        bag.CreateItem(item, slot);
                                }
                                else
                                {
                                        InventoryPage currentPage = InventoryPage.Items[item].Key;
                                        byte currentSlot = InventoryPage.Items[item].Value;

                                        if (currentPage == Equipment)
                                        {
                                                Item itemAtTargetLocation;
                                                if (!bag.TryGet(slot, out itemAtTargetLocation))
                                                {
                                                        UnEquipItem((EquipmentSlot) currentSlot, bag, slot);
                                                }
                                                else
                                                {
                                                        // unequip to occupied slot
                                                        Debug.ThrowException(new NotImplementedException());
                                                }
                                        }
                                        else if (_bags.Contains(currentPage))
                                        {
                                                Item itemAtTargetLocation;
                                                if (!bag.TryGet(slot, out itemAtTargetLocation))
                                                {
                                                        bag.MoveItem(item, slot);
                                                }
                                                else
                                                {
                                                        bag.SwitchItem(item, itemAtTargetLocation);
                                                }
                                        }
                                        else
                                        {
                                                Debug.ThrowException(new Exception("the bag this item is in is not active"));
                                        }
                                }
                        }
                        else
                        {
                                UnEquipBag(equippedBag, bag, slot);
                        }
                }

                internal void SetItem(Item item, InventoryPage page, byte slot)
                {
                        if (page is Equipment)
                        {
                                SetItem(item, (Equipment) page, (EquipmentSlot) slot);
                        }
                        else if (page is Bag)
                        {
                                SetItem(item, (Bag) page, slot);
                        }
                        else
                        {
                                Debug.ThrowException(new ArgumentException("can only handle equipment or bag"));
                        }
                }

                public Bag GetBag(byte bagSlot)
                {
                        return _bags[bagSlot];
                }
        }
}