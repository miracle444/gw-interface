#region

using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Modification.Native;

#endregion

namespace GuildWarsInterface.Modification.Patches
{
        internal static class PortPatch
        {
                private static readonly IntPtr _location = (IntPtr) 0x005B2ACA;

                public static void Apply()
                {
                        uint dwOldProtection;
                        Kernel32.VirtualProtect(_location, 1, 0x40, out dwOldProtection);
                        Marshal.WriteByte(_location, 0x8B);
                        Kernel32.VirtualProtect(_location, 1, dwOldProtection, out dwOldProtection);
                }
        }
}