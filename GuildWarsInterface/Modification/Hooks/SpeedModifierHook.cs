using System;
using System.Runtime.InteropServices;
using Binarysharp.Assemblers.Fasm;

namespace GuildWarsInterface.Modification.Hooks
{
        internal class SpeedModifierHook
        {
                private static IntPtr _speedModifierLocation;

                internal static float SpeedModifier
                {
                        get
                        {
                                try
                                {
                                        var data = Marshal.ReadIntPtr(_speedModifierLocation);
                                        return BitConverter.ToSingle(BitConverter.GetBytes(Marshal.ReadInt32(data + 0x60)), 0);
                                }
                                catch (AccessViolationException)
                                {
                                        return 0;
                                }
                        }
                }

                internal static void Install()
                {
                        var hookLocation = new IntPtr(0x005D2EF1);

                        IntPtr codeCave = Marshal.AllocHGlobal(128);
                        _speedModifierLocation = Marshal.AllocHGlobal(4);

                        byte[] code = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + codeCave,
                                        "mov ebx, dword[ecx*0x4+eax]",
                                        "mov dword[" + _speedModifierLocation + "], ebx",
                                        "test ebx, ebx",
                                        "jmp " + (hookLocation + 5)
                                });
                        Marshal.Copy(code, 0, codeCave, code.Length);

                        HookHelper.Jump(hookLocation, codeCave);
                }
        }
}