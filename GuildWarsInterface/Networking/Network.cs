#region

using GuildWarsInterface.Modification.Hooks;
using GuildWarsInterface.Modification.Patches;
using GuildWarsInterface.Networking.Servers;

#endregion

namespace GuildWarsInterface.Networking
{
        internal static class Network
        {
                private static readonly FileServer _fileServer = new FileServer();
                public static readonly AuthServer AuthServer = new AuthServer();
                public static readonly GameServer GameServer = new GameServer();

                public static void Initialize()
                {
                        ConnectHook.Install();
                        PortPatch.Apply();
                        DisableEncryptionPatch.Apply();
                        GetHostByNameHook.Install();

                        GameProtocolHook.Install(gameProtocol =>
                                {
                                        GameServer.Protocol = new Protocol.Protocol(gameProtocol);
                                        AuthServer.Protocol = GameServer.Protocol.Next;
                                });

                        _fileServer.Start();
                        AuthServer.Start();
                        GameServer.Start();
                }
        }
}