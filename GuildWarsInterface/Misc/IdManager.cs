#region

using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Datastructures;
using GuildWarsInterface.Datastructures.Agents;
using GuildWarsInterface.Datastructures.Base;
using GuildWarsInterface.Datastructures.Items;

#endregion

namespace GuildWarsInterface.Misc
{
        public static class IdManager
        {
                private static uint _maxId = 1;
                private static readonly Dictionary<IIdentifyable, uint> _ids = new Dictionary<IIdentifyable, uint>();

                private static bool TryGet(uint id, out IIdentifyable result)
                {
                        KeyValuePair<IIdentifyable, uint> possibleResult = _ids.FirstOrDefault(element => element.Value == id);

                        result = possibleResult.Key;

                        if (possibleResult.Value != id || possibleResult.Key == null)
                        {
                                return false;
                        }

                        return true;
                }

                internal static bool TryGet<T>(uint id, out T result) where T : IIdentifyable
                {
                        IIdentifyable possibleResult;
                        if (TryGet(id, out possibleResult) && possibleResult is T)
                        {
                                result = (T) possibleResult;
                                return true;
                        }

                        result = default(T);
                        return false;
                }

                internal static uint GetId(Agent agent)
                {
                        return GetIdInternal(agent);
                }

                internal static ushort GetId(InventoryPage inventoryPage)
                {
                        return (ushort) GetIdInternal(inventoryPage);
                }

                internal static ushort GetId(Party party)
                {
                        return (ushort) GetIdInternal(party.Leader);
                }

                private static uint GetIdInternal(IIdentifyable key)
                {
                        if (!_ids.ContainsKey(key))
                        {
                                _ids.Add(key, _maxId++);
                        }

                        return _ids[key];
                }

                public static uint GetId(Item agent)
                {
                        return GetIdInternal(agent);
                }
        }
}