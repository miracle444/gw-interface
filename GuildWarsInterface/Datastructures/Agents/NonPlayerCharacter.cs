#region

using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Agents
{
        public sealed class NonPlayerCharacter : Creature
        {
                // size:
                // [6, ... , 250] standard: 100

                // files: 
                // 9062 xunlai chest ~ /
                // 16203 olias, whisperer order, palawa joko ~ 245627
                // 116225 female ghost ~ 130087
                // 116228 male ghost ~ 130092
                // 116378 skeletton ~ 353390
                // 129701 aatxe ~ /
                // 141714 grasping darkness ~ 142181
                // 243640 corsars, dervish henchmen ~ 245357
                // 274514 empty model ~ /
                // 282999 angry asuras ~ 314390
                // 283000 asuras ~ 314390

                private readonly uint _file;
                private readonly NpcFlags _flags;
                private readonly uint _model;
                private readonly uint _size;

                public NonPlayerCharacter(uint model, uint size = 20)
                {
                        _model = model;
                        _file = 116225;
                        _size = size;
                }

                public NonPlayerCharacter(uint model, uint file, NpcFlags flags, uint size = 20)
                        : this(model, size)
                {
                        _file = file;
                        _flags = flags;
                }

                protected override void OnNameChanged()
                {
                }

                protected override void OnCreation()
                {
                        Network.GameServer.Send(GameServerMessage.NpcStats,
                                                IdManager.GetId(this),
                                                _file,
                                                0,
                                                _size << 24,
                                                0,
                                                (uint) _flags,
                                                (byte) Professions.Primary,
                                                (byte) 20,
                                                new[] {(char) 0x8102, (char) 0x58FA});

                        if (_model > 0) Network.GameServer.Send(GameServerMessage.NpcModel, IdManager.GetId(this), new[] {_model});

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