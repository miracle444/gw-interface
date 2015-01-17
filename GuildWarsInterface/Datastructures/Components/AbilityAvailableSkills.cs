#region

using System.Collections.Generic;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Components
{
        internal sealed class AbilityAvailableSkills
        {
                private readonly List<Skill> _availableSkills;

                public AbilityAvailableSkills()
                {
                        _availableSkills = new List<Skill>();
                }

                public void SetAvailableSkill(Skill skill)
                {
                        if (!_availableSkills.Contains(skill))
                        {
                                _availableSkills.Add(skill);

                                if (Game.State == GameState.Playing)
                                {
                                        SendUpdateAvailableSkillsPacket();
                                }
                        }
                }

                public void RemoveAvailableSkill(Skill skill)
                {
                        if (_availableSkills.Contains(skill))
                        {
                                _availableSkills.Remove(skill);

                                if (Game.State == GameState.Playing)
                                {
                                        SendUpdateAvailableSkillsPacket();
                                }
                        }
                }

                private uint[] Serialize()
                {
                        var serialized = new List<uint>();

                        foreach (Skill skill in _availableSkills)
                        {
                                var skillId = (uint) skill;

                                uint section = skillId / 32;
                                uint offset = skillId % 32;

                                while (serialized.Count - 1 < section)
                                {
                                        serialized.Add(0);
                                }

                                serialized[(int) section] |= (uint) (1 << (int) offset);
                        }

                        return serialized.ToArray();
                }

                public void SendUpdateAvailableSkillsPacket()
                {
                        Network.GameServer.Send(GameServerMessage.UpdateAvailableSkills, Serialize());
                }
        }
}