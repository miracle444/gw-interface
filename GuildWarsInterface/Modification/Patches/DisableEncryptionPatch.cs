#region

using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Modification.Native;

#endregion

namespace GuildWarsInterface.Modification.Patches
{
        internal static class DisableEncryptionPatch
        {
                private static readonly IntPtr _location = (IntPtr) 0x0058FEAB;

                public static void Apply()
                {
                        uint dwOldProtection;
                        Kernel32.VirtualProtect(_location, 5, 0x40, out dwOldProtection);
                        Marshal.Copy(new byte[] {0x8A, 0x14, 0x18}, 0, _location, 3);
                        Kernel32.VirtualProtect(_location, 5, dwOldProtection, out dwOldProtection);
                }
        }
}