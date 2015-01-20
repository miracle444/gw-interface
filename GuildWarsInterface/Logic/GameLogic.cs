using System;
using System.Linq;
using GuildWarsInterface.Datastructures;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Interaction;

namespace GuildWarsInterface.Logic
{
        public static class GameLogic
        {
                public delegate void ChangeMapHandler(Map map);

                public delegate void ChatMessageHandler(string message, Chat.Channel channel);

                public delegate void PartyAcceptJoinRequestHandler(Party joiningParty);

                public delegate void PartyInviteHandler(PlayerCharacter invitedCharacter);

                public delegate void PartyKickInviteHandler(Party partyToKick);

                public delegate void PartyKickJoinRequestHandler(Party partyToKick);

                public delegate void PartyKickMemberHandler(PlayerCharacter memberToKick);

                public delegate void SkillBarEquipSkillHandler(uint slot, Skill skill);

                public delegate void SkillBarMoveSkillToEmptySlotHandler(uint slotFrom, uint slotTo);

                public delegate void SkillBarSwapSkillsHandler(uint slot1, uint slot2);

                public static ChatMessageHandler ChatMessage = (message, channel) => Chat.ShowMessage(message, Game.Player.Character, Chat.GetColorForChannel(channel));

                public static PartyInviteHandler PartyInvite = invitedCharacter =>
                        {
                                Party invitedCharacterParty = Game.Zone.Parties.FirstOrDefault(party => party.Members.Contains(invitedCharacter));

                                if (invitedCharacterParty == null)
                                {
                                        invitedCharacterParty = new Party(invitedCharacter);

                                        Game.Zone.AddParty(invitedCharacterParty);
                                }

                                Game.Zone.Parties.FirstOrDefault(p => p.Members.Contains(Game.Player.Character)).AddInvite(invitedCharacterParty);
                        };

                public static PartyKickInviteHandler PartyKickInvite = partyToKick => Game.Zone.Parties.FirstOrDefault(p => p.Members.Contains(Game.Player.Character)).RemoveInvite(partyToKick);

                public static PartyAcceptJoinRequestHandler PartyAcceptJoinRequest = joiningParty => Game.Zone.Parties.FirstOrDefault(p => p.Members.Contains(Game.Player.Character)).MergeParty(joiningParty);

                public static PartyKickJoinRequestHandler PartyKickJoinRequest = partyToKick => Game.Zone.Parties.FirstOrDefault(p => p.Members.Contains(Game.Player.Character)).RemoveJoinRequest(partyToKick);

                public static Action PartyLeave = () => Game.Zone.Parties.FirstOrDefault(p => p.Members.Contains(Game.Player.Character)).RemoveMember(Game.Player.Character);

                public static PartyKickMemberHandler PartyKickMember = memberToKick => Game.Zone.Parties.FirstOrDefault(p => p.Members.Contains(Game.Player.Character)).RemoveMember(memberToKick);

                public static Action ExitToCharacterScreen = () => { };
                public static Action ExitToLoginScreen = () => { };

                public static ChangeMapHandler ChangeMap = map => Game.ChangeMap(map, zone => Game.Player.Character.Transformation.Position = MapData.GetDefaultSpawnPoint(zone.Map));

                public static SkillBarSwapSkillsHandler SkillBarSwapSkills = Game.Player.Abilities.SkillBar.MoveSkill;

                public static SkillBarEquipSkillHandler SkillBarEquipSkill = Game.Player.Abilities.SkillBar.SetSkill;

                public static SkillBarMoveSkillToEmptySlotHandler SkillBarMoveSkillToEmptySlot = Game.Player.Abilities.SkillBar.MoveSkill;
        }
}