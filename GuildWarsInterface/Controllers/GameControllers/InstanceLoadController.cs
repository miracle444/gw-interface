#region

using System;
using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class InstanceLoadController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(139, InstanceLoadRequestItemsHandler);
                        controllerManager.RegisterHandler(130, InstanceLoadRequestMapDataHandler);
                        controllerManager.RegisterHandler(138, InstanceLoadRequestPlayerDataHandler);
                }

                private void InstanceLoadRequestItemsHandler(List<object> objects)
                {
                        Network.GameServer.Send(GameServerMessage.CreateInventory, (ushort) 1, (byte) 0);
                        Network.GameServer.Send(GameServerMessage.UpdateActiveWeaponset, (ushort) 1, (byte) 0);

                        Game.Player.Character.Inventory.LoadItems();

                        Network.GameServer.Send(GameServerMessage.ConnectionStatus, (byte) 0, (ushort) 0x160, 0x85EB21CD);
                }

                private void InstanceLoadRequestMapDataHandler(List<object> objects)
                {
                        float[] data = MapData.GetMapCoordinates(Game.Zone.Map);

                        Network.GameServer.Send(GameServerMessage.InstanceLoadMapData,
                                                MapData.GetMapFile(Game.Zone.Map),
                                                data[0],
                                                data[1],
                                                (ushort) 0,
                                                (byte) 0,
                                                (byte) 0);

                        foreach (Map unlockedOutpost in Enum.GetValues(typeof (Map)).Cast<Map>())
                        {
                                Network.GameServer.Send(GameServerMessage.ShowOutpostOnWorldMap,
                                                        (ushort) unlockedOutpost,
                                                        (byte) 0);
                        }
                }

                private void InstanceLoadRequestPlayerDataHandler(List<object> objects)
                {
                        Network.GameServer.Send(GameServerMessage.PlayerData230, 0x61747431);

                        Game.Player.Abilities.LoadAbilities1();

                        Game.Player.Character.SkillBar.SendUpdateSkillBarPacket();

                        Network.GameServer.Send(GameServerMessage.PlayerData221,
                                                0,
                                                0,
                                                0,
                                                0,
                                                0,
                                                0,
                                                0,
                                                0,
                                                0,
                                                (uint) Game.Player.Character.Level,
                                                0,
                                                0,
                                                0,
                                                0,
                                                0);


                        Game.Player.Character.Create();
                        Game.Player.Abilities.Profession = Game.Player.Abilities.Profession;


                        if (Game.Zone.Map != Map.AscalonCity && Game.Zone.Map != Map.RiversideProvince)
                        {
                                Network.GameServer.Send(GameServerMessage.MapExploration127, 0x40, 0x80, 0x1B);
                                Network.GameServer.Send(GameServerMessage.MapExploration126, new uint[]
                                        {
                                                0x000B0000, 0x0054FFFF, 0x013A043B, 0x00E8043A,
                                                0x00000000, 0x00000000, 0x17000000
                                        });
                        }
                        else
                        {
                                Network.GameServer.Send(GameServerMessage.MapExploration127, 0x100, 0x200, 0x1E3);
                                Network.GameServer.Send(GameServerMessage.MapExploration126, new uint[]
                                        {
                                                0x0, 0xBBFF004C, 0xFB03FB02, 0xF805FA03,
                                                0x707F806, 0x608E805, 0x508E608, 0x103E50A,
                                                0xDD0A0403, 0x1040005, 0x1040403, 0xC3041403,
                                                0x2030005, 0x50403, 0xC3041403, 0xC030005,
                                                0xC4051904, 0x1A031302, 0x1A03D50A, 0x1B02D50A,
                                                0x550E0A, 0x20A02C3, 0x2D10B10, 0x20A040A,
                                                0x2D10B00, 0x20A040A, 0x6E30700, 0x3030208,
                                                0x30E05E6, 0x20605E6, 0x30503F1, 0x32102CD,
                                                0x2CD0304, 0x4020421, 0xA2202CD, 0x2DF0AF4,
                                                0x20D0812, 0x21305CE, 0x2B50210, 0x2270515,
                                                0x61402B5, 0x21402E0, 0xA300353C, 0xDF021803,
                                                0xF807F806, 0xFA04FA07, 0xF2030304, 0xFB030403,
                                                0x30DFF03, 0x42002D7, 0x20D02C5, 0x3C40520,
                                                0x520020C, 0x62F03C4, 0x62F02C5, 0x3AFF05F8,
                                                0xFFFF0016, 0x4FB02C7, 0x5F905FA, 0xFFFF04FA,
                                                0xFFFFFFFF, 0x3EFFFFFF, 0x0, 0xFFFF0017
                                        });
                                Network.GameServer.Send(GameServerMessage.MapExploration126, new uint[]
                                        {
                                                0xFFFFFFFF, 0xFFFFFFFF, 0x3FC02E0, 0x6FA04FB,
                                                0x4FA05F9, 0xD9003222, 0x5A029704, 0x5A02960A,
                                                0x5705960A, 0x5605980A, 0x56059C05, 0x57069A07,
                                                0x58069906, 0x59079707, 0x5A07900C, 0x5C06910B,
                                                0xF808F60A, 0xFFFFFF06, 0x2900001A, 0xFFFFFF00,
                                                0x5BFFFFFF, 0xDE03FB03, 0xDE031605, 0xDE031605,
                                                0xD8031605, 0xED020202, 0xEC070502, 0xE4030114,
                                                0x10DDF1F, 0x31B80F, 0xD04052E, 0x3FC02F4, 0x20605F9,
                                                0x20606EE, 0x20706ED, 0x3FA04ED, 0x2FC02FB,
                                                0x44602FC, 0x74503AF, 0x74503AD, 0x74503AD,
                                                0x9F509F5, 0x7F001476, 0xF509F509, 0xFFFFFF02,
                                                0xFFFFFFFF, 0xFFFFFFFF, 0x8AFFFF,
                                                0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0xFF000000
                                        });
                        }

                        Game.Zone.SpawnAgents();

                        Game.Player.Abilities.LoadAbilities2();

                        Network.GameServer.Send(GameServerMessage.ControlledCharacter, IdManager.GetId(Game.Player.Character), 3);

                        Game.Zone.CreateParties();

                        Game.State = GameState.Playing;

                        LoadingComplete();
                }

                private void LoadingComplete()
                {
                        Game.Player.FriendList.Update();

                        Game.Zone.Loaded = true;

                        Game.Player.Character.SpeechBubble("Round 29!");
                }
        }
}