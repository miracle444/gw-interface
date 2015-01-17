#region

using System.Collections.Generic;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Declarations;

#endregion

namespace GuildWarsInterface.Datastructures
{
        public sealed class Zone
        {
                private readonly List<Creature> _agents;
                private readonly List<Party> _parties;

                internal Zone(Map map)
                {
                        Map = map;

                        IsExplorable = false;

                        _agents = new List<Creature>();

                        _parties = new List<Party>();
                }

                public Map Map { get; private set; }

                public IEnumerable<Creature> Agents
                {
                        get { return _agents.ToArray(); }
                }

                public IEnumerable<Party> Parties
                {
                        get { return _parties.ToArray(); }
                }

                public bool IsExplorable { get; private set; }

                public bool Loaded { get; internal set; }

                internal void RefreshParties()
                {
                        var parties = new List<Party>(_parties);

                        foreach (Party party in parties)
                        {
                                RemoveParty(party);
                                AddParty(party);
                        }
                }

                public void AddAgent(Creature creature)
                {
                        _agents.Add(creature);

                        if (Game.State == GameState.Playing)
                        {
                                creature.Create();
                        }
                }

                public void RemoveAgent(Creature creature)
                {
                        _agents.Remove(creature);

                        if (creature.Created)
                        {
                                creature.Destroy();
                        }
                }

                internal void SpawnAgents()
                {
                        foreach (Creature agent in _agents)
                        {
                                if (Game.Player.Character == agent) continue;

                                agent.Create();
                        }
                }

                public void AddParty(Party party)
                {
                        _parties.Add(party);

                        if (Game.State == GameState.Playing)
                        {
                                party.Create();

                                party.SendPreActiveInformation();
                        }
                }

                public void RemoveParty(Party party)
                {
                        _parties.Remove(party);

                        if (Game.State == GameState.Playing)
                        {
                                party.Destroy();
                        }
                }

                internal void CreateParties()
                {
                        foreach (Party party in _parties)
                        {
                                party.Create();
                        }

                        foreach (Party party in _parties)
                        {
                                party.SendPreActiveInformation();
                        }
                }
        }
}