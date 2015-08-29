#region

using System;
using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Logic;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class MiscController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(2, ExitToCharacterScreenHandler);
                        controllerManager.RegisterHandler(3, ExitToLoginScreenHandler);
                        controllerManager.RegisterHandler(171, ChangeMapHandler);
                        controllerManager.RegisterHandler(78, StylistChangeAppearanceHandler);
                }

                private void StylistChangeAppearanceHandler(List<object> objects)
                {
                        Game.Player.Character.Appearance = new PlayerAppearance((uint) objects[1]);

                        Network.GameServer.Send(GameServerMessage.AccountCurrency, (ushort) 101, (ushort) 1, (ushort) 0);
                        Network.GameServer.Send(GameServerMessage.AccountCurrency, (ushort) 102, (ushort) 1, (ushort) 0);
                        Network.GameServer.Send(GameServerMessage.OpenWindow, 0, (byte) 3, 0);

                        Console.WriteLine(Game.Player.Character.Appearance);
                }

                private void ExitToCharacterScreenHandler(List<object> objects)
                {
                        Game.State = GameState.CharacterScreen;

                        Network.GameServer.Disconnect();

                        GameLogic.ExitToCharacterScreen();
                }

                private void ExitToLoginScreenHandler(List<object> objects)
                {
                        Game.State = GameState.LoginScreen;

                        Network.GameServer.Disconnect();

                        GameLogic.ExitToLoginScreen();
                }

                private void ChangeMapHandler(List<object> objects)
                {
                        GameLogic.ChangeMap((Map) (ushort) objects[1]);
                }
        }
}