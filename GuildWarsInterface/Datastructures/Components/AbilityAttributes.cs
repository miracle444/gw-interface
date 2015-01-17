#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GuildWarsInterface.Debugging;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Misc;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;
using Attribute = GuildWarsInterface.Declarations.Attribute;

#endregion

namespace GuildWarsInterface.Datastructures.Components
{
        internal sealed class AbilityAttributes
        {
                private readonly Dictionary<Attribute, KeyValuePair<byte, uint>> _attributes;

                public AbilityAttributes()
                {
                        _attributes = new Dictionary<Attribute, KeyValuePair<byte, uint>>();

                        foreach (Attribute attribute in Enum.GetValues(typeof (Attribute)).Cast<Attribute>())
                        {
                                _attributes.Add(attribute, new KeyValuePair<byte, uint>(0, 0));
                        }
                }

                public void SetAttribute(Attribute attribute, byte value, uint bonus)
                {
                        Debug.Requires(!DeclarationConversion.IsPrimaryAttribute(attribute) ||
                                       DeclarationConversion.AttributesForProfession(Game.Player.Character.Professions.Primary).Contains(attribute));
                        Debug.Requires(value <= 12);

                        if (_attributes[attribute].Key == value && _attributes[attribute].Value == bonus) return;

                        _attributes[attribute] = new KeyValuePair<byte, uint>(value, bonus);

                        if (Game.State == GameState.Playing)
                        {
                                UpdateAttributes();
                        }
                }

                public byte GetAttributeValue(Attribute attribute)
                {
                        return _attributes[attribute].Key;
                }

                public uint GetAttributeBonus(Attribute attribute)
                {
                        return _attributes[attribute].Value;
                }

                private uint[] Serialize()
                {
                        KeyValuePair<Attribute, KeyValuePair<byte, uint>>[] nonZeroAttributes = _attributes.Where(entry => entry.Value.Key > 0 || entry.Value.Value > 0).ToArray();

                        List<uint> serialized = nonZeroAttributes.Select(attribute => (uint) attribute.Key).ToList();
                        serialized.AddRange(nonZeroAttributes.Select(attribute => attribute.Value.Key).Select(dummy => (uint) dummy));
                        serialized.AddRange(nonZeroAttributes.Select(attribute => attribute.Value.Key + attribute.Value.Value));

                        return serialized.ToArray();
                }

                public void UpdateAttributes()
                {
                        Network.GameServer.Send(GameServerMessage.UpdateAttributes,
                                                IdManager.GetId(Game.Player.Character),
                                                Serialize());
                }

                public override string ToString()
                {
                        var sb = new StringBuilder();

                        sb.Append("[ ");

                        foreach (var attribute in _attributes)
                        {
                                if (attribute.Value.Key == 0 && attribute.Value.Value == 0) continue;

                                if (sb.Length > 2) sb.Append(" | ");

                                sb.Append("(");
                                sb.Append(attribute.Key);
                                sb.Append(":");
                                sb.Append(attribute.Value.Key);

                                if (attribute.Value.Value > 0)
                                {
                                        sb.Append("+");
                                        sb.Append(attribute.Value.Value);
                                }

                                sb.Append(")");
                        }

                        sb.Append(" ]");

                        return sb.ToString();
                }
        }
}