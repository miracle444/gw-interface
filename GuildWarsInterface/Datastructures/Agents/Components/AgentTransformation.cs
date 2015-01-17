#region

using System;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Agents.Components
{
        public sealed class AgentTransformation
        {
                private readonly Agent _agent;
                private readonly AgentClientMemory _agentClientMemory;
                private AgentMovement _movement;
                private float _orientation;

                public AgentTransformation(Agent agent)
                {
                        _agent = agent;
                        _agentClientMemory = new AgentClientMemory(agent);

                        Position = new float[2];
                        Movement = new AgentMovement(Position, MovementState.NotMoving);
                }

                internal float[] ExplicitPosition { get; private set; }

                public float[] Position
                {
                        get
                        {
                                if (Game.State == GameState.Playing && _agent.Created)
                                {
                                        return new[] {_agentClientMemory.ClientMemoryX, _agentClientMemory.ClientMemoryY};
                                }

                                return ExplicitPosition;
                        }
                        set
                        {
                                ExplicitPosition = value;

                                if (Game.State == GameState.Playing && _agent.Created)
                                {
                                        Network.GameServer.Send(GameServerMessage.UpdateAgentPosition, IdManager.GetId(_agent), ExplicitPosition[0], ExplicitPosition[1], (ushort) 0);
                                }
                        }
                }

                public float Orientation
                {
                        get { return _orientation; }
                        set
                        {
                                _orientation = value;

                                if (Game.State == GameState.Playing && _agent.Created)
                                {
                                        Network.GameServer.Send(GameServerMessage.AgentRotation, IdManager.GetId(_agent), Orientation, 5);
                                }
                        }
                }

                private AgentMovement Movement
                {
                        get { return _movement; }
                        set
                        {
                                _movement = value;

                                if (Game.State == GameState.Playing && _agent.Created)
                                {
                                        if (_movement.State == MovementState.Moving)
                                        {
                                                Console.WriteLine("agent started moving");

                                                Network.GameServer.Send(GameServerMessage.MovementSpeed, IdManager.GetId(_agent), (float) 1, (byte) 1);

                                                Network.GameServer.Send(GameServerMessage.Move, IdManager.GetId(_agent), Movement.Goal[0], Movement.Goal[1], (ushort) 0, (ushort) 0);
                                        }
                                        else
                                        {
                                                Console.WriteLine("agent stopped moving");

                                                Network.GameServer.Send(GameServerMessage.MovementSpeed, IdManager.GetId(_agent), (float) 1, (byte) 9);

                                                Network.GameServer.Send(GameServerMessage.Move, IdManager.GetId(_agent), Movement.Goal[0], Movement.Goal[1], (ushort) 0, (ushort) 0);
                                        }
                                }
                        }
                }
        }
}