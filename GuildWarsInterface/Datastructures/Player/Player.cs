#region

using System;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Datastructures.Components;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;

#endregion

namespace GuildWarsInterface.Datastructures.Player
{
        public sealed class Player
        {
                public readonly Abilities Abilities;
                public readonly Account Account;
                public readonly FriendList FriendList;

                private PlayerCharacter _character;

                internal Player()
                {
                        Account = new Account();
                        Abilities = new Abilities();
                        FriendList = new FriendList();
                }

                public PlayerStatus Status { get; internal set; }

                public PlayerCharacter Character
                {
                        get
                        {
                                if (Game.State == GameState.CharacterCreation)
                                {
                                        Debug.ThrowException(new Exception("debug: should not access controlled character during character creation"));
                                }

                                return _character;
                        }
                        set { _character = value; }
                }
        }
}