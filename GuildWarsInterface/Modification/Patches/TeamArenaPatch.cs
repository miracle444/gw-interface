#region

using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Modification.Native;

#endregion

namespace GuildWarsInterface.Modification.Patches
{
        internal static class TeamArenaPatch
        {
                private static readonly IntPtr _location = (IntPtr) 0x008BCA5C;

                public static void Apply()
                {
                        uint dwOldProtection;
                        Kernel32.VirtualProtect(_location, 4, 0x40, out dwOldProtection);
                        Marshal.WriteInt32(_location, 0x40C0003);
                        Kernel32.VirtualProtect(_location, 4, dwOldProtection, out dwOldProtection);
                }
        }
}