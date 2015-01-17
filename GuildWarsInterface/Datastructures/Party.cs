#region

using System;
using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Datastructures.Base;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures
{
        public sealed class Party : Creatable
        {
                public readonly PlayerCharacter Leader;
                private readonly List<Party> _invites;
                private readonly List<Party> _joinRequests;
                private readonly List<PlayerCharacter> _members;

                public Party(PlayerCharacter leader)
                {
                        if (leader == null)
                        {
                                Debug.ThrowException(new ArgumentException("leader of a party cannot be null"));
                        }

                        Leader = leader;
                        _members = new List<PlayerCharacter> {Leader};
                        _invites = new List<Party>();
                        _joinRequests = new List<Party>();
                }

                public PlayerCharacter[] Members
                {
                        get { return _members.ToArray(); }
                }

                protected override void OnCreation()
                {
                        Network.GameServer.Send(GameServerMessage.CreateParty1, IdManager.GetId(this));
                        Network.GameServer.Send(GameServerMessage.AddPartyMember, IdManager.GetId(this), (ushort) IdManager.GetId(Leader), (byte) 1);
                        Network.GameServer.Send(GameServerMessage.CreateParty2, IdManager.GetId(this));
                }

                protected override void OnDestruction()
                {
                        if (_members.Contains(Game.Player.Character))
                        {
                                _invites.ForEach(RemoveInvite);
                                _joinRequests.ForEach(RemoveJoinRequest);
                        }
                        else
                        {
                                Game.Zone.Parties.Where(p => p._joinRequests.Contains(this)).ToList().ForEach(p => p.RemoveJoinRequest(this));
                                Game.Zone.Parties.Where(p => p._invites.Contains(this)).ToList().ForEach(p => p.RemoveInvite(this));
                        }
                }

                internal void SendPreActiveInformation()
                {
                        foreach (PlayerCharacter member in _members)
                        {
                                if (Game.Zone.Parties.Any(party => party != this && party.Members.Contains(member)))
                                {
                                        Debug.ThrowException(new Exception("member " + member + " already part of another party"));
                                }

                                if (member == Leader) continue;

                                Network.GameServer.Send(GameServerMessage.AddPartyMember, IdManager.GetId(this), (ushort) IdManager.GetId(member), (byte) 1);
                        }

                        if (_members.Contains(Game.Player.Character))
                        {
                                ShowParty();
                        }
                }

                private void ShowParty()
                {
                        Network.GameServer.Send(GameServerMessage.ShowPartyWindow, IdManager.GetId(this), (byte) (Leader == Game.Player.Character ? 1 : 0));

                        foreach (Party invitedParty in _invites)
                        {
                                Network.GameServer.Send(GameServerMessage.InviteParty, IdManager.GetId(invitedParty));
                        }

                        foreach (Party joinRequestParty in _joinRequests)
                        {
                                Network.GameServer.Send(GameServerMessage.AddPartyJoinRequest, IdManager.GetId(joinRequestParty));
                        }
                }

                public void AddMember(PlayerCharacter agent)
                {
                        if (ParentZone != null && ParentZone.Parties.Any(party => party._members.Contains(agent)))
                        {
                                Debug.ThrowException(new Exception("agent " + agent.Name + " is already member of a party"));
                        }

                        _members.Add(agent);

                        if (Created && ParentZone == Game.Zone)
                        {
                                Network.GameServer.Send(GameServerMessage.AddPartyMember, IdManager.GetId(this), (ushort) IdManager.GetId(agent), (byte) 1);

                                if (agent == Game.Player.Character)
                                {
                                        ShowParty();
                                }
                        }
                }

                public void RemoveMember(PlayerCharacter agent)
                {
                        if (!_members.Contains(agent))
                        {
                                Debug.ThrowException(new Exception("agent " + agent.Name + " not part of this party"));
                        }

                        if (_members.Count <= 1)
                        {
                                Debug.ThrowException(new Exception("cannot remove last member of group"));
                        }

                        _members.Remove(agent);

                        if (Created && ParentZone == Game.Zone)
                        {
                                if (agent != Leader)
                                {
                                        Network.GameServer.Send(GameServerMessage.RemovePartyMember, IdManager.GetId(this), (ushort) IdManager.GetId(agent));


                                        if (agent == Game.Player.Character)
                                        {
                                                Game.Zone.AddParty(new Party(Game.Player.Character));
                                        }
                                }
                                else
                                {
                                        Game.Zone.RemoveParty(this);

                                        var restParty = new Party(_members[0]);

                                        Game.Zone.AddParty(restParty);

                                        foreach (PlayerCharacter member in _members)
                                        {
                                                if (member == restParty.Leader) continue;

                                                restParty.AddMember(member);
                                        }

                                        foreach (Party invite in _invites)
                                        {
                                                restParty.AddInvite(invite);
                                        }

                                        foreach (Party joinRequest in _joinRequests)
                                        {
                                                restParty.AddJoinRequest(joinRequest);
                                        }

                                        Game.Zone.AddParty(new Party(Leader));
                                }
                        }
                }

                public void AddInvite(Party party)
                {
                        if (party.ParentZone != ParentZone)
                        {
                                Debug.ThrowException(new Exception("incompatible parties: not the same parent zone"));
                        }

                        if (party == this)
                        {
                                Debug.ThrowException(new Exception("cannot invite party to itself"));
                        }

                        _invites.Add(party);

                        if (Created && ParentZone == Game.Zone && _members.Contains(Game.Player.Character))
                        {
                                Network.GameServer.Send(GameServerMessage.InviteParty, IdManager.GetId(party));
                        }
                }

                public void RemoveInvite(Party party)
                {
                        if (party.ParentZone != ParentZone)
                        {
                                Debug.ThrowException(new Exception("incompatible parties: not the same parent zone"));
                        }

                        if (!_invites.Contains(party))
                        {
                                Debug.ThrowException(new Exception("party not invited to this party"));
                        }

                        _invites.Remove(party);

                        if (Created && ParentZone == Game.Zone && _members.Contains(Game.Player.Character))
                        {
                                Network.GameServer.Send(GameServerMessage.KickInvitedParty, IdManager.GetId(party));
                        }
                }

                public void AddJoinRequest(Party party)
                {
                        if (party.ParentZone != ParentZone)
                        {
                                Debug.ThrowException(new Exception("incompatible parties: not the same parent zone"));
                        }

                        if (party == this)
                        {
                                Debug.ThrowException(new Exception("cannot join request party on itself"));
                        }

                        _joinRequests.Add(party);

                        if (Created && ParentZone == Game.Zone && _members.Contains(Game.Player.Character))
                        {
                                Network.GameServer.Send(GameServerMessage.AddPartyJoinRequest, IdManager.GetId(party));
                        }
                }

                public void RemoveJoinRequest(Party party)
                {
                        if (party.ParentZone != ParentZone)
                        {
                                Debug.ThrowException(new Exception("incompatible parties: not the same parent zone"));
                        }

                        if (!_joinRequests.Contains(party))
                        {
                                Debug.ThrowException(new Exception("party not a join request of this party"));
                        }

                        _joinRequests.Remove(party);

                        if (Created && ParentZone == Game.Zone && _members.Contains(Game.Player.Character))
                        {
                                Network.GameServer.Send(GameServerMessage.KickPartyJoinRequest, IdManager.GetId(party));
                        }
                }

                public void MergeParty(Party otherParty)
                {
                        Game.Zone.RemoveParty(otherParty);

                        foreach (PlayerCharacter member in otherParty._members)
                        {
                                AddMember(member);
                        }
                }
        }
}