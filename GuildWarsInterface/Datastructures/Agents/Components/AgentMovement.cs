namespace GuildWarsInterface.Datastructures.Agents.Components
{
        public sealed class AgentMovement
        {
                public AgentMovement(float[] goal, MovementState state)
                {
                        Goal = goal;
                        State = state;
                }

                public float[] Goal { get; private set; }
                public MovementState State { get; private set; }
        }

        public enum MovementState
        {
                NotMoving,
                Moving
        }
}