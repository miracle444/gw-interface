#region

using System;
using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Logic;
using Attribute = GuildWarsInterface.Declarations.Attribute;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class AbilitiesController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(7, DecreaseAttributeHandler);
                        controllerManager.RegisterHandler(8, IncreaseAttributeHandler);
                        controllerManager.RegisterHandler(59, ChangeSecondProfessionHandler);
                        controllerManager.RegisterHandler(85, EquipSkillHandler);
                        controllerManager.RegisterHandler(87, SwapSkillHandler);
                        controllerManager.RegisterHandler(88, MoveSkillToEmptySlotHandler);
                }

                private void DecreaseAttributeHandler(List<object> objects)
                {
                        var attribute = (Attribute) objects[3];

                        uint bonus = Game.Player.Abilities.GetAttributeBonus(attribute);
                        byte current = Game.Player.Abilities.GetAttributeValue(attribute);

                        Game.Player.Abilities.SetAttribute(attribute, (byte) (current - 1), bonus);
                }

                private void IncreaseAttributeHandler(List<object> objects)
                {
                        var attribute = (Attribute) objects[3];

                        uint bonus = Game.Player.Abilities.GetAttributeBonus(attribute);
                        byte current = Game.Player.Abilities.GetAttributeValue(attribute);

                        Game.Player.Abilities.SetAttribute(attribute, (byte) (current + 1), bonus);
                }

                private void ChangeSecondProfessionHandler(List<object> objects)
                {
                        Game.Player.Abilities.Profession = new Professions(Game.Player.Abilities.Profession.Primary, (Profession) objects[2]);
                }

                private void SwapSkillHandler(List<object> objects)
                {
                        uint skill1;
                        uint skill2;
                        if (Game.Player.Character.SkillBar.TryGetSlot((Skill) (uint) objects[2], (uint) objects[3], out skill1) &&
                            Game.Player.Character.SkillBar.TryGetSlot((Skill) (uint) objects[4], (uint) objects[5], out skill2))
                        {
                                GameLogic.SkillBarSwapSkills(skill1, skill2);
                        }
                        else
                        {
                                Debug.ThrowException(new ArgumentException());
                        }
                }

                private void EquipSkillHandler(List<object> objects)
                {
                        if (Game.Player.Character.SkillBar.GetSkill((uint) objects[2]) == (Skill) (uint) objects[3]) return;

                        GameLogic.SkillBarEquipSkill((uint) objects[2], (Skill) (uint) objects[3]);
                }

                private void MoveSkillToEmptySlotHandler(List<object> objects)
                {
                        if (Game.Player.Character.SkillBar.GetSkill((uint) objects[4]) != Skill.None)
                        {
                                Debug.ThrowException(new ArgumentException());
                        }

                        uint skillToMoveLocation;
                        if (!Game.Player.Character.SkillBar.TryGetSlot((Skill) (uint) objects[2], (uint) objects[3], out skillToMoveLocation))
                        {
                                Debug.ThrowException(new ArgumentException());
                        }

                        GameLogic.SkillBarMoveSkillToEmptySlot(skillToMoveLocation, (uint) objects[4]);
                }
        }
}