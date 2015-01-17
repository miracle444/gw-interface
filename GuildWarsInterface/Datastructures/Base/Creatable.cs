#region

using GuildWarsInterface.Debugging;

#endregion

namespace GuildWarsInterface.Datastructures.Base
{
        public abstract class Creatable
        {
                protected Zone ParentZone;

                public bool Created
                {
                        get { return ParentZone != null && ParentZone == Game.Zone; }
                }

                internal void Create()
                {
                        Debug.Requires(!Created);

                        ParentZone = Game.Zone;

                        OnCreation();
                }

                protected abstract void OnCreation();

                internal void Destroy()
                {
                        Debug.Requires(Created);

                        OnDestruction();

                        ParentZone = null;
                }

                protected abstract void OnDestruction();
        }
}