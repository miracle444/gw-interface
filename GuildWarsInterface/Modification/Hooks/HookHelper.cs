#region

using System;
using System.Runtime.InteropServices;
using Binarysharp.Assemblers.Fasm;
using GuildWarsInterface.Modification.Native;

#endregion

namespace GuildWarsInterface.Modification.Hooks
{
        internal static class HookHelper
        {
                public static void Jump(IntPtr from, IntPtr to)
                {
                        byte[] hook = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + from,
                                        "jmp " + to
                                });

                        uint dwOldProtection;
                        Kernel32.VirtualProtect(from, 5, 0x40, out dwOldProtection);
                        Marshal.Copy(hook, 0, from, 5);
                        Kernel32.VirtualProtect(from, 5, dwOldProtection, out dwOldProtection);
                }
        }
}