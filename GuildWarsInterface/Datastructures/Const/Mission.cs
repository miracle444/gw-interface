using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Declarations;
using GuildWarsInterface.Modification.Native;

namespace GuildWarsInterface.Datastructures.Const
{
        public sealed class Mission
        {
                [Flags]
                public enum MissionFlags : uint
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

                public Mission(Map map)
                {
                        _address = _constMissionBase + 124 * (int) map;
                }

                public MissionFlags Flags
                {
                        get
                        {
                                try
                                {
                                        uint dwOldProtection;
                                        Kernel32.VirtualProtect(_address + 0x10, 4, 0x40, out dwOldProtection);
                                        var result = (MissionFlags)Marshal.ReadInt32(_address + 0x10);
                                        Kernel32.VirtualProtect(_address + 0x10, 4, dwOldProtection, out dwOldProtection);
                                        return result;
                                }
                                catch (Exception)
                                {
                                        return new MissionFlags();
                                }
                        }
                        set
                        {
                                uint dwOldProtection;
                                Kernel32.VirtualProtect(_address + 0x10, 4, 0x40, out dwOldProtection);
                                Marshal.WriteInt32(_address + 0x10, (int)value); 
                                Kernel32.VirtualProtect(_address + 0x10, 4, dwOldProtection, out dwOldProtection);
                        }
                }
        }
}