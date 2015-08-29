#region

using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Interaction;
using GuildWarsInterface.Logic;
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

                        uint slot;
                        if (Game.Player.Character.SkillBar.TryGetSlot((Skill) (uint) objects[1], (uint) objects[2], out slot))
                        {
                                GameLogic.CastSkill(slot, target);
                        }
                }
        }
}