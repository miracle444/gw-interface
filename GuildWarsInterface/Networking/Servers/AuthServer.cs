#region

using System;
using GuildWarsInterface.Controllers.AuthControllers;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Networking.Protocol;
using GuildWarsInterface.Networking.Servers.Base;

#endregion

namespace GuildWarsInterface.Networking.Servers
{
        internal class AuthServer : Server
        {
                internal const short PORT = 6112;

                public uint TransactionCounter;

                public AuthServer()
                {
                        RegisterController(new ComputerInfoController());
                        RegisterController(new LoginController());
                        RegisterController(new MiscController());
                }

                protected override short Port
                {
                        get { return PORT; }
                }

                protected override void Received(byte[] data)
                {
                        switch (BitConverter.ToUInt16(data, 0))
                        {
                                case 1024:
                                        return;
                                case 16896:
                                        Network.AuthServer.Send(5633, new byte[20]);
                                        Game.State = GameState.LoginScreen;
                                        return;
                                default:
                                        base.Received(data);
                                        break;
                        }
                }

                public void Send(AuthServerMessage message, params object[] parameters)
                {
                        Send((int) message, parameters);
                }

                private void RegisterController(IController controller)
                {
                        controller.Register(this);
                }

                public void SendTransactionSuccessCode(TransactionSuccessCode code)
                {
                        Send(AuthServerMessage.TransactionSuccessCode, TransactionCounter, (uint)code);
                }
        }
}