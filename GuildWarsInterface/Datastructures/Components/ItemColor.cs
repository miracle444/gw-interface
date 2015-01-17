#region

using GuildWarsInterface.Declarations;

#endregion

namespace GuildWarsInterface.Datastructures.Components
{
        public sealed class ItemColor
        {
                private readonly Dye _color1;
                private readonly Dye _color2;
                private readonly Dye _color3;
                private readonly Dye _color4;

                public ItemColor(Dye color1, Dye color2, Dye color3, Dye color4)
                {
                        _color1 = color1;
                        _color2 = color2;
                        _color3 = color3;
                        _color4 = color4;
                }

                public ItemColor(Dye color1)
                {
                        _color1 = color1;
                }

                public ItemColor()
                {
                }

                public ushort Serialize()
                {
                        var result = (ushort) _color1;

                        result |= (byte) ((byte) _color2 << 4);
                        result |= (byte) ((byte) _color3 << 8);
                        result |= (byte) ((byte) _color4 << 12);

                        return result;
                }
        }
}