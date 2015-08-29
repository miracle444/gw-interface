#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

#endregion

namespace GuildWarsInterface.Networking.Protocol
{
        internal class Protocol
        {
                private readonly IntPtr _address;

                internal Protocol(IntPtr address)
                {
                        _address = address;
                }

                public Protocol Next
                {
                        get { return new Protocol(Marshal.ReadIntPtr(_address + 0xC)); }
                }

                private int Size
                {
                        get { return Marshal.ReadInt32(_address + 0x30); }
                }

                public IEnumerable<KeyValuePair<int, IntPtr>> Handlers
                {
                        get
                        {
                                for (int i = 0; i < Size; i++)
                                {
                                        yield return new KeyValuePair<int, IntPtr>(i, Marshal.ReadIntPtr(Marshal.ReadIntPtr(_address + 0x2C) + 0xC * i + 8));
                                }
                        }
                }

                public uint PrefixSize(int messageId, int field)
                {
                        var stuff = new List<int>();

                        if (messageId > Size) return 0;

                        int current = 1;
                        int actual = 0;

                        while (actual++ < field)
                        {
                                int a = Marshal.ReadInt32(Marshal.ReadIntPtr(Marshal.ReadIntPtr(_address + 0x2C) + 0xC * messageId) + current++ * 0x4) & 0xF;

                                stuff.Add(a);

                                switch (a)
                                {
                                        case 6:
                                        case 10:
                                                actual--;
                                                break;
                                        case 7:
                                        case 11:
                                                if (field == actual) return 2;
                                                break;
                                        case 12:
                                                if (field == actual) return 1;
                                                actual--;
                                                break;
                                }
                        }

                        return 0;
                }

                public bool Deserialize(byte[] data, out List<object> packet, out byte[] remainingData)
                {
                        using (var stream = new MemoryStream(data))
                        using (var reader = new BinaryReader(stream))
                        {
                                IntPtr message = Marshal.ReadIntPtr(_address + 0x1C) + 0x8 * reader.ReadUInt16();

                                packet = new List<object> {Marshal.ReadInt32(Marshal.ReadIntPtr(message))};

                                for (int i = 1; i < Marshal.ReadInt32(message + 0x4); i++)
                                {
                                        int parameter = Marshal.ReadInt32(Marshal.ReadIntPtr(message) + i * 0x4);

                                        switch (parameter & 0xF)
                                        {
                                                case 0:
                                                        packet.Add(reader.ReadUInt32());
                                                        break;
                                                case 1:
                                                        packet.Add(reader.ReadSingle());
                                                        break;
                                                case 2:
                                                        packet.Add(new[] {reader.ReadSingle(), reader.ReadSingle()});
                                                        break;
                                                case 3:
                                                        packet.Add(new[] {reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()});
                                                        break;
                                                case 4:
                                                        switch (parameter >> 8)
                                                        {
                                                                case sizeof (Byte):
                                                                        packet.Add(reader.ReadByte());
                                                                        break;
                                                                case sizeof (UInt16):
                                                                        packet.Add(reader.ReadUInt16());
                                                                        break;
                                                                case sizeof (UInt32):
                                                                        packet.Add(reader.ReadUInt32());
                                                                        break;
                                                                default:
                                                                        throw new ArgumentException();
                                                        }
                                                        break;
                                                case 5:
                                                case 9:
                                                        packet.Add(reader.ReadBytes(parameter >> 8));
                                                        break;
                                                case 7:
                                                        var s = new char[reader.ReadUInt16()];
                                                        for (int j = 0; j < s.Length; j++)
                                                                s[j] = (char) reader.ReadUInt16();
                                                        packet.Add(new string(s));
                                                        break;
                                                case 11:
                                                        ushort length = reader.ReadUInt16();

                                                        switch ((parameter >> 4) & 0xF)
                                                        {
                                                                case 0:
                                                                        packet.Add(reader.ReadBytes(length));
                                                                        break;
                                                                case 1:
                                                                        var sa = new ushort[length];
                                                                        for (int j = 0; j < length; j++)
                                                                                sa[j] = reader.ReadUInt16();
                                                                        packet.Add(sa);
                                                                        break;
                                                                case 2:
                                                                        var ia = new uint[length];
                                                                        for (int j = 0; j < length; j++)
                                                                                ia[j] = reader.ReadUInt32();
                                                                        packet.Add(ia);
                                                                        break;
                                                                default:
                                                                        throw new ArgumentException();
                                                        }
                                                        break;
                                                case 6:
                                                case 10:
                                                        break;
                                                default:
                                                        int a = parameter & 0xF;
                                                        break;
                                        }
                                }

                                remainingData = reader.ReadBytes((int) (stream.Length - stream.Position));
                                return true;
                        }
                }
        }
}