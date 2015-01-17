#region

using GuildWarsInterface.Declarations;

#endregion

namespace GuildWarsInterface.Datastructures.Components
{
        public class ItemStat
        {
                private readonly uint _serialized;

                public ItemStat(ItemStatIdentifier identifier, byte param1, byte param2)
                {
                        Identifier = identifier;
                        Parameter1 = param1;
                        Parameter2 = param2;

                        _serialized = (uint) (((ushort) Identifier << 16) | (Parameter1 << 8) | Parameter2);
                }

                public ItemStat(uint serialized)
                {
                        _serialized = serialized;
                }

                public ItemStatIdentifier Identifier { get; private set; }
                public byte Parameter1 { get; private set; }
                public byte Parameter2 { get; private set; }

                public uint Serialize()
                {
                        return _serialized;
                }
        }
}