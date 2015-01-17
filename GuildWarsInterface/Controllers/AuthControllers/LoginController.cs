#region

using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Logic;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Controllers.AuthControllers
{
        internal class LoginController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(18, LoginHandler18);
                }

                private void LoginHandler18(List<object> data)
                {
                        Network.AuthServer.LoginCount = (uint) data[1];

                        if (AuthLogic.Login((string) data[4], (string) data[5], (string) data[7]))
                        {
                                Game.State = GameState.CharacterScreen;

                                Game.Player.Account.SendCharacters();

                                Network.AuthServer.Send(AuthServerMessage.Gui, Network.AuthServer.LoginCount, (ushort) 0);
                                Network.AuthServer.Send(AuthServerMessage.FriendList, Network.AuthServer.LoginCount, 1);

                                Network.AuthServer.Send(AuthServerMessage.AccountPermissions,
                                                        Network.AuthServer.LoginCount,
                                                        2,
                                                        4,
                                                        new byte[] {0x38, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
                                                        new byte[] {0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0C, 0x00},
                                                        new byte[] {0x80, 0xDC, 0x06, 0xD7, 0xD0, 0x70, 0x04, 0x4A, 0x84, 0x60, 0xDB, 0x2B, 0xB6, 0x7D, 0x11, 0x72},
                                                        new byte[] {0x55, 0xF4, 0x6D, 0xC9, 0x7A, 0x04, 0x01, 0x49, 0xA8, 0x85, 0x4A, 0x0D, 0x78, 0x4B, 0xE5, 0x20},
                                                        8,
                                                        new byte[] {0x01, 0x00, 0x02, 0x00, 0x58, 0x00, 0x01, 0x00},
                                                        (byte) 23,
                                                        0);

                                Network.AuthServer.Send(AuthServerMessage.StreamTerminator, Network.AuthServer.LoginCount, 0);
                        }
                        else
                        {
                                Network.AuthServer.Send(AuthServerMessage.StreamTerminator, Network.AuthServer.LoginCount, 227);
                        }
                }
        }
}