#region

using System;

#endregion

namespace GuildWarsInterface.Declarations
{
        [Flags]
        public enum CreatureStatus : uint
        {
                Clean = 0x0,
                Bleeding = 0x1,
                Condition = 0x2,
                Exploited = 0x4,
                Crippled = 0x8,
                Dead = 0x10,
                Wounded = 0x20,
                PoisonedOrDiseased = 0x40,
                Enchanted = 0x80,
                FreezePlayer = 0x100,
                GameMaster = 0x200,
                DegenHex = 0x400,
                Hexed = 0x800,
                Spawn = 0x1000,
                Sit = 0x2000,
                Sit2 = 0x4000,
                WeaponSpell = 0x8000,
                SpecialForceGrenade = 0x10000,
                SpecialForceEquipment = 0x20000,
                Unknown19 = 0x40000,
                Unknown20 = 0x80000,
                Unknown21 = 0x100000,
                Unknown22 = 0x200000,
                Unknown23 = 0x400000,
                Unknown24 = 0x800000,
                OffhandDagger = 0x1000000,
                Unknown26 = 0x2000000,
                Unknown27 = 0x4000000,
                Unknown28 = 0x8000000,
                Unknown29 = 0x10000000,
                Unknown30 = 0x20000000,
                Unknown31 = 0x40000000,
                Unknown32 = 0x80000000,
        }
}