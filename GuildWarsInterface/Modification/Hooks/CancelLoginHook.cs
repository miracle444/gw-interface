using System;
using System.Runtime.InteropServices;
using Binarysharp.Assemblers.Fasm;

namespace GuildWarsInterface.Modification.Hooks
{
        internal class CancelLoginHook
        {
                internal static void Install(HookType hook)
                {
                        var hookLocation = new IntPtr(0x0A2B2C0);

                        IntPtr codeCave = Marshal.AllocHGlobal(128);

                        byte[] code = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + codeCave,
                                        "pushad",
                                        "call " + Marshal.GetFunctionPointerForDelegate(hook),
                                        "popad",
                                        "retn"
                                });
                        Marshal.Copy(code, 0, codeCave, code.Length);

                        Marshal.WriteIntPtr(hookLocation, codeCave);
                }

                [UnmanagedFunctionPointer(CallingConvention.StdCall)]
                internal delegate void HookType();
        }
}