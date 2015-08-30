using GuildWarsInterface.Datastructures.Const;

namespace GuildWarsInterface.Declarations
{
        public enum Map
        {
                RandomArenas = 188,
                TeamArenas = 189,
                HeroesAscent = 330,
                AscalonCity = 81,
                PresearingAscalonCity = 148,
                RiversideProvince = 73,
                LakesideCounty = 146,
                DAllessioArena = 187,
                GreatTempleOfBalthazar = 248,

                Count = 877
        }

        public static class MapExtension
        {
                public static AreaInfo Info(this Map map)
                {
                        return new AreaInfo(map);
                }
        }
}