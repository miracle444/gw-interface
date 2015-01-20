#region

using System;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Debugging;

#endregion

namespace GuildWarsInterface.Declarations
{
        public static class MapData
        {
                public static float[] GetMapCoordinates(Map map)
                {
                        switch (map)
                        {
                                case Map.RandomArenas:
                                        return new[] {1917F, 3041F};
                                case Map.TeamArenas:
                                        return new[] {-1873F, 352F};
                                case Map.HeroesAscent:
                                        return new[] {2017F, -3241F};
                                case Map.AscalonCity:
                                        return new[] {6565F, 4578F};
                                case Map.PresearingAscalonCity:
                                        return new[] {9487F, 4758F};
                                case Map.RiversideProvince:
                                        return new[] {-22544F, 8059F};
                                case Map.LakesideCounty:
                                        return new[] {6602F, 4485F};
                                case Map.DAllessioArena:
                                        return new[] {-3331F, -5193F};
                                default:
                                        Debug.ThrowException(new IndexOutOfRangeException("map data unknown for map: " + map));
                                        return null;
                        }
                }

                public static Position GetDefaultSpawnPoint(Map map)
                {
                        switch (map)
                        {
                                case Map.RandomArenas:
                                        return new Position(3854F, 3874F, 0);
                                case Map.TeamArenas:
                                        return new Position(-1873F, 352F, 0);
                                case Map.HeroesAscent:
                                        return new Position(2017F, -3241F, 0);
                                case Map.PresearingAscalonCity:
                                        return new Position(9487F, 4758F, 0);
                                case Map.AscalonCity:
                                        return new Position(6565F, 4578F, 0);
                                case Map.RiversideProvince:
                                        return new Position(-22544F, 8059F, 0);
                                case Map.LakesideCounty:
                                        return new Position(6602F, 4485F, 0);
                                case Map.DAllessioArena:
                                        return new Position(-3331F, -5193F, 0);
                                default:
                                        Debug.ThrowException(new IndexOutOfRangeException("map data unknown for map: " + map));
                                        return null;
                        }
                }

                public static uint GetMapFile(Map map)
                {
                        switch (map)
                        {
                                case Map.RandomArenas:
                                        return 0x284DB;
                                case Map.TeamArenas:
                                        return 0x28538;
                                case Map.HeroesAscent:
                                        return 0x2879C;
                                case Map.PresearingAscalonCity:
                                case Map.LakesideCounty:
                                        return 0x1b97d;
                                case Map.AscalonCity:
                                        return 0x5624;
                                case Map.RiversideProvince:
                                        return 0x29ba;
                                case Map.DAllessioArena:
                                        return 0x25A0B;
                                default:
                                        Debug.ThrowException(new IndexOutOfRangeException("map data unknown for map: " + map));
                                        return 0;
                        }
                }
        }
}