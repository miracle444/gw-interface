namespace GuildWarsInterface.Declarations
{
        internal enum AgentProperty
        {
                // 0
                Appearance,
                Unknown1, // TODO: Generic Value missing!
                MeleeAttack,
                MeleeSkillAttack1,
                Attack,

                // 5
                Unknown2, // TODO: Generic Value missing!
                ApplyAura,
                RemoveAura,
                FreezePlayer,
                ShakeScreen,

                // 10
                SkillDamage,
                ApplyMarker,
                RemoveMarker,
                Unknown3,
                AddArmor, // TODO: Generic Value missing!

                // 15
                ArmorColor,
                DamageModifier1,
                DamageModifier2,
                Unknown4, // TODO: Generic Value missing!
                Unknown5, // TODO: Generic Value missing!

                // 20
                ApplyEffect1, // 2077 effects exist
                ApplyEffect2,
                Animation,
                DivineAura,
                Unknown6, // TODO: Generic Value missing!

                // 25
                WingsEmote,
                FameEmote,
                ZaishenEmote,
                AnimationLooped,
                BossGlow,

                // 30
                ApplyGuild1,
                ApplyGuild2,
                Unknown7, // TODO: Generic Value missing!
                CurrentEnergy,
                CurrentHealth,

                // 35
                Knockdown1,
                Level,
                LevelUp,
                AttackFailed,
                PickUpItem,

                // 40
                Unknown8, // TODO: Generic Value missing!
                MaximumEnergy,
                MaximumHealth,
                EnergyRegeneration,
                HealthRegeneration,

                // 45
                Unknown9, // TODO: Generic Value missing!
                MeleeSkillAttack2,
                Unknown10, // TODO: Generic Value missing!
                CastAttack, // TODO: Generic Value missing!
                InterruptAttack,

                // 50
                CastAttackSkill,
                Unknown12, // TODO: Generic Value missing!
                EnergyModifier2,
                EnergyModifier3,
                EnergyVisual,

                // 55
                ModifyHealthWithFloaters,
                ModifyHealthWithoutFloaters,
                Unknown13, // TODO: Generic Value missing!
                FightStance,
                InterruptSkill,

                // 60
                CastSkill,
                CastTimeModifier,
                EnergyModifier4,
                Knockdown2, // float seconds
                UnknownUsedForSuccessfulCharcreation, // TODO: Generic Value missing!

                // 65
                PvPTeam
        }
}