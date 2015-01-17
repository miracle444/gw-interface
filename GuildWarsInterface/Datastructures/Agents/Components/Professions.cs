#region

using GuildWarsInterface.Declarations;

#endregion

namespace GuildWarsInterface.Datastructures.Agents.Components
{
        public sealed class Professions
        {
                public Professions(Profession primary, Profession secondary)
                {
                        Secondary = secondary;
                        Primary = primary;
                }

                public Profession Primary { get; private set; }
                public Profession Secondary { get; private set; }

                public override string ToString()
                {
                        return string.Format("{0}/{1}", Primary, Secondary);
                }
        }
}