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
                private MovementType _cacheType = MovementType.Stop;
                private float _cacheVelocity = 1F;
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

                public event Action GoalChanged;
                public event Action MovementTypeChanged;

                public void SetGoal(float x, float y, ushort plane)
                {
                        if (Game.State == GameState.Playing)
                        {
                                Network.GameServer.Send((GameServerMessage) 32,
                                                        IdManager.GetId(_agent),
                                                        _cacheVelocity,
                                                        (byte) _cacheType);
                                Console.WriteLine("velocity: {0}, type: {1}", _cacheType, _cacheType);

                                Network.GameServer.Send((GameServerMessage) 30,
                                                        IdManager.GetId(_agent),
                                                        x,
                                                        y,
                                                        plane,
                                                        plane);

                                Console.WriteLine("x: {0}, y: {1}, plane: {2}", x, y, plane);
                        }
                }

                public void Move(Position goal, float speedModifier, MovementType movementType)
                {
                        Network.GameServer.Send((GameServerMessage) 32,
                                                IdManager.GetId(_agent),
                                                speedModifier,
                                                (byte) movementType);

                        Network.GameServer.Send((GameServerMessage) 30,
                                                IdManager.GetId(_agent),
                                                goal.X,
                                                goal.Y,
                                                goal.Plane,
                                                goal.Plane);
                }
        }
}