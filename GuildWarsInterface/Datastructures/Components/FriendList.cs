using System.Collections.Generic;
using System.Linq;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Networking;
using GuildWarsInterface.Networking.Protocol;

namespace GuildWarsInterface.Datastructures.Components
{
        public sealed class FriendList
        {
                public enum Type
                {
                        None,
                        Friend,
                        Ignored
                }

                private readonly Dictionary<string, Entry> _entries = new Dictionary<string, Entry>();

                public IEnumerable<Entry> Entries
                {
                        get { return _entries.Values.ToArray(); }
                }

                public IEnumerable<Entry> Friends
                {
                        get { return Entries.Where(entry => entry.Type == Type.Friend); }
                }

                public IEnumerable<Entry> Ignored
                {
                        get { return Entries.Where(entry => entry.Type == Type.Ignored); }
                }

                public void Add(Type type, string baseCharacterName, string currentCharacterName = "", PlayerStatus playerStatus = PlayerStatus.Offline, Map map = 0)
                {
                        lock (this)
                        {
                                var newEntry = new Entry(type, baseCharacterName, currentCharacterName, playerStatus, map);

                                if (!_entries.ContainsKey(newEntry.BaseCharacterName))
                                {
                                        _entries.Add(newEntry.BaseCharacterName, newEntry);
                                }
                                else
                                {
                                        _entries[newEntry.BaseCharacterName] = newEntry;
                                }

                                if (Game.State == GameState.Playing)
                                {
                                        UpdateEntry(newEntry);
                                }
                        }
                }

                public void Remove(string baseCharacterName)
                {
                        lock (this)
                        {
                                if (_entries.ContainsKey(baseCharacterName))
                                {
                                        _entries.Remove(baseCharacterName);
                                }
                        }
                }

                public void Move(string baseCharacterName, Type target)
                {
                        lock (this)
                        {
                                if (target == Type.None)
                                {
                                        Remove(baseCharacterName);
                                }
                                else if (_entries.ContainsKey(baseCharacterName))
                                {
                                        _entries[baseCharacterName].Type = target;
                                }
                        }
                }

                private static void InitEntry(Entry entry)
                {
                        Network.AuthServer.Send(AuthServerMessage.FriendList, Network.AuthServer.TransactionCounter, (uint) entry.Type, entry.BaseCharacterName);
                }

                private static void UpdateEntry(Entry entry)
                {
                        Network.AuthServer.Send(AuthServerMessage.UpdateFriendList, (uint) entry.PlayerStatus, entry.BaseCharacterName, entry.CurrentCharacterName);

                        //If a map was specified, show it next to name
                        if (entry.Map != 0)
                        {
                                Network.AuthServer.Send(AuthServerMessage.FriendListLocationInfo, (uint) entry.Map,
                                                        (ushort) 0, (byte) 0, (byte) 0, // district info (not shown anyway)
                                                        entry.BaseCharacterName);
                        }
                }

                internal void Init()
                {
                        Entries.ToList().ForEach(InitEntry);
                }

                internal void Update()
                {
                        Entries.ToList().ForEach(UpdateEntry);
                }

                public void Clear()
                {
                        Entries.ToList().ForEach(e => Remove(e.BaseCharacterName));
                }

                public sealed class Entry
                {
                        public readonly string BaseCharacterName;
                        private string _currentCharacterName;
                        private Map _map;
                        private PlayerStatus _playerStatus;

                        internal Entry(Type type, string baseCharacterName, string currentCharacterName = "", PlayerStatus playerStatus = PlayerStatus.Offline, Map map = 0)
                        {
                                Type = type;
                                BaseCharacterName = baseCharacterName;
                                _currentCharacterName = currentCharacterName;
                                _playerStatus = playerStatus;
                                _map = map;
                        }

                        public Type Type { get; internal set; }

                        public string CurrentCharacterName
                        {
                                get { return _currentCharacterName; }
                                set
                                {
                                        _currentCharacterName = value;
                                        UpdateEntry(this);
                                }
                        }

                        public PlayerStatus PlayerStatus
                        {
                                get { return _playerStatus; }
                                set
                                {
                                        _playerStatus = value;
                                        UpdateEntry(this);
                                }
                        }

                        public Map Map
                        {
                                get { return _map; }
                                set
                                {
                                        _map = value;
                                        UpdateEntry(this);
                                }
                        }
                }
        }
}