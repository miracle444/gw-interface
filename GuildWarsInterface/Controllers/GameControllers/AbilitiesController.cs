#region

using System;
using System.Collections.Generic;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
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
                        uint locationOfSkill1 = 8;
                        uint locationOfSkill2 = 8;

                        for (uint i = 0; i < 8; i++)
                        {
                                if (Game.Player.Abilities.SkillBar.GetSkill(i) == (Skill) objects[2] &&
                                    Game.Player.Abilities.SkillBar.GetCopy(i) == (uint) objects[3])
                                {
                                        locationOfSkill1 = i;
                                }
                                else if (Game.Player.Abilities.SkillBar.GetSkill(i) == (Skill) objects[4] &&
                                         Game.Player.Abilities.SkillBar.GetCopy(i) == (uint) objects[5])
                                {
                                        locationOfSkill2 = i;
                                }
                        }

                        if (locationOfSkill1 > 7 || locationOfSkill2 > 7)
                        {
                                Debug.ThrowException(new ArgumentException());
                        }

                        Game.Player.Abilities.SkillBar.MoveSkill(locationOfSkill1, locationOfSkill2);
                }

                private void EquipSkillHandler(List<object> objects)
                {
                        if (Game.Player.Abilities.SkillBar.GetSkill((uint) objects[2]) == (Skill) objects[3]) return;

                        Game.Player.Abilities.SkillBar.SetSkill((uint) objects[2], (Skill) objects[3]);
                }

                private void MoveSkillToEmptySlotHandler(List<object> objects)
                {
                        if (Game.Player.Abilities.SkillBar.GetSkill((uint) objects[4]) != Skill.None)
                        {
                                Debug.ThrowException(new ArgumentException());
                        }

                        var skillToMove = (Skill) (uint) objects[2];
                        uint skillToMoveLocation = 8;

                        for (uint i = 0; i < 8; i++)
                        {
                                if (Game.Player.Abilities.SkillBar.GetSkill(i) == skillToMove &&
                                    Game.Player.Abilities.SkillBar.GetCopy(i) == (uint) objects[3])
                                {
                                        skillToMoveLocation = i;
                                }
                        }

                        if (skillToMoveLocation > 7)
                        {
                                Debug.ThrowException(new ArgumentException());
                        }

                        Game.Player.Abilities.SkillBar.MoveSkill(skillToMoveLocation, (uint) objects[4]);
                }
        }
}