#region

using GuildWarsInterface.Datastructures.Agents.Components.Base;
using GuildWarsInterface.Declarations;

#endregion

namespace GuildWarsInterface.Datastructures.Agents.Components
{
        public sealed class CreatureHealth : AgentBasicResource
        {
                private readonly Creature _creature;

                public CreatureHealth(Creature creature)
                {
                        _creature = creature;
                        Current = 1;
                        Maximum = 1;
                }

                protected override void OnRegenerationChanged()
                {
                        if (_creature.Created)
                        {
                                _creature.SendAgentPropertyFloat(AgentProperty.HealthRegeneration, Regeneration);
                        }
                }

                protected override void OnCurrentChanged()
                {
                        if (_creature.Created)
                        {
                                _creature.SendAgentPropertyFloat(AgentProperty.CurrentHealth, Current / (float) Maximum);
                        }
                }

                protected override void OnMaximumChanged()
                {
                        if (_creature.Created)
                        {
                                _creature.SendAgentPropertyInt(AgentProperty.MaximumHealth, Maximum);
                        }
                }

                protected override void OnModify(float change, Agent source, bool showFloaters = true)
                {
                        source.SendAgentTargetPropertyFloat(showFloaters ? AgentProperty.ModifyHealthWithFloaters : AgentProperty.ModifyHealthWithoutFloaters, _creature, change);
                }
        }
}