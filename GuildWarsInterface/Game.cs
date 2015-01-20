#region

using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Datastructures;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Datastructures.Player;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Logic;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Modification.Hooks;
using GuildWarsInterface.Modification.Patches;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface
{
        public static class Game
        {
                public static readonly Player Player = new Player();

                public static Zone Zone { get; private set; }
                public static GameState State { get; internal set; }
                
                public static bool Initialize()
                {
                        try
                        {
                                TeamArenaPatch.Apply();
                                CancelLoginHook.Install(() => AuthLogic.CancelLogin());
                                AgentMovementHook.Install(id =>
                                        {
                                                Creature creature;
                                                if (IdManager.TryGet(id, out creature))
                                                {
                                                        creature.Transformation.OnChanged();
                                                }
                                        });

                                AgentTrackerHook.Install(data =>
                                {
                                                AgentTransformation.Tracker = data;

                                                AgentTransformation.OnGoalChanged();
                                        });

                                Network.Initialize();

                                Zone = new Zone(Map.AscalonCity);

                                return true;
                        }
                        catch (Exception)
                        {
                                return false;
                        }
                }

                public static void ChangeMap(Map map, Action<Zone> initialization)
                {
                        if (State == GameState.Playing) State = GameState.ChangingMap;

                        var newZone = new Zone(map);

                        initialization(newZone);

                        if (State == GameState.CharacterScreen)
                        {
                                State = GameState.LoadingScreen;

                                Network.AuthServer.Send(AuthServerMessage.Dispatch,
                                                        Network.AuthServer.LoginCount,
                                                        0,
                                                        (uint) newZone.Map,
                                                        new byte[]
                                                                {
                                                                        0x02, 0x00, 0x23, 0x98, 0x7F, 0x00, 0x00, 0x01,
                                                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                                                                },
                                                        0);
                        }
                        else if (State == GameState.ChangingMap)
                        {
                                State = GameState.LoadingScreen;

                                Network.GameServer.Send(GameServerMessage.Dispatch,
                                                        new byte[]
                                                                {
                                                                        0x02, 0x00, 0x23, 0x98, 0x7F, 0x00, 0x00, 0x01,
                                                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
                                                                },
                                                        0,
                                                        (byte) 0,
                                                        (ushort) newZone.Map,
                                                        (byte) 0,
                                                        0);
                        }
                        else
                        {
                                Debug.ThrowException(new Exception("cannot change zone in gamestate " + State));
                        }

                        Zone = newZone;
                }

                public static void TimePassed(uint milliseconds)
                {
                        if (State != GameState.Playing)
                        {
                                Debug.ThrowException(new Exception("time cannot pass when not playing"));
                        }

                        Network.GameServer.Send(GameServerMessage.Tick, milliseconds);
                }
        }
}