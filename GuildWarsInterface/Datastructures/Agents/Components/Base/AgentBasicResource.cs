#region

using System;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Agents.Components.Base
{
        public abstract class AgentBasicResource
        {
                private const uint MAX_VALUE = 0x00400000 - 1;

                private float _current;
                private uint _maximum;
                private float _regeneration;

                public float Regeneration
                {
                        get { return _regeneration; }
                        set
                        {
                                _regeneration = value;

                                OnRegenerationChanged();
                        }
                }

                public uint Current
                {
                        get { return (uint) Math.Round(_current * Maximum); }
                        set
                        {
                                if (value > MAX_VALUE)
                                {
                                        _current = 0;
                                }
                                else if (value > Maximum)
                                {
                                        _current = 1;
                                }
                                else
                                {
                                        _current = value / (float) Maximum;
                                }

                                OnCurrentChanged();
                        }
                }

                public uint Maximum
                {
                        get { return _maximum; }
                        set
                        {
                                _maximum = value;

                                OnMaximumChanged();
                        }
                }

                public void Increase(uint amount, Agent source, Skill skill = Skill.None, bool showFloaters = true)
                {
                        Current += amount;
                        Network.GameServer.Send(GameServerMessage.AgentPropertyInt, (uint) AgentProperty.SkillDamage, 0, (uint) skill);
                        OnModify(amount / (float) Maximum, source, showFloaters);
                }

                public void Decrease(uint amount, Agent source, Skill skill = Skill.None, bool showFloaters = true)
                {
                        Current -= amount;
                        Network.GameServer.Send(GameServerMessage.AgentPropertyInt, (uint) AgentProperty.SkillDamage, 0, (uint) skill);
                        OnModify(-(amount / (float) Maximum), source, showFloaters);
                }

                protected abstract void OnRegenerationChanged();
                protected abstract void OnCurrentChanged();

                protected abstract void OnMaximumChanged();
                protected abstract void OnModify(float change, Agent source, bool showFloaters = true);
        }
}