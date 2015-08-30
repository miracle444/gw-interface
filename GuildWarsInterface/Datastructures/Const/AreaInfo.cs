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

                // unknown@0x0: [0, 5]
                // unknown@0x18: [1, 8] \ 6
                // unknown@0x20: {1, 2}
                // unknown@0x24: {1, 2, 4, 5, 6, 8, 12}
                // unknown@0x28: [0, 0x12]
                // unknown@0x2C: {0, 1, 4}
                // unknown@0x30: {1, 20} related to solo arenas
                // unknown@0x34: {10, 16, 20}
                // unknown@0x38: {502, 505, 506, 507, 550, 552, 555, 581, 584, 586, 634, 639, 681, 701, 703, 1440}
                // unknown@0x3C: {4, 5, 6, 10, 16, 19, 20, 21, 23, 24, 29, 32, 33, 35, 38, 49, 51, 52, 55, 109, 120, 123, 124, 130, 133, 134, 135, 138, 139, 140, 142, 
                //                149, 167, 168, 170, 171, 172, 173, 174, 175, 176, 177, 178, 179, 185, 186, 194, 212, 213, 214, 216, 217, 218, 219, 220, 222, 224, 225,
                //                226, 242, 250, 251, 271, 272, 273, 274, 275, 276, 277, 279, 283, 284, 290, 291, 292, 293, 294, 295, 296, 297, 298, 310, 311, 312, 328, 
                //                329, 331, 332, 333, 334, 335, 336, 337, 338, 351, 355, 356, 357, 358, 359, 360, 370, 378, 381, 387, 403, 414, 415, 420, 421, 424, 425,
                //                426, 427, 428, 431, 433, 434, 435, 440, 449, 450, 456, 469, 474, 476, 477, 478, 480, 482, 485, 490, 491, 492, 493, 494, 495, 496, 503, 
                //                513, 529, 530, 533, 534, 537, 538, 541, 542, 543, 544, 545, 546, 553, 558, 566, 569, 624, 625, 638, 639, 640, 643, 644, 645, 646, 647, 
                //                648, 649, 650, 651, 652, 664, 665, 669, 675, 679, 680, 689, 694, 710, 783, 844, 857, 877}
                // unknown@0x48: related to compass map

                public Continent Continent
                {
                        get { return (Continent) ReadInt(0x4); }
                        set { WriteInt(0x4, (int) value); }
                }

                public Region Region
                {
                        get { return (Region) ReadInt(0x8); }
                        set { WriteInt(0x8, (int) value); }
                }

                public RegionType RegionType
                {
                        get { return (RegionType) ReadInt(0xC); }
                        set { WriteInt(0xC, (int) value); }
                }

                public AreaFlags Flags
                {
                        get { return (AreaFlags) ReadInt(0x10); }
                        set { WriteInt(0x10, (int) value); }
                }

                public int Thumbnail
                {
                        get { return ReadInt(0x14); }
                        set { WriteInt(0x14, value); }
                }

                public int PartySize
                {
                        get { return ReadInt(0x1C); }
                        set { WriteInt(0x1C, value); }
                }

                public int X
                {
                        get { return ReadInt(0x40); }
                        set { WriteInt(0x40, value); }
                }

                public int Y
                {
                        get { return ReadInt(0x44); }
                        set { WriteInt(0x44, value); }
                }

                public int Name
                {
                        get { return ReadInt(0x74); }
                        set { WriteInt(0x74, value); }
                }

                public int Description
                {
                        get { return ReadInt(0x78); }
                        set { WriteInt(0x78, value); }
                }

                private int ReadInt(int offset)
                {
                        uint dwOldProtection;
                        Kernel32.VirtualProtect(_address + offset, 4, 0x40, out dwOldProtection);
                        int result = Marshal.ReadInt32(_address + offset);
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
        }
}