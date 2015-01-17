#region

using System;
using GuildWarsInterface.Datastructures.Agents.Components.Base;
using GuildWarsInterface.Declarations;

#endregion

namespace GuildWarsInterface.Datastructures.Agents.Components
{
        public sealed class CreatureEnergy : AgentBasicResource
        {
                private readonly Creature _creature;

                public CreatureEnergy(Creature creature)
                {
                        _creature = creature;
                        Current = 0;
                        Maximum = 1;
                }

                protected override void OnRegenerationChanged()
                {
                        if (_creature.Created)
                        {
                                _creature.SendAgentPropertyFloat(AgentProperty.EnergyRegeneration, Regeneration);
                        }
                }

                protected override void OnCurrentChanged()
                {
                        if (_creature.Created)
                        {
                                _creature.SendAgentPropertyFloat(AgentProperty.CurrentEnergy, Current / (float) Maximum);
                        }
                }

                protected override void OnMaximumChanged()
                {
                        if (_creature.Created)
                        {
                                _creature.SendAgentPropertyInt(AgentProperty.MaximumEnergy, Maximum);
                        }
                }

                protected override void OnModify(float change, Agent source, bool showFloaters = true)
                {
                        throw new NotImplementedException();
                }
        }
}