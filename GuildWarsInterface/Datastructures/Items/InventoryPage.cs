using System;
using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Datastructures.Base;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

namespace GuildWarsInterface.Datastructures.Items
{
        public class InventoryPage : Creatable, IIdentifyable
        {
                internal static readonly Dictionary<Item, KeyValuePair<InventoryPage, byte>> Items = new Dictionary<Item, KeyValuePair<InventoryPage, byte>>();

                public readonly Item Bag;
                public readonly byte Size;
                private readonly StorageType _storage;
                private readonly InventoryType _type;

                internal InventoryPage(InventoryType type, StorageType storage, byte size, Item bag = null)
                {
                        _type = type;
                        _storage = storage;
                        Size = size;
                        Bag = bag;
                }

                internal virtual void CreateItem(Item item, byte slot)
                {
                        Debug.Requires(item != null);
                        Debug.Requires(!Items.ContainsKey(item));
                        Item itemCurrentlyInSlot;
                        Debug.Requires(!TryGet(slot, out itemCurrentlyInSlot));

                        if (Game.State == GameState.Playing)
                        {
                                if (!item.Created) item.Create();

                                CreateItemInstance(item, slot);
                        }

                        Items.Add(item, new KeyValuePair<InventoryPage, byte>(this, slot));
                }

                internal virtual void MoveItem(Item item, byte slot)
                {
                        Debug.Requires(item != null);
                        Debug.Requires(Items.ContainsKey(item));
                        Item itemCurrentlyInSlot;
                        Debug.Requires(!TryGet(slot, out itemCurrentlyInSlot));
                        Debug.Requires(slot < Size);

                        if (Game.State == GameState.Playing)
                        {
                                CreateMoveItem(item, slot);
                        }

                        Items[item] = new KeyValuePair<InventoryPage, byte>(this, slot);
                }

                internal virtual void SwitchItem(Item sourceItem, Item targetItem)
                {
                        Debug.Requires(sourceItem != null);
                        Debug.Requires(Items.ContainsKey(sourceItem));
                        Debug.Requires(targetItem != null);
                        Debug.Requires(Items.ContainsKey(targetItem));

                        if (Game.State == GameState.Playing)
                        {
                                CreateSwitchItem(sourceItem, targetItem);
                        }

                        KeyValuePair<InventoryPage, byte> sourceLocation = Items[sourceItem];
                        KeyValuePair<InventoryPage, byte> targetLocation = Items[targetItem];

                        Items[sourceItem] = targetLocation;
                        Items[targetItem] = sourceLocation;
                }

                internal void PutItem(Item item, byte slot)
                {
                        Debug.Requires(item != null);
                        Debug.Requires(!Items.ContainsKey(item));
                        Item itemCurrentlyInSlot;
                        Debug.Requires(!TryGet(slot, out itemCurrentlyInSlot));

                        Items.Add(item, new KeyValuePair<InventoryPage, byte>(this, slot));
                }

                protected override void OnCreation()
                {
                        uint associatedBagItemId = 0;

                        if (Bag != null)
                        {
                                if (!Bag.Created) Bag.Create();

                                associatedBagItemId = IdManager.GetId(Bag);
                        }

                        if (_type == InventoryType.Bag && Game.State == GameState.Playing)
                        {
                                Network.GameServer.Send(GameServerMessage.CreateBag,
                                                        (ushort) 1,
                                                        (byte) _storage,
                                                        IdManager.GetId(this),
                                                        Size,
                                                        associatedBagItemId);
                        }
                        else
                        {
                                Network.GameServer.Send(GameServerMessage.CreateInventoryPage,
                                                        (ushort) 1,
                                                        (byte) _type,
                                                        (byte) _storage,
                                                        IdManager.GetId(this),
                                                        Size,
                                                        associatedBagItemId);
                        }
                }

                protected override void OnDestruction()
                {
                        Debug.ThrowException(new Exception("cannot unload inventory page"));
                }

                internal void LoadItems()
                {
                        if (!Created) Create();

                        foreach (var itemsEntry in Items.Where(entry => entry.Value.Key == this))
                        {
                                Console.WriteLine("loading " + itemsEntry.Key);
                                if (!itemsEntry.Key.Created) itemsEntry.Key.Create();

                                CreateItemInstance(itemsEntry.Key, itemsEntry.Value.Value);
                        }
                }

                private void CreateItemInstance(Item item, byte slot)
                {
                        Debug.Requires(slot < Size);
                        Debug.Requires(item != null);
                        Debug.Requires(item.Created);

                        Network.GameServer.Send(GameServerMessage.ItemLocation,
                                                (ushort) 1,
                                                IdManager.GetId(item),
                                                IdManager.GetId(this),
                                                slot);
                }

                private void CreateSwitchItem(Item sourceItem, Item targetItem)
                {
                        Network.GameServer.Send(GameServerMessage.SwapItems,
                                                (ushort) 1,
                                                IdManager.GetId(sourceItem),
                                                IdManager.GetId(targetItem));
                }

                private void CreateMoveItem(Item item, byte slot)
                {
                        Network.GameServer.Send(GameServerMessage.MoveItem,
                                                (ushort) 1,
                                                IdManager.GetId(item),
                                                IdManager.GetId(this),
                                                slot);
                }

                public bool TryGetFreeSlot(out byte slot)
                {
                        for (byte i = 0; i < Size; i++)
                        {
                                Item dummy;
                                if (!TryGet(i, out dummy))
                                {
                                        slot = i;
                                        return true;
                                }
                        }

                        slot = 0;
                        return false;
                }

                public virtual bool TryGet(byte slot, out Item result)
                {
                        result = Items.FirstOrDefault(entry => entry.Value.Key == this && entry.Value.Value == slot).Key;

                        return result != null;
                }
        }
}