using System;
using System.Linq;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

namespace GuildWarsInterface.Datastructures.Items
{
        public sealed class Equipment : InventoryPage
        {
                private readonly Weaponset[] _weaponsets;
                private byte _currentWeaponset;

                internal Equipment()
                        : base(InventoryType.Equipped, StorageType.Equipped, 9)
                {
                        _weaponsets = new Weaponset[4];

                        for (int i = 0; i < 4; i++)
                        {
                                _weaponsets[i] = new Weaponset();
                        }

                        _currentWeaponset = 0;
                }

                internal override void CreateItem(Item item, byte slot)
                {
                        Debug.ThrowException(new Exception("should use the more specific function instead"));
                }

                internal override void MoveItem(Item item, byte slot)
                {
                        Debug.ThrowException(new Exception("should use the more specific function instead"));
                }

                internal void CreateItem(Item item, EquipmentSlot slot)
                {
                        base.CreateItem(item, (byte) slot);

                        EquipmentChanged(item, slot);
                }

                internal override void SwitchItem(Item sourceItem, Item targetItem)
                {
                        base.SwitchItem(sourceItem, targetItem);

                        var slot = (EquipmentSlot) Items.FirstOrDefault(entry => entry.Key == sourceItem).Value.Value;

                        EquipmentChanged(sourceItem, slot);
                }

                internal void MoveItem(Item item, EquipmentSlot slot)
                {
                        base.MoveItem(item, (byte) slot);

                        EquipmentChanged(item, slot);
                }

                public bool TryGet(EquipmentSlot equipmentSlot, out Item equippedItem)
                {
                        return TryGet((byte) equipmentSlot, out equippedItem);
                }

                internal void EquipmentChanged(Item item, EquipmentSlot slot)
                {
                        switch (slot)
                        {
                                case EquipmentSlot.Mainhand:
                                        _weaponsets[_currentWeaponset].Mainhand = item;
                                        break;
                                case EquipmentSlot.Offhand:
                                        _weaponsets[_currentWeaponset].Offhand = item;
                                        break;
                        }

                        if (Game.State == GameState.Playing)
                        {
                                Game.Player.Character.UpdateFullEquipment();
                        }
                }

                public Item GetWeaponsetMainhandItem(byte weaponset)
                {
                        Debug.Requires(weaponset <= 3);

                        return _weaponsets[weaponset].Mainhand;
                }

                public void SetWeaponsetMainhandItem(Item weapon, byte weaponset)
                {
                        Debug.Requires(weaponset <= 3);

                        if (weaponset == _currentWeaponset)
                        {
                                Game.Player.Character.Inventory.SetItem(weapon, this, EquipmentSlot.Mainhand);
                        }
                        else
                        {
                                _weaponsets[weaponset].Mainhand = weapon;

                                if (Game.State == GameState.Playing)
                                {
                                        Network.GameServer.Send(GameServerMessage.UpdateWeaponsetWeapons,
                                                                (ushort) 1,
                                                                weaponset,
                                                                _weaponsets[weaponset].Mainhand != null ? IdManager.GetId(_weaponsets[weaponset].Mainhand) : 0,
                                                                _weaponsets[weaponset].Offhand != null ? IdManager.GetId(_weaponsets[weaponset].Offhand) : 0);
                                }
                        }
                }

                public Item GetWeaponsetOffhandItem(byte weaponset)
                {
                        Debug.Requires(weaponset <= 3);

                        return _weaponsets[weaponset].Offhand;
                }

                public void SetWeaponsetOffhandItem(Item weapon, byte weaponset)
                {
                        Debug.Requires(weaponset <= 3);

                        if (weaponset == _currentWeaponset)
                        {
                                Game.Player.Character.Inventory.SetItem(weapon, this, EquipmentSlot.Offhand);
                        }
                        else
                        {
                                _weaponsets[weaponset].Offhand = weapon;

                                if (Game.State == GameState.Playing)
                                {
                                        Network.GameServer.Send(GameServerMessage.UpdateWeaponsetWeapons,
                                                                (ushort) 1,
                                                                weaponset,
                                                                _weaponsets[weaponset].Mainhand != null ? IdManager.GetId(_weaponsets[weaponset].Mainhand) : 0,
                                                                _weaponsets[weaponset].Offhand != null ? IdManager.GetId(_weaponsets[weaponset].Offhand) : 0);
                                }
                        }
                }

                public void SetActiveWeaponset(byte newWeaponset)
                {
                        Debug.Requires(newWeaponset <= 3);

                        if (_currentWeaponset == newWeaponset) return;


                        Item oldLeadhand = _weaponsets[_currentWeaponset].Mainhand;
                        Item oldOffhand = _weaponsets[_currentWeaponset].Offhand;

                        _currentWeaponset = newWeaponset;

                        Item newLeadhand = _weaponsets[newWeaponset].Mainhand;
                        Item newOffhand = _weaponsets[newWeaponset].Offhand;


                        if (Game.State == GameState.Playing)
                        {
                                Network.GameServer.Send(GameServerMessage.UpdateActiveWeaponset, (ushort) 1, newWeaponset);
                        }

                        SwitchItems(oldLeadhand, newLeadhand, EquipmentSlot.Mainhand);
                        SwitchItems(oldOffhand, newOffhand, EquipmentSlot.Offhand);
                }

                private void SwitchItems(Item oldItem, Item newItem, EquipmentSlot equipmentSlot)
                {
                        if (oldItem == newItem) return; // same item again

                        if (oldItem != null)
                        {
                                if (newItem != null)
                                {
                                        SwitchItem(newItem, oldItem);
                                }
                                else
                                {
                                        Bag bag;
                                        byte slot;
                                        if (!Game.Player.Character.Inventory.TryGetFreeSlot(out bag, out slot))
                                        {
                                                Debug.ThrowException(new ArgumentException("no space to switch"));
                                        }

                                        bag.MoveItem(oldItem, slot);

                                        EquipmentChanged(newItem, equipmentSlot);
                                }
                        }
                        else if (newItem != null)
                        {
                                MoveItem(newItem, equipmentSlot);
                        }
                }

                internal void LoadWeaponsets()
                {
                        for (byte i = 0; i < 4; i++)
                        {
                                Item leadhand = _weaponsets[i].Mainhand;
                                Item offhand = _weaponsets[i].Offhand;

                                Debug.Requires(leadhand == null || leadhand.Created);
                                Debug.Requires(offhand == null || offhand.Created);

                                Network.GameServer.Send(GameServerMessage.UpdateWeaponsetWeapons,
                                                        (ushort) 1,
                                                        i,
                                                        leadhand != null ? IdManager.GetId(leadhand) : 0,
                                                        offhand != null ? IdManager.GetId(offhand) : 0);
                        }
                }

                // TODO: remove method
                public Item Get(EquipmentSlot slot)
                {
                        Item result;
                        if (TryGet(slot, out result))
                        {
                                return result;
                        }

                        return null;
                }
        }
}