using System;

namespace GuildWarsInterface.Datastructures.Agents.Components
{
        public class Position : IEquatable<Position>
        {
                public Position(float x, float y, short plane)
                {
                        X = x;
                        Y = y;
                        Plane = plane;
                }

                public float X { get; private set; }
                public float Y { get; private set; }
                public short Plane { get; private set; }

                public bool Equals(Position other)
                {
                        if (ReferenceEquals(null, other)) return false;
                        if (ReferenceEquals(this, other)) return true;
                        return X.Equals(other.X) && Y.Equals(other.Y) && Plane == other.Plane;
                }

                public override int GetHashCode()
                {
                        unchecked
                        {
                                int hashCode = X.GetHashCode();
                                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                                hashCode = (hashCode * 397) ^ Plane.GetHashCode();
                                return hashCode;
                        }
                }

                public static bool operator ==(Position left, Position right)
                {
                        return Equals(left, right);
                }

                public static bool operator !=(Position left, Position right)
                {
                        return !Equals(left, right);
                }

                public override bool Equals(object other)
                {
                        if (ReferenceEquals(null, other)) return false;
                        if (ReferenceEquals(this, other)) return true;
                        if (other.GetType() != GetType()) return false;
                        return Equals((Position) other);
                }

                public float DistanceTo(Position other)
                {
                        float dx = X - other.X;
                        float dy = Y - other.Y;

                        return dx * dx + dy * dy;
                }
        }
}