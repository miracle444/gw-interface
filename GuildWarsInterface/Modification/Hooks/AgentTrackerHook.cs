using System;
using System.Runtime.InteropServices;
using Binarysharp.Assemblers.Fasm;

namespace GuildWarsInterface.Modification.Hooks
{
        internal class AgentTrackerHook
        {
                internal static void Install(HookType hook)
                {
                        var myIdLocation = new IntPtr(0x00D55964);
                        var hookLocation1 = new IntPtr(0x005D5061);
                        var hookLocation = new IntPtr(0x005D5240); //new IntPtr(0x005D5061));

                        IntPtr codeCave2 = Marshal.AllocHGlobal(4);

                        IntPtr codeCave1 = Marshal.AllocHGlobal(128);

                        byte[] code1 = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + codeCave1,
                                        "push esi",
                                        "push edi",

                                        "mov esi, dword[ebx+0x10]", // safe agentid
                                        "mov dword[" + codeCave2 + "],esi",

                                        "lea esi, dword[ebx+0x78]",

                                        "jmp " + (hookLocation1 + 5)
                                });
                        Marshal.Copy(code1, 0, codeCave1, code1.Length);

                        HookHelper.Jump(hookLocation1, codeCave1);


                         IntPtr codeCave = Marshal.AllocHGlobal(128);

                         byte[] code = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + codeCave,
                                        "mov ecx, dword[" + codeCave2 + "]",
                                        "cmp ecx, dword[" + myIdLocation + "]",
                                        "jne skip",

                                        "pushad",
                                        "push dword [ebp+0x8]",
                                        "call " + Marshal.GetFunctionPointerForDelegate(hook),
                                        "popad",

                                        "skip:",
                                        "pop edi",
                                        "pop esi",
                                        "pop ebx",
                                        "leave",
                                        "retn 8"
                                });
                        Marshal.Copy(code, 0, codeCave, code.Length);

                        HookHelper.Jump(hookLocation, codeCave);
                }

                [UnmanagedFunctionPointer(CallingConvention.StdCall)]
                internal delegate void HookType(IntPtr data);
        }
}