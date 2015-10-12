using System;
using GuildWarsInterface.Datastructures;
using GuildWarsInterface.Datastructures.Components;
using GuildWarsInterface.Declarations;

namespace GuildWarsInterface.Logic
{
        public static class AuthLogic
        {
                public delegate bool LoginLogicHandler(string email, string password, string character);

                public delegate void PlayHandler(Map map);

                public delegate bool AddFriendHandler(FriendList.Type type, string baseCharacterName, string currentCharacterName);

                public delegate bool MoveFriendHandler(string baseCharacterName, FriendList.Type target);

                public static LoginLogicHandler Login = (email, password, character) => true;

                public static Action CancelLogin = () => { };

                public static PlayHandler Play = (map) => Game.ChangeMap(map, zone =>
                        {
                                zone.AddAgent(Game.Player.Character);
                                zone.AddParty(new Party(Game.Player.Character));
                                Game.Player.Character.Transformation.Position = MapData.GetDefaultSpawnPoint(zone.Map);
                        });

                public static Action Logout = () => { };

                public static AddFriendHandler AddFriend = (type, baseCharacterName, currentCharacterName) =>
                        {
                                Game.Player.FriendList.Add(type, baseCharacterName, currentCharacterName);
                                return true;
                        };

                public static MoveFriendHandler MoveFriend = (name, target) =>
                        {
                                Game.Player.FriendList.Move(name, target);
                                return true;
                        };
        }
}