#region

using System;
using GuildWarsInterface.Datastructures.Agents.Components;
using GuildWarsInterface.Datastructures.Components;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;
using Attribute = GuildWarsInterface.Declarations.Attribute;

#endregion

namespace GuildWarsInterface.Datastructures.Player
{
        public sealed class Abilities
        {
                public readonly SkillBar SkillBar;
                private readonly AbilityAttributes _attributes;
                private readonly AbilityAvailableProfessions _availableProfessions;
                private readonly AbilityAvailableSkills _availableSkills;
                private byte _freeAttributePoints;
                private Professions _profession;

                internal Abilities()
                {
                        _attributes = new AbilityAttributes();
                        _availableSkills = new AbilityAvailableSkills();
                        _availableProfessions = new AbilityAvailableProfessions();
                        _profession = new Professions(Declarations.Profession.Warrior, Declarations.Profession.None);

                        SkillBar = new SkillBar();

                        _freeAttributePoints = 0;
                }

                public Professions Profession
                {
                        get { return _profession; }
                        set
                        {
                                _profession = value;

                                if (Game.Player.Character.Created)
                                {
                                        Network.GameServer.Send(GameServerMessage.UpdatePrivateProfessions,
                                                                IdManager.GetId(Game.Player.Character),
                                                                (byte) _profession.Primary,
                                                                (byte) _profession.Secondary,
                                                                (byte) 1);
                                }
                        }
                }

                public void SetAvailableProfession(Profession profession, bool value)
                {
                        _availableProfessions.SetAvailableProfession(profession, value);
                }

                public void ClearAvailableSkills()
                {
                        _availableSkills.Clear();
                }

                public void AddAvailableSkill(Skill skill)
                {
                        _availableSkills.SetAvailableSkill(skill);
                }

                public void RemoveAvailableSkill(Skill skill)
                {
                        _availableSkills.RemoveAvailableSkill(skill);
                }

                public byte GetFreeAttributePoints()
                {
                        return _freeAttributePoints;
                }

                public void SetFreeAttributePoints(byte value)
                {
                        _freeAttributePoints = value;

                        if (Game.State == GameState.LoadingScreen ||
                            Game.State == GameState.Playing)
                        {
                                Network.GameServer.Send(GameServerMessage.UpdateAttributePoints1,
                                                        IdManager.GetId(Game.Player.Character),
                                                        _freeAttributePoints);

                                Network.GameServer.Send(GameServerMessage.UpdateAttributePoints2,
                                                        IdManager.GetId(Game.Player.Character),
                                                        _freeAttributePoints);
                        }
                }

                public void SetAttribute(Attribute attribute, byte value, uint bonus)
                {
                        byte oldValue = GetAttributeValue(attribute);

                        byte low = Math.Min(oldValue, value);
                        byte high = Math.Max(oldValue, value);

                        int totalChange = 0;

                        for (int i = low + 1; i <= high; i++)
                        {
                                totalChange += GetAttributeLevelCost(i);
                        }

                        if (value > oldValue) totalChange = -totalChange;

                        int newFreeAttributePoints = _freeAttributePoints + totalChange;

                        Debug.Requires(newFreeAttributePoints >= 0);

                        SetFreeAttributePoints((byte) newFreeAttributePoints);

                        _attributes.SetAttribute(attribute, value, bonus);
                }

                private int GetAttributeLevelCost(int level)
                {
                        Debug.Requires(level <= 12);

                        switch (level)
                        {
                                case 1:
                                case 2:
                                case 3:
                                case 4:
                                case 5:
                                case 6:
                                case 7:
                                        return level;
                                case 8:
                                        return 9;
                                case 9:
                                        return 11;
                                case 10:
                                        return 13;
                                case 11:
                                        return 16;
                                case 12:
                                        return 20;
                                default:
                                        throw new ArgumentException();
                        }
                }

                public byte GetAttributeValue(Attribute attribute)
                {
                        return _attributes.GetAttributeValue(attribute);
                }

                public uint GetAttributeBonus(Attribute attribute)
                {
                        return _attributes.GetAttributeBonus(attribute);
                }

                internal void LoadAbilities1()
                {
                        Network.GameServer.Send(GameServerMessage.SetAttributePoints,
                                                IdManager.GetId(Game.Player.Character),
                                                _freeAttributePoints,
                                                (byte) 0);


                        SkillBar.SendUpdateSkillBarPacket();
                }

                internal void LoadAbilities2()
                {
                        _attributes.UpdateAttributes();

                        _availableSkills.SendUpdateAvailableSkillsPacket();

                        _availableProfessions.SendUpdateAvailableProfessionsPacket();
                }
        }
}