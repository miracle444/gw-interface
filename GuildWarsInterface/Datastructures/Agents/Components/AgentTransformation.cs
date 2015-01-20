#region

using System;
using System.Runtime.InteropServices;
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

                public static MovementType MovementType = MovementType.Stop;
                internal static IntPtr Tracker = IntPtr.Zero;

                public void Move(float x, float y, short plane, float speedModifier, MovementType type)
                {
                        if (Game.State == GameState.Playing)
                        {
                                Network.GameServer.Send((GameServerMessage) 32,
                                                        IdManager.GetId(_agent),
                                                        speedModifier,
                                                        (byte) type);

                                Network.GameServer.Send((GameServerMessage) 30,
                                                        IdManager.GetId(_agent),
                                                        x,
                                                        y,
                                                        (ushort) plane,
                                                        (ushort) plane);
                        }
                }

                public event Action Changed;

                public float Speed
                {
                        get
                        {
                                 var mx = _agentClientMemory.ClientMemoryMoveX;
                        var my = _agentClientMemory.ClientMemoryMoveY;

                        return (float) Math.Sqrt(mx * mx + my * my);
                        }
                }

                public short Plane
                {
                        get { return _agentClientMemory.ClientMemoryPlane; }
                }

                public static float GoalX
                {
                        get { return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(Tracker + 8)), 0); }
                }

                public static float GoalY
                {
                        get { return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(Tracker + 12)), 0); }
                }

                public static event Action GoalChanged;

                public static void OnGoalChanged()
                {
                        if (GoalChanged != null) GoalChanged();
                }

                public void OnChanged()
                {
                        if (Changed != null) Changed();
                }
        }
}