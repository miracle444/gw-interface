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
                        Network.AuthServer.TransactionCounter = (uint) data[1];

                        if (AuthLogic.Login((string) data[4], (string) data[5], (string) data[7]))
                        {
                                Game.State = GameState.CharacterScreen;

                                Game.Player.Account.SendCharacters();

                                Network.AuthServer.Send(AuthServerMessage.Gui, Network.AuthServer.TransactionCounter, (ushort) 0);

                                Game.Player.FriendList.Init();
                                Network.AuthServer.Send(AuthServerMessage.PlayerStatus, Network.AuthServer.TransactionCounter, (uint) Game.Player.Status);

                                Network.AuthServer.Send(AuthServerMessage.AccountPermissions,
                                                        Network.AuthServer.TransactionCounter,
                                                        2,
                                                        4,
                                                        new byte[] {0x38, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00},
                                                        new byte[] {0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x0C, 0x00},
                                                        new byte[16], // account id
                                                        new byte[16], // last played character id
                                                        8, // account management system version
                                                        Game.Player.Account.SerializeUnlocks(),
                                                        (byte) 23, // accepted eula
                                                        0); // enable name change (requires name change credits)

                                Network.AuthServer.SendTransactionSuccessCode(TransactionSuccessCode.Success);
                        }
                        else
                        {
                                Network.AuthServer.SendTransactionSuccessCode(TransactionSuccessCode.LoginFailed);
                        }
                }
        }
}