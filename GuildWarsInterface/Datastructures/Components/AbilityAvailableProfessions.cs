#region

using System;
using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Components
{
        internal class AbilityAvailableProfessions
        {
                private readonly Dictionary<Profession, bool> _availableProfessions;

                public AbilityAvailableProfessions()
                {
                        _availableProfessions = new Dictionary<Profession, bool>();

                        foreach (Profession profession in Enum.GetValues(typeof (Profession)).Cast<Profession>())
                        {
                                _availableProfessions.Add(profession, false);
                        }
                }

                private uint Serialize()
                {
                        uint serialized = 0;

                        foreach (var profession in _availableProfessions)
                        {
                                if (profession.Value)
                                {
                                        serialized |= (uint) (1 << (int) profession.Key);
                                }
                        }

                        return serialized;
                }

                public void SendUpdateAvailableProfessionsPacket()
                {
                        Network.GameServer.Send(GameServerMessage.UpdateAvailableProfessions,
                                                IdManager.GetId(Game.Player.Character),
                                                Serialize());
                }

                public void SetAvailableProfession(Profession profession, bool value)
                {
                        _availableProfessions[profession] = value;

                        if (Game.State == GameState.Playing)
                        {
                                SendUpdateAvailableProfessionsPacket();
                        }
                }
        }
}