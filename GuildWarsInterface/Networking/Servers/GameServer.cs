#region

using System;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Controllers.GameControllers;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking.Protocol;
using GuildWarsInterface.Networking.Servers.Base;

#endregion

namespace GuildWarsInterface.Networking.Servers
{
        internal class GameServer : Server
        {
                internal const short PORT = 9112;

                public GameServer()
                {
                        RegisterController(new AbilitiesController());
                        RegisterController(new CharacterCreationController());
                        RegisterController(new ChatController());
                        RegisterController(new InstanceLoadController());
                        RegisterController(new InventoryController());
                        RegisterController(new MiscController());
                        RegisterController(new MovementController());
                        RegisterController(new PartyController());
                        RegisterController(new SkillController());
                        RegisterController(new VendorController());
                        RegisterController(new AttackController());
                }

                protected override short Port
                {
                        get { return PORT; }
                }

                protected override void Received(byte[] data)
                {
                        switch (BitConverter.ToUInt16(data, 0))
                        {
                                case 1280:
                                        return;
                                case 16896:
                                        Network.GameServer.Send(5633, new byte[20]);

                                        if (Game.State != GameState.CharacterCreation)
                                        {
                                                Network.GameServer.Send(GameServerMessage.InstanceLoadHead,
                                                                        (byte) 0x3F,
                                                                        (byte) 0x3F,
                                                                        (byte) 0,
                                                                        (byte) 0);

                                                Network.GameServer.Send(GameServerMessage.InstanceLoadDistrictInfo,
                                                                        IdManager.GetId(Game.Player.Character),
                                                                        (ushort) Game.Zone.Map,
                                                                        (byte) (Game.Zone.IsExplorable ? 1 : 0),
                                                                        (ushort) 1,
                                                                        (ushort) 0,
                                                                        (byte) 0,
                                                                        (byte) 0);
                                        }
                                        else
                                        {
                                                Network.GameServer.Send(GameServerMessage.BeginCharacterCreation);
                                        }

                                        return;
                                default:
                                        base.Received(data);
                                        break;
                        }
                }

                private void RegisterController(IController controller)
                {
                        controller.Register(this);
                }

                public void Send(GameServerMessage message, params object[] parameters)
                {
                        Send((int) message, parameters);
                }
        }
}