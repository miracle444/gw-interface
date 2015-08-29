#region

using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Player
{
        public sealed class SkillBar
        {
                private readonly PlayerCharacter _character;
                private readonly Dictionary<SkillBarSkill, uint> _copies;
                private readonly SkillBarSkill[] _skills;

                internal SkillBar(PlayerCharacter character)
                {
                        _character = character;
                        _skills = new SkillBarSkill[8];
                        _copies = new Dictionary<SkillBarSkill, uint>();

                        for (uint i = 0; i < 8; i++)
                        {
                                _skills[i] = new SkillBarSkill(this, Skill.None);
                                _copies.Add(_skills[i], 0);
                        }
                }

                public void SetSkill(uint index, Skill value)
                {
                        _skills[index] = new SkillBarSkill(this, value);

                        UpdateCopies();

                        if (Game.State == GameState.Playing)
                        {
                                SendUpdateSkillBarPacket();
                        }
                }

                private void UpdateCopies()
                {
                        _copies.Clear();

                        var counters = new Dictionary<Skill, uint>();

                        for (uint i = 0; i < 8; i++)
                        {
                                Skill skill = _skills[i].Skill;
                                uint copy = 0;

                                if (skill != Skill.None)
                                {
                                        if (!counters.ContainsKey(skill))
                                        {
                                                counters.Add(skill, 1);
                                        }
                                        else
                                        {
                                                copy = counters[skill];

                                                counters[skill] = copy + 1;
                                        }
                                }

                                _copies.Add(_skills[i], copy);
                        }
                }

                public void MoveSkill(uint from, uint to)
                {
                        SkillBarSkill temp = _skills[from];
                        _skills[from] = _skills[to];
                        _skills[to] = temp;

                        if (Game.State == GameState.Playing)
                        {
                                SendUpdateSkillBarPacket();
                        }
                }

                public Skill GetSkill(uint index)
                {
                        return _skills[index].Skill;
                }

                private uint GetCopy(uint index)
                {
                        return _copies[_skills[index]];
                }

                private uint[] Serialize2()
                {
                        return _skills.Select(skill => _copies[skill]).ToArray();
                }

                private uint[] Serialize()
                {
                        return _skills.Select(skill => (uint) skill.Skill).ToArray();
                }

                internal void SendUpdateSkillBarPacket()
                {
                        Network.GameServer.Send(GameServerMessage.UpdateSkillBar,
                                                IdManager.GetId(Game.Player.Character),
                                                Serialize(),
                                                Serialize2(),
                                                (byte) 1);
                }

                public override string ToString()
                {
                        var sb = new StringBuilder();

                        sb.Append("[ ");

                        foreach (SkillBarSkill skill in _skills)
                        {
                                if (sb.Length > 2) sb.Append(" | ");

                                sb.Append(skill);
                        }

                        sb.Append(" ]");

                        return sb.ToString();
                }

                internal bool TryGetSlot(Skill skill, uint copy, out uint slot)
                {
                        for (int i = 0; i < _skills.Length; i++)
                        {
                                if (_skills[i].Skill != skill) continue;

                                if (_copies[_skills[i]] == copy)
                                {
                                        slot = (uint) i;
                                        return true;
                                }
                        }

                        slot = 9;
                        return false;
                }

                public void RechargeStart(uint slot, uint recharge)
                {
                        Skill skill = GetSkill(slot);
                        uint copy = GetCopy(slot);

                        Network.GameServer.Send(GameServerMessage.SkillRechargedVisualAutoAfterRecharge,
                                                IdManager.GetId(_character),
                                                (ushort) skill,
                                                copy);

                        Network.GameServer.Send(GameServerMessage.SkillRecharging,
                                                IdManager.GetId(_character),
                                                (ushort) skill,
                                                copy,
                                                recharge);
                }

                public void RechargeEnd(uint slot)
                {
                        Skill skill = GetSkill(slot);
                        uint copy = GetCopy(slot);

                        Network.GameServer.Send(GameServerMessage.SkillRecharged,
                                                IdManager.GetId(_character),
                                                (ushort) skill,
                                                copy);

                        RechargedVisual(slot);
                }

                public void RechargedVisual(uint slot)
                {
                        Skill skill = GetSkill(slot);
                        uint copy = GetCopy(slot);

                        Network.GameServer.Send(GameServerMessage.SkillRechargedVisual,
                                                IdManager.GetId(_character),
                                                (ushort) skill,
                                                copy);
                }

                private class SkillBarSkill
                {
                        private readonly SkillBar _parent;

                        public SkillBarSkill(SkillBar parent, Skill skill)
                        {
                                _parent = parent;
                                Skill = skill;
                        }

                        public Skill Skill { get; private set; }

                        public override string ToString()
                        {
                                return string.Format("({0}:{1})", Skill, _parent._copies[this]);
                        }
                }
        }
}