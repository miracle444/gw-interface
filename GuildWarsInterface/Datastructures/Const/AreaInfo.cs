using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Modification.Native;

namespace GuildWarsInterface.Datastructures.Const
{
        public class AreaInfo
        {
                [Flags]
                public enum AreaFlags : uint
                {
                        Unknown0 = 0x0,
                        Unknown1 = 0x1,
                        Unknown2 = 0x2,
                        Unknown3 = 0x4,
                        Unknown4 = 0x8,
                        Unknown5 = 0x10,
                        HideOnWorldmap = 0x20,
                        Unknown7 = 0x40,
                        Unknown8 = 0x80,
                        Unknown9 = 0x100,
                        Unknown10 = 0x200,
                        Unknown11 = 0x400,
                        Unknown12 = 0x800,
                        Unknown13 = 0x1000,
                        Unknown14 = 0x2000,
                        Unknown15 = 0x4000,
                        Unknown16 = 0x8000,
                        Unknown17 = 0x10000,
                        Unknown18 = 0x20000,
                        CaptionEnterBattle = 0x40000,
                        OnlyShowEnterBattle = 0x80000,
                        CaptionChooseMission = 0x100000,
                        Unknown22 = 0x200000,
                        Unknown23 = 0x400000,
                        Unknown24 = 0x800000,
                        HideEnterButton = 0x1000000,
                        Unknown26 = 0x2000000,
                        Unknown27 = 0x4000000,
                        Unknown28 = 0x8000000,
                        Unknown29 = 0x10000000,
                        Unknown30 = 0x20000000,
                        Unknown31 = 0x40000000,
                        Unknown32 = 0x80000000,
                }

                private readonly IntPtr _address;
                private readonly IntPtr _constMissionBase = (IntPtr) 0x008B6EE0;

                internal AreaInfo(Map map)
                {
                        _address = _constMissionBase + 124 * (int) map;
                }

                private int ReadInt(int offset)
                {
                        uint dwOldProtection;
                        Kernel32.VirtualProtect(_address + offset, 4, 0x40, out dwOldProtection);
                        var result = Marshal.ReadInt32(_address + offset);
                        Kernel32.VirtualProtect(_address + offset, 4, dwOldProtection, out dwOldProtection);
                        return result;
                }

                private void WriteInt(int offset, int value)
                {
                        uint dwOldProtection;
                        Kernel32.VirtualProtect(_address + offset, 4, 0x40, out dwOldProtection);
                        Marshal.WriteInt32(_address + offset, value);
                        Kernel32.VirtualProtect(_address + offset, 4, dwOldProtection, out dwOldProtection);
                }

                public AreaType Type
                {
                        get
                        {
                                return (AreaType) ReadInt(0xC);
                        }
                        set
                        {
                                WriteInt(0xC, (int) value);
                        }
                }

                public Continent Continent
                {
                        get
                        {
                                return (Continent)ReadInt(0x4);
                        }
                        set
                        {
                                WriteInt(0x4, (int)value);
                        }
                }

                public AreaFlags Flags
                {
                        get
                        {
                                return (AreaFlags)ReadInt(0x10);
                        }
                        set
                        {
                                WriteInt(0x10, (int)value);
                        }
                }

                public byte[] All
                {
                        get
                        {
                                var result = new byte[124];
                                uint dwOldProtection;
                                Kernel32.VirtualProtect(_address, 124, 0x40, out dwOldProtection);
                                for (var i = 0; i < 124; i++)
                                {
                                        result[i] = Marshal.ReadByte(_address + i);
                                }
                                Kernel32.VirtualProtect(_address, 124, dwOldProtection, out dwOldProtection);
                                return result;
                        }
                        set {
                                if (value.Length != 124) return;
                                uint dwOldProtection;
                                Kernel32.VirtualProtect(_address, 124, 0x40, out dwOldProtection);
                                for (var i = 0; i < 124; i++)
                                {
                                        if (i < 0x4 || i >= 0x10) continue;
                                        Marshal.WriteByte(_address + i, value[i]);
                                }
                                Kernel32.VirtualProtect(_address, 124, dwOldProtection, out dwOldProtection);
                        }
                }
        }

        public static class AreaInfoExtension
        {
                public static AreaInfo Info(this Map map)
                {
                        return new AreaInfo(map);
                }
        }
}