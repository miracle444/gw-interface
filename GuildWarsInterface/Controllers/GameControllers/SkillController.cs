#region

using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Interaction;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class SkillController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(64, CastSkillHandler);
                }

                private void CastSkillHandler(List<object> objects)
                {
                        Creature target;
                        if ((uint) objects[3] == 0)
                        {
                                target = Game.Player.Character;
                        }
                        else if (!IdManager.TryGet((uint) objects[3], out target))
                        {
                                Chat.ShowMessage("invalid skill target");

                                Network.GameServer.Send(GameServerMessage.SkillRechargedVisualAutoAfterRecharge,
                                                        IdManager.GetId(Game.Player.Character),
                                                        (ushort) objects[1],
                                                        (uint) objects[2]);

                                return;
                        }

                        CastSkill((Skill) (uint) objects[1], (uint) objects[2], target);
                }

                private void CastSkill(Skill skill, uint copy, Creature target)
                {
                        Game.Player.Character.SendAgentPropertyFloat(AgentProperty.CastTimeModifier, 3.1F);
                        Game.Player.Character.SendAgentTargetPropertyInt(AgentProperty.CastSkill, Game.Player.Character, (uint) skill);

                        Network.GameServer.Send(GameServerMessage.SkillRechargedVisualAutoAfterRecharge,
                                                IdManager.GetId(Game.Player.Character),
                                                (ushort) skill,
                                                copy);

                        Network.GameServer.Send(GameServerMessage.SkillRecharging,
                                                IdManager.GetId(Game.Player.Character),
                                                (ushort) skill,
                                                copy,
                                                1);

                        Network.GameServer.Send(GameServerMessage.SkillRecharged,
                                                IdManager.GetId(Game.Player.Character),
                                                (ushort) skill,
                                                copy);
                }
        }
}