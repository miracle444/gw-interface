using System;
using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Datastructures.Components;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

namespace GuildWarsInterface.Datastructures.Items
{
        public sealed class Item : Agent
        {
                private readonly ItemColor _color;
                private readonly ItemFlags _flags;
                internal readonly uint _model;
                private readonly IEnumerable<ItemStat> _stats;
                private readonly ItemType _type;

                public Item(ItemType type, uint model, string name, ItemFlags flags, ItemColor color, IEnumerable<ItemStat> stats)
                {
                        _type = type;
                        _model = model;
                        _color = color;
                        Name = name;
                        _flags = flags;
                        _stats = stats.ToArray();
                }

                protected override void OnCreation()
                {
                        Network.GameServer.Send(GameServerMessage.CreateItem,
                                                IdManager.GetId(this),
                                                _model,
                                                (byte) _type,
                                                (byte) 1,
                                                _color.Serialize(),
                                                (ushort) 0,
                                                (byte) 0,
                                                (uint) _flags,
                                                0,
                                                0,
                                                1,
                                                new HString(Name).Serialize(),
                                                _stats.Select(stat => stat.Serialize()).ToArray());
                }

                protected override void OnNameChanged()
                {
                        // TODO
                }

                protected override void OnDestruction()
                {
                        Debug.ThrowException(new Exception("cannot unload item"));
                }

                public Item Clone()
                {
                        return new Item(_type, _model, Name, _flags, _color, _stats);
                }

                public override string ToString()
                {
                        return string.Format("[Item] {0}", Name);
                }
        }
}