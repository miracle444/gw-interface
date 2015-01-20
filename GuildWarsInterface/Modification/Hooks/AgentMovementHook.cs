using System;
using System.Runtime.InteropServices;
using Binarysharp.Assemblers.Fasm;

namespace GuildWarsInterface.Modification.Hooks
{
        internal class AgentMovementHook
        {
                internal static void Install(HookType hook)
                {
                        var hookLocation = new IntPtr(0x007B25F6);

                        IntPtr codeCave = Marshal.AllocHGlobal(128);

                        byte[] code = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + codeCave,
                                        "mov dword[esi+0x88],ecx",
                                        "pushad",
                                        "push dword[esi+0x2C]",
                                        "call " + Marshal.GetFunctionPointerForDelegate(hook),
                                        "popad",
                                        "jmp " + (hookLocation + 6)
                                });
                        Marshal.Copy(code, 0, codeCave, code.Length);

                        HookHelper.Jump(hookLocation, codeCave);
                }

                [UnmanagedFunctionPointer(CallingConvention.StdCall)]
                internal delegate void HookType(uint id);
        }
}