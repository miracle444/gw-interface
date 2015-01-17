#region

using System;

#endregion

namespace GuildWarsInterface.Declarations
{
        public static class DeclarationConversion
        {
                public static bool IsPrimaryAttribute(Attribute attribute)
                {
                        switch (attribute)
                        {
                                case Attribute.FastCasting:
                                case Attribute.SoulReaping:
                                case Attribute.EnergyStorage:
                                case Attribute.DivineFavor:
                                case Attribute.Strength:
                                case Attribute.Expertise:
                                case Attribute.CriticalStrikes:
                                case Attribute.SpawningPower:
                                case Attribute.Leadership:
                                case Attribute.Mysticism:
                                        return true;
                                default:
                                        return false;
                        }
                }


                public static Attribute[] AttributesForProfession(Profession profession)
                {
                        switch (profession)
                        {
                                case Profession.None:
                                        return new Attribute[0];
                                case Profession.Warrior:
                                        return new[]
                                                {
                                                        Attribute.Strength,
                                                        Attribute.AxeMastery,
                                                        Attribute.HammerMastery,
                                                        Attribute.Swordsmanship,
                                                        Attribute.Tactics
                                                };
                                case Profession.Ranger:
                                        return new[]
                                                {
                                                        Attribute.BeastMastery,
                                                        Attribute.Expertise,
                                                        Attribute.WildernessSurvival,
                                                        Attribute.Marksmanship
                                                };
                                case Profession.Monk:
                                        return new[]
                                                {
                                                        Attribute.HealingPrayers,
                                                        Attribute.SmitingPrayers,
                                                        Attribute.ProtectionPrayers,
                                                        Attribute.DivineFavor
                                                };
                                case Profession.Necromancer:
                                        return new[]
                                                {
                                                        Attribute.BloodMagic,
                                                        Attribute.DeathMagic,
                                                        Attribute.SoulReaping,
                                                        Attribute.Curses
                                                };
                                case Profession.Mesmer:
                                        return new[]
                                                {
                                                        Attribute.FastCasting,
                                                        Attribute.IllusionMagic,
                                                        Attribute.DominationMagic,
                                                        Attribute.InspirationMagic
                                                };
                                case Profession.Elementalist:
                                        return new[]
                                                {
                                                        Attribute.AirMagic,
                                                        Attribute.EarthMagic,
                                                        Attribute.FireMagic,
                                                        Attribute.WaterMagic,
                                                        Attribute.EnergyStorage
                                                };
                                case Profession.Assassin:
                                        return new[]
                                                {
                                                        Attribute.DaggerMastery,
                                                        Attribute.DeadlyArts,
                                                        Attribute.ShadowArts,
                                                        Attribute.CriticalStrikes
                                                };
                                case Profession.Ritualist:
                                        return new[]
                                                {
                                                        Attribute.Communing,
                                                        Attribute.RestorationMagic,
                                                        Attribute.ChannelingMagic,
                                                        Attribute.SpawningPower
                                                };
                                case Profession.Paragon:
                                        return new[]
                                                {
                                                        Attribute.SpearMastery,
                                                        Attribute.Command,
                                                        Attribute.Motivation,
                                                        Attribute.Leadership
                                                };
                                case Profession.Dervish:
                                        return new[]
                                                {
                                                        Attribute.ScytheMastery,
                                                        Attribute.WindPrayers,
                                                        Attribute.EarthPrayers,
                                                        Attribute.Mysticism
                                                };
                                default:
                                        throw new ArgumentOutOfRangeException("profession");
                        }
                }

                public static bool ItemTypeToEquipmentSlot(ItemType itemType, out EquipmentSlot result)
                {
                        switch (itemType)
                        {
                                case ItemType.Axe:
                                case ItemType.Bow:
                                case ItemType.Hammer:
                                case ItemType.Wand:
                                case ItemType.Staff:
                                case ItemType.Sword:
                                case ItemType.Dagger:
                                case ItemType.Scyte:
                                case ItemType.Spear:
                                        result = EquipmentSlot.Mainhand;
                                        return true;
                                case ItemType.Focus:
                                case ItemType.Shield:
                                        result = EquipmentSlot.Offhand;
                                        return true;
                                case ItemType.Feet:
                                        result = EquipmentSlot.Feet;
                                        return true;
                                case ItemType.Chest:
                                        result = EquipmentSlot.Chest;
                                        return true;
                                case ItemType.Arms:
                                        result = EquipmentSlot.Arms;
                                        return true;
                                case ItemType.Head:
                                        result = EquipmentSlot.Head;
                                        return true;
                                case ItemType.Legs:
                                        result = EquipmentSlot.Legs;
                                        return true;
                                default:
                                        result = EquipmentSlot.Costume;
                                        return false;
                        }
                }
        }
}