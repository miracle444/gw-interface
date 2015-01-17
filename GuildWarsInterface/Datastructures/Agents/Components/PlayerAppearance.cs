#region

#endregion

namespace GuildWarsInterface.Datastructures.Agents.Components
{

        #region value ranges

        /*
        Height: 0-15 (small <-- 8-15 -- middle -- 0-7 --> tall)

        In the following X/Y means there exist 0-X for male and 0-Y for female:

        Warrior:

        Faces: 23/30
        HairColors: 23/23
        Hairstyles: 25/28
        SkinColors: 21/21


        Ranger:

        Faces: 25/27
        HairColors: 26/26
        Hairstyles: 26/28
        SkinColors: 20/20


        Monk:

        Faces: 24/25
        HairColors: 23/23
        Hairstyles: 27/25
        SkinColors: 19/19


        Necromancer:

        Faces: 25/29
        HairColors: 27/27
        Hairstyles: 26/31
        SkinColors: 19/19


        Mesmer:

        Faces: 26/27
        HairColors: 29/29
        Hairstyles: 24/28
        SkinColors: 17/17


        Elementalist:

        Faces: 25/27
        HairColors: 29/29
        Hairstyles: 26/27
        SkinColors: 21/21


        Assassin:

        Faces: 6/6
        HairColors: 6/6
        Hairstyles: 7/9
        SkinColors: 5/5


        Ritualist:

        Faces: 6/6
        HairColors: 6/6
        Hairstyles: 8/9
        SkinColors: 6/6


        Paragon:

        Faces: 9/8
        HairColors: 15/15
        Hairstyles: 10/12
        SkinColors: 7/7


        Dervish:

        Faces: 7/8
        HairColors: 15/15
        Hairstyles: 11/10
        SkinColors: 7/7
        */

        #endregion

        public sealed class PlayerAppearance
        {
                internal PlayerAppearance(uint packed)
                {
                        Sex = GetPackedValue(packed, 0, 1);
                        Height = GetPackedValue(packed, 1, 4);
                        SkinColor = GetPackedValue(packed, 5, 5);
                        HairColor = GetPackedValue(packed, 10, 5);
                        Face = GetPackedValue(packed, 15, 5);
                        Profession = GetPackedValue(packed, 20, 4);
                        Hairstyle = GetPackedValue(packed, 24, 5);
                        Campaign = GetPackedValue(packed, 29, 3);
                }

                public PlayerAppearance(uint sex, uint height, uint skinColor, uint hairColor, uint face, uint profession, uint hairstyle, uint campaign)
                {
                        Sex = sex;
                        Height = height;
                        SkinColor = skinColor;
                        HairColor = hairColor;
                        Face = face;
                        Profession = profession;
                        Hairstyle = hairstyle;
                        Campaign = campaign;
                }

                public uint Campaign { get; private set; }
                public uint Face { get; private set; }
                public uint HairColor { get; private set; }
                public uint Hairstyle { get; private set; }
                public uint Height { get; private set; }
                public uint Profession { get; internal set; }
                public uint Sex { get; private set; }
                public uint SkinColor { get; private set; }

                internal uint GetPackedValue()
                {
                        return PackValue(Sex, 0, 1) |
                               PackValue(Height, 1, 4) |
                               PackValue(SkinColor, 5, 5) |
                               PackValue(HairColor, 10, 5) |
                               PackValue(Face, 15, 5) |
                               PackValue(Profession, 20, 4) |
                               PackValue(Hairstyle, 24, 5) |
                               PackValue(Campaign, 29, 3);
                }

                private uint PackValue(uint value, int position, int length)
                {
                        uint mask = ~(0xFFFFFFFF << length);

                        uint maskedValue = value & mask;

                        return maskedValue << position;
                }

                private uint GetPackedValue(uint packed, int position, int length)
                {
                        uint mask = ~(0xFFFFFFFF << length);

                        uint shifted = packed >> position;

                        return shifted & mask;
                }

                public override string ToString()
                {
                        return string.Format("Campaign: {0}\n" +
                                             "Face: {1}\n" +
                                             "HairColor: {2}\n" +
                                             "Hairstyle: {3}\n" +
                                             "Height: {4}\n" +
                                             "Profession: {5}\n" +
                                             "Sex: {6}\n" +
                                             "SkinColor: {7}", Campaign, Face, HairColor, Hairstyle, Height, Profession, Sex, SkinColor);
                }
        }
}