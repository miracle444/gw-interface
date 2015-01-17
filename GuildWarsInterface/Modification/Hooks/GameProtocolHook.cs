#region

using System;
using System.Runtime.InteropServices;
using Binarysharp.Assemblers.Fasm;

#endregion

namespace GuildWarsInterface.Modification.Hooks
{
        internal class GameProtocolHook
        {
                internal static void Install(HookType hook)
                {
                        var hookLocation = new IntPtr(0x004078F1);

                        IntPtr codeCave = Marshal.AllocHGlobal(128);

                        byte[] code = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + codeCave,
                                        "pushad",
                                        "push edx",
                                        "call " + Marshal.GetFunctionPointerForDelegate(hook),
                                        "popad",
                                        "call 0x00409890",
                                        "jmp " + (hookLocation + 5)
                                });
                        Marshal.Copy(code, 0, codeCave, code.Length);

                        HookHelper.Jump(hookLocation, codeCave);
                }

                [UnmanagedFunctionPointer(CallingConvention.StdCall)]
                internal delegate void HookType(IntPtr gameProtocol);
        }
}