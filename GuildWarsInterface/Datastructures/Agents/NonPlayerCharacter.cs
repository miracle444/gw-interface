#region

using System;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Agents
{
        public sealed class NonPlayerCharacter : Creature
        {
                public enum Model : ulong
                {
                        Empty = 0x43052,
                        XunlaiChest = 0x2366,
                        Aatxe = 0x1FAA5,
                        GhostMale = 0x1FC2C0001C604,
                        GhostFemale = 0x1FC270001C601,
                        GraspingDarkness = 0x22B6500022992,
                        Dhuum = 0x5652600056523
                }

                private readonly NpcFlags _flags;
                private readonly Model _model;
                private readonly uint _size;

                public NonPlayerCharacter(Model model, uint size = 100)
                {
                        _model = model;
                        _size = Math.Min(Math.Max(size, 6), 250);
                        _flags = NpcFlags.Unknown3;
                }

                protected override void OnNameChanged()
                {
                }

                protected override void OnCreation()
                {
                        Network.GameServer.Send(GameServerMessage.NpcStats,
                                                IdManager.GetId(this),
                                                (uint)((ulong)_model & 0xFFFFFFFF),
                                                0,
                                                _size << 24,
                                                0,
                                                (uint) _flags,
                                                (byte) Professions.Primary,
                                                (byte) 20,
                                                new[] {(char) 0x8102, (char) 0x58FA});

                        if ((ulong)_model > 0xFFFFFFFF) Network.GameServer.Send(GameServerMessage.NpcModel, IdManager.GetId(this), new[] { (uint)((ulong)_model >> 32) });

                        Network.GameServer.Send(GameServerMessage.NpcName, IdManager.GetId(this), new HString(Name).Serialize());

                        Professions = Professions;
                        Level = Level;
                        Status = Status;
                        Health.Maximum = Health.Maximum;
                        Energy.Maximum = Energy.Maximum;
                        Health.Current = Health.Current;
                        Energy.Current = Energy.Current;

                        Spawn(0x20000000, false);
                        Transformation.Orientation = Transformation.Orientation;
                        Morale = Morale;
                }
        }
}