#region

using System;
using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Controllers.Base;
using GuildWarsInterface.Datastructures;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Logic;
using GuildWarsInterface.Misc;

#endregion

namespace GuildWarsInterface.Controllers.GameControllers
{
        internal class PartyController : IController
        {
                public void Register(IControllerManager controllerManager)
                {
                        controllerManager.RegisterHandler(150, AcceptJoinRequestHandler);
                        controllerManager.RegisterHandler(151, KickInviteHandler);
                        controllerManager.RegisterHandler(152, KickJoinRequestHandler);
                        controllerManager.RegisterHandler(154, InviteHandler);
                        controllerManager.RegisterHandler(156, LeaveHandler);
                        controllerManager.RegisterHandler(163, KickMemberHandler);
                }

                private void KickMemberHandler(List<object> objects)
                {
                        Party controlledCharacterParty = Game.Zone.Parties.FirstOrDefault(party => party.Members.Contains(Game.Player.Character));

                        if (controlledCharacterParty == null)
                        {
                                Debug.ThrowException(new Exception("cannot kick member without party"));
                        }

                        if (controlledCharacterParty.Leader != Game.Player.Character)
                        {
                                Debug.ThrowException(new Exception("can only kick member if leader"));
                        }

                        var memberToKick = (PlayerCharacter) Game.Zone.Agents.FirstOrDefault(agent => IdManager.GetId(agent) == (ushort) objects[1]);

                        if (memberToKick == null)
                        {
                                Debug.ThrowException(new Exception("agent to kick does not exist"));
                        }

                        GameLogic.PartyKickMember(memberToKick);
                }

                private void AcceptJoinRequestHandler(List<object> objects)
                {
                        Party controlledCharacterParty = Game.Zone.Parties.FirstOrDefault(party => party.Members.Contains(Game.Player.Character));

                        if (controlledCharacterParty == null)
                        {
                                Debug.ThrowException(new Exception("cannot accept join request without party"));
                        }

                        if (controlledCharacterParty.Leader != Game.Player.Character)
                        {
                                Debug.ThrowException(new Exception("can only accept join request if leader"));
                        }

                        Party joinRequestParty = Game.Zone.Parties.FirstOrDefault(party => IdManager.GetId(party) == (ushort) objects[1]);

                        if (joinRequestParty == null)
                        {
                                Debug.ThrowException(new Exception("party to accept join request does not exist"));
                        }

                        GameLogic.PartyAcceptJoinRequest(joinRequestParty);
                }

                private void KickJoinRequestHandler(List<object> objects)
                {
                        Party controlledCharacterParty = Game.Zone.Parties.FirstOrDefault(party => party.Members.Contains(Game.Player.Character));

                        if (controlledCharacterParty == null)
                        {
                                Debug.ThrowException(new Exception("cannot kick join request without party"));
                        }

                        if (controlledCharacterParty.Leader != Game.Player.Character)
                        {
                                Debug.ThrowException(new Exception("can only kick join request if leader"));
                        }

                        Party joinRequestPartyToKick = Game.Zone.Parties.FirstOrDefault(party => IdManager.GetId(party) == (ushort) objects[1]);

                        if (joinRequestPartyToKick == null)
                        {
                                Debug.ThrowException(new Exception("party to kick does not exist"));
                        }

                        GameLogic.PartyKickJoinRequest(joinRequestPartyToKick);
                }

                private void KickInviteHandler(List<object> objects)
                {
                        Party controlledCharacterParty = Game.Zone.Parties.FirstOrDefault(party => party.Members.Contains(Game.Player.Character));

                        if (controlledCharacterParty == null)
                        {
                                Debug.ThrowException(new Exception("cannot kick invite without party"));
                        }

                        if (controlledCharacterParty.Leader != Game.Player.Character)
                        {
                                Debug.ThrowException(new Exception("can only kick invite if leader"));
                        }

                        Party invitedPartyToKick = Game.Zone.Parties.FirstOrDefault(party => IdManager.GetId(party) == (ushort) objects[1]);

                        if (invitedPartyToKick == null)
                        {
                                Debug.ThrowException(new Exception("party to kick does not exist"));
                        }

                        GameLogic.PartyKickInvite(invitedPartyToKick);
                }

                private void InviteHandler(List<object> objects)
                {
                        Party controlledCharacterParty = Game.Zone.Parties.FirstOrDefault(party => party.Members.Contains(Game.Player.Character));

                        if (controlledCharacterParty == null)
                        {
                                Debug.ThrowException(new Exception("cannot invite without party"));
                        }

                        if (controlledCharacterParty.Leader != Game.Player.Character)
                        {
                                Debug.ThrowException(new Exception("can only invite if leader"));
                        }

                        var invitedCharacter = (PlayerCharacter) Game.Zone.Agents.FirstOrDefault(agent => IdManager.GetId(agent) == (ushort) objects[1]);

                        if (invitedCharacter == null)
                        {
                                Debug.ThrowException(new Exception("invited agent does not exist"));
                        }

                        GameLogic.PartyInvite(invitedCharacter);
                }

                private void LeaveHandler(List<object> objects)
                {
                        Party controlledCharacterParty = Game.Zone.Parties.FirstOrDefault(party => party.Members.Contains(Game.Player.Character));

                        if (controlledCharacterParty == null)
                        {
                                Debug.ThrowException(new Exception("cannot leave while not in a party"));
                        }

                        GameLogic.PartyLeave();
                }
        }
}