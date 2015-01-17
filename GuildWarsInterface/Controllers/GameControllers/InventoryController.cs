#region

using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class InventoryController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(42, EquipItemHandler);
                        controllerManager.RegisterHandler(44, ChangeWeaponsetHandler);
                        controllerManager.RegisterHandler(73, UnEquipItemHandler);
                        controllerManager.RegisterHandler(101, EquipBagHandler);
                        controllerManager.RegisterHandler(110, MoveBagHandler);
                        controllerManager.RegisterHandler(108, MoveItemHandler);
                        controllerManager.RegisterHandler(119, UnEquipBagHandler);
                }

                private void EquipItemHandler(List<object> objects)
                {
                        /*Item item;
                        if (!IdManager.TryGet((uint) objects[1], out item))
                        {
                                Debug.ThrowException(new ArgumentException("unknown item id"));
                        }

                        EquipmentSlot equipmentSlot;
                        if (!DeclarationConversion.ItemTypeToEquipmentSlot(item.Type, out equipmentSlot))
                        {
                                Debug.ThrowException(new ArgumentException("cannot equip item (unknown itemtype->equipmentslot conversion)"));
                        }

                        Game.Player.Inventory.SetItem(item, Game.Player.Inventory.Equipment, equipmentSlot);*/
                }

                private void ChangeWeaponsetHandler(List<object> objects)
                {
                        //Game.Player.Inventory.Equipment.SetActiveWeaponset((byte) objects[1]);
                }

                private void UnEquipItemHandler(List<object> objects)
                {
                        /*var equipmentSlot = (EquipmentSlot) (byte)objects[1];

                        InventoryPage targetPage;
                        if (!IdManager.TryGet((uint) objects[2], out targetPage))
                        {
                                Debug.ThrowException(new ArgumentException("unknown page id"));
                        }
                        else if (targetPage == Game.Player.Inventory.Equipment)
                        {
                                Debug.ThrowException(new ArgumentException("cannot unequip to equipment"));
                        }
                        else if (!(targetPage is Bag))
                        {
                                Debug.ThrowException(new ArgumentException("can only unequip equipment to a bag"));
                        }

                        var targetBag = (Bag) targetPage;

                        var targetSlot = (byte) objects[3];

                        if (targetSlot == 0xFF)
                        {
                                // first free slot (gets used when item is drawn directly on a bag)

                                if (!targetPage.TryGetFreeSlot(out targetSlot))
                                {
                                        // TODO: add apropriate native handling
                                        Console.WriteLine("bag full! cannot put item in there");
                                        return;
                                }
                        }

                        Item equippedItem;
                        if (!Game.Player.Inventory.Equipment.TryGet(equipmentSlot, out equippedItem))
                        {
                                Debug.ThrowException(new ArgumentException("no equipped item at equipment slot"));
                        }

                        Game.Player.Inventory.SetItem(equippedItem, targetBag, targetSlot);*/
                }


                private void UnEquipBagHandler(List<object> objects)
                {
                        /*Item bag;
                        if (!IdManager.TryGet((uint) objects[1], out bag))
                        {
                                Debug.ThrowException(new ArgumentException("unknown bag item"));
                        }

                        InventoryPage page;
                        if (!IdManager.TryGet((ushort) objects[2], out page))
                        {
                                Debug.ThrowException(new ArgumentException("unknown page id"));
                        }

                        var slot = (byte) objects[3];

                        if (slot == 0)
                        {
                                // first free slot (gets used when item is drawn directly on a bag)

                                if (!page.TryGetFreeSlot(out slot))
                                {
                                        // TODO: add apropriate native handling
                                        Console.WriteLine("bag full! cannot put item in there");
                                        return;
                                }
                        }
                        else
                        {
                                slot--;
                        }

                        Game.Player.Inventory.SetItem(bag, page, slot);*/
                }

                private void EquipBagHandler(List<object> objects)
                {
                        /*Item bag;
                        if (!IdManager.TryGet((uint) objects[1], out bag))
                        {
                                Debug.ThrowException(new ArgumentException());
                        }

                        var slot = (byte) objects[2];

                        if (slot == 0)
                        {
                                // first free slot (gets used when item is drawn directly on a bag)

                                if (!Game.Player.Inventory.TryGetFreeBagSlot(out slot))
                                {
                                        // TODO: add apropriate native handling
                                        Console.WriteLine("no empty bag slot left");
                                        return;
                                }
                        }
                        else
                        {
                                slot--;
                        }

                        Game.Player.Inventory.SetBag(bag, slot);*/
                }

                private void MoveItemHandler(List<object> objects)
                {
                        /*Item item;
                        if (!IdManager.TryGet((uint) objects[1], out item))
                        {
                                Debug.ThrowException(new ArgumentException("unknown item id"));
                        }

                        InventoryPage page;
                        if (!IdManager.TryGet((ushort) objects[2], out page))
                        {
                                Debug.ThrowException(new ArgumentException("unknown page id"));
                        }

                        MoveItem(item, page, (byte) objects[3]);*/
                }

                private void MoveBagHandler(List<object> objects)
                {
                        /*Bag bagToMove = Game.Player.Inventory.GetBag((byte) objects[1]);

                        if (bagToMove == null)
                        {
                                Debug.ThrowException(new ArgumentException("bag to move must exist"));
                        }

                        MoveBag(bagToMove, (byte) objects[2]);*/
                }
        }
}