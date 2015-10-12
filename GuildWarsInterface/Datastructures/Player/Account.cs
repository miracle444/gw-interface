#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

#endregion

namespace GuildWarsInterface.Datastructures.Player
{
        public sealed class Account
        {
                public enum Currency
                {
                        FreeBasicSkillUnlock = 2,
                        FreeEliteSkillUnlock,
                        FreeWeaponUpgradeUnlock,
                        FreeRuneUnlock,

                        FreeBasicSkillUnlock2 = 46,

                        FreeEliteSkillUnlock2 = 48,

                        MakeoverCredit = 101,
                        ExtremeMakeoverCredit,

                        Count = 131
                }

                private readonly List<PlayerCharacter> _characters;
                private readonly List<KeyValuePair<Unlock, ushort>> _unlocks;

                internal Account()
                {
                        _characters = new List<PlayerCharacter>();
                        _unlocks = new List<KeyValuePair<Unlock, ushort>>();
                }

                public IEnumerable<PlayerCharacter> Characters
                {
                        get { return _characters.ToArray(); }
                }

                public void AddCharacter(PlayerCharacter character)
                {
                        _characters.Add(character);

                        if (Game.State != GameState.Handshake && Game.State != GameState.LoginScreen)
                        {
                                Network.AuthServer.Send(AuthServerMessage.Character,
                                                        Network.AuthServer.TransactionCounter,
                                                        new byte[16],
                                                        0,
                                                        character.Name,
                                                        character.GetLoginScreenAppearance());
                        }
                }

                public void ClearCharacters()
                {
                        _characters.ToList().ForEach(RemoveCharacter);
                }

                internal void RemoveCharacter(PlayerCharacter character)
                {
                        _characters.Remove(character);
                }

                internal void SendCharacters()
                {
                        foreach (PlayerCharacter character in Characters)
                        {
                                Network.AuthServer.Send(AuthServerMessage.Character,
                                                        Network.AuthServer.TransactionCounter,
                                                        new byte[16],
                                                        0,
                                                        character.Name,
                                                        character.GetLoginScreenAppearance());
                        }
                }

                public void SetCurrency(Currency currency, ushort total, ushort used)
                {
                        Network.GameServer.Send(GameServerMessage.AccountCurrency, (ushort) currency, total, used);
                }

                public byte[] SerializeUnlocks()
                {
                        using (var stream = new MemoryStream())
                        using (var writer = new BinaryWriter(stream))
                        {
                                foreach (var unlock in _unlocks)
                                {
                                        writer.Write((ushort) unlock.Key);
                                        writer.Write(unlock.Value);
                                }

                                return stream.ToArray();
                        }
                }

                private enum Unlock : ushort
                {
                        AdditionalCharacterSlot = 1,
                        PvpUnlockCoupon,
                        MiniatureKanaxai,
                        MiniaturePanda,
                        MiniatureIslandGuardian,
                        MiniatureLonghairYeti,
                        MiniatureNagaRaincaller,
                        MiniatureOni,
                        PvpUnlockPackPropheciesWarriorSkills,
                        PvpUnlockPackPropheciesRangerSkills,
                        PvpUnlockPackPropheciesMonkSkills,
                        PvpUnlockPackPropheciesNecromancerSkills,
                        PvpUnlockPackPropheciesMesmerSkills,
                        PvpUnlockPackPropheciesElementalistSkills,
                        GuildWarsPropheciesPvpEdition,
                        GrayGiantMission,
                        GuildWarsFactionsPvpEdition,
                        GuildWarsNightfallPvpEdition,
                        MiniatureBear,
                        MiniatureAsura,
                        MiniatureVizu,
                        MiniatureShirokenAssassin,
                        MiniatureZhed,
                        MiniatureGrawl,
                        MiniatureDestroyer,
                        RawrTournamentToken,
                        PvpAccessKit,
                        CoreSkillUnlockPack,
                        PropheciesSkillUnlockPack,
                        FactionsSkillUnlockPack,
                        NightfallSkillUnlockPack,
                        EyeOfTheNorthSkillUnlockPack,
                        PvpItemUnlockPack,
                        MiniatureCeratadon,

                        RawrDragonTournamentToken = 37,
                        RawrPhoenixTournamentToken,
                        RawrTournamentToken2,
                        GuruGoldTournamentToken,
                        GuruSilverTournamentToken,

                        IgneousSummoningStone = 87,
                        PvpAccessKit2,
                        CharacterNameChange,
                        XunlaiStoragePane,
                        MakoverPack,
                        ExtremeMakeover,
                        PetUnlockPack,
                        GuildWars4ThAnniversaryStoragePane,
                        SignatureAsmodianWingEmote,

                        GrenthCostume = 98,
                        DwaynaCostume,

                        LimitedEditionWintersdayCostumePack = 101,
                        ShiningBladeUniform,
                        WhiteMantleDisguise,
                        LimitedEditionWarInKrytaCostumePack,
                        LichCostume,
                        MadKingsCourtCostume,
                        LimitedEditionHalloween2010CostumePack,
                        AgentOfBalthazar,
                        DiscipleOfMelandru,
                        LimitedEditionWintersday2010CostumePack,
                        KeiransTuxedoCostume,
                        WeddingCoupleAttire,
                        GentlemansTuxedoCostume,
                        FormalAttire,
                        DapperTuxedo,
                        WeddingCoupleAttire2,
                        LimitedEditionWeddingPartyCostumePack,
                        MercenaryHeroSlot,
                        MercenaryHeroThreePack,
                        MercenaryHeroEightPack,
                        AegisOfUnityCostume,
                        DragonguardCostume,
                        LimitedEditionWindsOfChangeCostumePack,
                        RavenheartWitchwearCostume,
                        ValeWraithCostume,
                        LimitedEditionHalloween2011CostumePack,
                        MiniatureMadKingsGuard,
                        EverlastingReindeerTonic,
                        AugurOfKormirCostume,
                        VisionOfLyssaCostume,
                        LimitedEditionWintersday2011CostumePack,
                        LimitedEditionPantheonCostumePack,
                }
        }
}