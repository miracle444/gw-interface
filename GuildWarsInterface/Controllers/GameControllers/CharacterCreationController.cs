#region

using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class CharacterCreationController : IController
        {
                private readonly PlayerCharacter _characterCreationAgent = new PlayerCharacter();

                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(89, CharacterCreateUpdateCampaignAndProfessionHandler_);
                        controllerManager.RegisterHandler(131, CreateNewCharacterHandler_);
                        controllerManager.RegisterHandler(133, ValidateNewCharacterHandler_);
                }

                private void CreateNewCharacterHandler_(List<object> objects)
                {
                        Network.GameServer.Send(GameServerMessage.SetAttributePoints,
                                                IdManager.GetId(_characterCreationAgent),
                                                (byte) 0,
                                                (byte) 0);

                        _characterCreationAgent.SendAgentPropertyInt(AgentProperty.UnknownUsedForSuccessfulCharcreation, 0);

                        Network.GameServer.Send(GameServerMessage.CharacterCreation380);
                }

                private void CharacterCreateUpdateCampaignAndProfessionHandler_(List<object> objects)
                {
                        Game.Player.Abilities.Profession = new Professions((Profession) objects[2], Profession.None);
                }

                private void ValidateNewCharacterHandler_(List<object> objects)
                {
                        var createdCharacter = new PlayerCharacter
                                {
                                        Name = (string) objects[1],
                                        Appearance = new PlayerAppearance((uint) objects[2])
                                };

                        ValidationSucceeded(createdCharacter);
                }

                private void ValidationSucceeded(PlayerCharacter createdCharacter)
                {
                        Game.Player.Account.AddCharacter(createdCharacter);

                        Network.GameServer.Send(GameServerMessage.CharacterCreated,
                                                new byte[16],
                                                createdCharacter.Name,
                                                (ushort) Map.TeamArenas,
                                                createdCharacter.GetLoginScreenAppearance());

                        Game.Player.Character = createdCharacter;
                        Game.State = GameState.CharacterScreen;
                }

                private void ValidationFailed()
                {
                        Network.GameServer.Send(GameServerMessage.CharacterCreationValidationFailed, 29);
                }
        }
}