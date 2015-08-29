#region

using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Player
{
        public sealed class Account
        {
                public enum Currency
                {
                        MakeoverCredit = 101,
                        ExtremeMakeoverCredit = 102

                        // value must be < 131
                }

                private readonly List<PlayerCharacter> _characters;

                internal Account()
                {
                        _characters = new List<PlayerCharacter>();
                }

                public IEnumerable<PlayerCharacter> Characters
                {
                        get { return _characters.ToArray(); }
                }

                public void AddCharacter(PlayerCharacter character)
                {
                        _characters.Add(character);

                        if (Game.State != GameState.Handshake && Game.State != GameState.LoginScreen)
                        {
                                Network.AuthServer.Send(AuthServerMessage.Character,
                                                        Network.AuthServer.LoginCount,
                                                        new byte[16],
                                                        0,
                                                        character.Name,
                                                        character.GetLoginScreenAppearance());
                        }
                }

                public void ClearCharacters()
                {
                        _characters.ToList().ForEach(RemoveCharacter);
                }

                internal void RemoveCharacter(PlayerCharacter character)
                {
                        _characters.Remove(character);
                }

                internal void SendCharacters()
                {
                        foreach (PlayerCharacter character in Characters)
                        {
                                Network.AuthServer.Send(AuthServerMessage.Character,
                                                        Network.AuthServer.LoginCount,
                                                        new byte[16],
                                                        0,
                                                        character.Name,
                                                        character.GetLoginScreenAppearance());
                        }
                }

                public void SetCurrency(Currency currency, ushort total, ushort used)
                {
                        Network.GameServer.Send(GameServerMessage.AccountCurrency, (ushort) currency, total, used);
                }
        }
}