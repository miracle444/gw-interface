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
                private Position _goal;
                private MovementType _movementType;
                private float _orientation;

                private Position _position;
                private float _speed;

                public AgentTransformation(Agent agent)
                {
                        _agent = agent;
                        _position = new Position(0, 0, 0);
                        _goal = _position;
                }

                public Position Position
                {
                        get { return _position; }
                        set
                        {
                                Position oldPosition = Position;
                                _position = value;

                                if (Position.DistanceTo(oldPosition) > 100000)
                                {
                                        if (Game.State == GameState.Playing && _agent.Created)
                                        {
                                                Network.GameServer.Send(GameServerMessage.UpdateAgentPosition, IdManager.GetId(_agent), Position.X, Position.Y, Position.Plane);
                                        }
                                }

                                if (Position.Plane != oldPosition.Plane)
                                {
                                        if (PlaneChanged != null) PlaneChanged();
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

                public Position Goal
                {
                        get { return _goal; }
                        set
                        {
                                if (_goal != value)
                                {
                                        _goal = value;

                                        if (GoalChanged != null) GoalChanged();
                                }
                        }
                }

                public float Speed
                {
                        get { return _speed; }
                        set
                        {
                                if (Math.Abs(_speed - value) > 0.1D)
                                {
                                        _speed = value;
                                        if (SpeedChanged != null) SpeedChanged();
                                }
                        }
                }

                public MovementType MovementType
                {
                        get { return _movementType; }
                        set
                        {
                                if (_movementType != value)
                                {
                                        _movementType = value;
                                        if (MovementTypeChanged != null) MovementTypeChanged();
                                }
                        }
                }

                public float SpeedModifier
                {
                        get
                        {
                                float speedModifier = Game.Player.Character.Transformation.Speed / Game.Player.Character.Speed;
                                return speedModifier > 0 ? Math.Max(0.01F, Math.Min(1F, speedModifier)) : 1;
                        }
                }

                public event Action GoalChanged;
                public event Action SpeedChanged;
                public event Action PlaneChanged;
                public event Action MovementTypeChanged;

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
        }
}