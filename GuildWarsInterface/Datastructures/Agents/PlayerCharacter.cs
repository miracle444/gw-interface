#region

using System;
using System.IO;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Datastructures.Items;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Agents
{
        public sealed class PlayerCharacter : Creature
        {
                private PlayerAppearance _appearance;

                public PlayerCharacter()
                {
                        _appearance = new PlayerAppearance(1, 9, 4, 0, 1, 3, 4, 0);
                }

                public PlayerAppearance Appearance
                {
                        get { return _appearance; }
                        set
                        {
                                _appearance = value;
                                OnAppearanceChanged();
                        }
                }

                private void OnAppearanceChanged()
                {
                        if (Game.State == GameState.Playing && Created)
                        {
                                UpdateAppearance();
                                RefreshAppearance();
                                Professions = Professions;
                        }
                }

                protected override void OnNameChanged()
                {
                        OnAppearanceChanged();

                        if (Game.State == GameState.Playing && Created)
                        {
                                Game.Zone.RefreshParties();
                        }
                }

                internal byte[] GetLoginScreenAppearance()
                {
                        var stream = new MemoryStream();

                        const ushort PACKING_VERSION = 6;
                        stream.Write(BitConverter.GetBytes(PACKING_VERSION), 0, 2);

                        var lastOutpost = (ushort) Map.HeroesAscent;
                        stream.Write(BitConverter.GetBytes(lastOutpost), 0, 2);

                        var unknown1 = new byte[]
                                {
                                        0x37, 0x38, 0x31, 0x30 // creation time?
                                };
                        stream.Write(unknown1, 0, unknown1.Length);

                        byte[] appearance = BitConverter.GetBytes(Appearance.GetPackedValue());
                        stream.Write(appearance, 0, 4);

                        var guildHall = new byte[16];
                        stream.Write(guildHall, 0, guildHall.Length);

                        var campaign = Campaign.Factions;
                        bool isPvp = true;
                        bool showHelm = true;
                        var properties = (ushort) ((byte) campaign | //4 bits
                                                   (Level << 4) | // 5 bits
                                                   (isPvp ? 1 : 0) << 9 | // 1 bit
                                                   ((byte) Professions.Secondary << 10) | // 4 bits
                                                   (showHelm ? 1 : 0) << 14); // 1 bit
                        // 1 bit left
                        stream.Write(BitConverter.GetBytes(properties), 0, 2);

                        var unknown2 = new byte[]
                                {
                                        0xDD, 0xDD, // ?
                                        0x05, // # equipment pieces
                                        0xDD, 0xDD, 0xDD, 0xDD // ?
                                };
                        stream.Write(unknown2, 0, unknown2.Length);

                        stream.Write(GetLoginAppearanceItemSerialized(EquipmentSlot.Head), 0, 4);
                        stream.WriteByte(1);

                        stream.Write(GetLoginAppearanceItemSerialized(EquipmentSlot.Chest), 0, 4);
                        stream.WriteByte(1);

                        stream.Write(GetLoginAppearanceItemSerialized(EquipmentSlot.Arms), 0, 4);
                        stream.WriteByte(1);

                        stream.Write(GetLoginAppearanceItemSerialized(EquipmentSlot.Legs), 0, 4);
                        stream.WriteByte(1);

                        stream.Write(GetLoginAppearanceItemSerialized(EquipmentSlot.Feet), 0, 4);
                        stream.WriteByte(1);

                        return stream.ToArray();
                }

                private byte[] GetLoginAppearanceItemSerialized(EquipmentSlot equipmentSlot)
                {
                        Item item;
                        if (Inventory.Equipment.TryGet(equipmentSlot, out item))
                        {
                                return BitConverter.GetBytes(item._model);
                        }

                        return new byte[4];
                }

                private void RefreshAppearance()
                {
                        Network.GameServer.Send(GameServerMessage.RefreshAgentAppearance, IdManager.GetId(this));
                }

                private void UpdateAppearance()
                {
                        Network.GameServer.Send(GameServerMessage.UpdateAgentAppearance,
                                                IdManager.GetId(this),
                                                IdManager.GetId(this),
                                                Appearance.GetPackedValue(),
                                                (byte) 0,
                                                0x800,
                                                0,
                                                Name);
                }

                internal void UpdateFullEquipment()
                {
                        Equipment equipment = Inventory.Equipment;

                        Network.GameServer.Send(GameServerMessage.UpdateFullEquipment,
                                                IdManager.GetId(this),
                                                equipment.Get(EquipmentSlot.Mainhand) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.Mainhand)) : 0,
                                                equipment.Get(EquipmentSlot.Offhand) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.Offhand)) : 0,
                                                equipment.Get(EquipmentSlot.Chest) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.Chest)) : 0,
                                                equipment.Get(EquipmentSlot.Feet) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.Feet)) : 0,
                                                equipment.Get(EquipmentSlot.Legs) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.Legs)) : 0,
                                                equipment.Get(EquipmentSlot.Arms) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.Arms)) : 0,
                                                equipment.Get(EquipmentSlot.Head) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.Head)) : 0,
                                                equipment.Get(EquipmentSlot.Costume) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.Costume)) : 0,
                                                equipment.Get(EquipmentSlot.CostumeHead) != null ?
                                                        IdManager.GetId(equipment.Get(EquipmentSlot.CostumeHead)) : 0);
                }

                protected override void OnCreation()
                {
                        UpdateAppearance();
                        Professions = Professions;
                        Level = Level;
                        Status = Status;
                        Health.Maximum = Health.Maximum;
                        Energy.Maximum = Energy.Maximum;
                        Health.Current = Health.Current;
                        Energy.Current = Energy.Current;
                        Spawn(0x30000000);
                        Transformation.Orientation = Transformation.Orientation;
                        UpdateFullEquipment();
                        Morale = Morale;
                }
        }
}