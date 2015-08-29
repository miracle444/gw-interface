using System;
using System.Collections.Generic;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

namespace GuildWarsInterface.Datastructures.Agents.Components
{
        public sealed class CreatureBuffs
        {
                private readonly Dictionary<uint, Buff> _buffs;
                private readonly Creature _creature;
                private uint _nextBuffId;

                public CreatureBuffs(Creature creature)
                {
                        _creature = creature;

                        _buffs = new Dictionary<uint, Buff>();
                }

                public IEnumerable<Buff> Buffs
                {
                        get
                        {
                                lock (this)
                                {
                                        return _buffs.Values;
                                }
                        }
                }

                public TimedBuff AddTimedBuff(Skill skill, float duration)
                {
                        return CreateBuff(id => new TimedBuff(_creature, id, skill, duration));
                }

                private T CreateBuff<T>(Func<uint, T> constructor) where T : Buff
                {
                        lock (this)
                        {
                                T result = constructor(_nextBuffId++);
                                _buffs.Add(result.Id, result);
                                result.Show();

                                return result;
                        }
                }

                private void RemoveBuff(Buff buff)
                {
                        lock (this)
                        {
                                if (_buffs.ContainsKey(buff.Id))
                                {
                                        _buffs.Remove(buff.Id);
                                        buff.Remove();
                                }
                        }
                }

                public abstract class Buff
                {
                        protected readonly Creature Creature;

                        internal readonly uint Id;
                        public readonly Skill Skill;

                        internal Buff(Creature creature, uint id, Skill skill)
                        {
                                Id = id;
                                Skill = skill;
                                Creature = creature;
                        }

                        internal abstract void Show();

                        internal void Remove()
                        {
                                Network.GameServer.Send((GameServerMessage) 56,
                                                        IdManager.GetId(Creature),
                                                        Id);
                        }
                }

                public sealed class TimedBuff : Buff
                {
                        public readonly float Duration;

                        public TimedBuff(Creature creature, uint id, Skill skill, float duration) : base(creature, id, skill)
                        {
                                Duration = duration;
                        }

                        internal override void Show()
                        {
                                Network.GameServer.Send((GameServerMessage) 54,
                                                        IdManager.GetId(Creature),
                                                        (ushort) Skill,
                                                        0,
                                                        Id,
                                                        Duration);
                        }
                }
        }
}