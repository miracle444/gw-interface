using System;
using GuildWarsInterface.Datastructures;
using GuildWarsInterface.Declarations;

namespace GuildWarsInterface.Logic
{
        public static class AuthLogic
        {
                public delegate bool LoginLogicHandler(string email, string password, string character);

                public delegate void PlayHandler(Map map);

                public static LoginLogicHandler Login = (email, password, character) => true;

                public static Action CancelLogin = () => { };

                public static PlayHandler Play = (map) => Game.ChangeMap(map, zone =>
                        {
                                zone.AddAgent(Game.Player.Character);
                                zone.AddParty(new Party(Game.Player.Character));
                                Game.Player.Character.Transformation.Position = MapData.GetDefaultSpawnPoint(zone.Map);
                        });

                public static Action Logout = () => { };
        }
}