using System;
using System.Runtime.InteropServices;
using Binarysharp.Assemblers.Fasm;
using System.IO;
using GuildWarsInterface.Modification.Native;

namespace GuildWarsInterface.Modification.Hooks
{
        internal class PacketSnifferHook
        {
                private static HookType _hookDelegate;
                private static readonly IntPtr _hookAddress = (IntPtr)0x5900CB;
                private const string filePath = @"E:\Users\Etienne2\Desktop\packetlog.txt";

                public static void Install()
                {

                        _hookDelegate = Hook;

                        IntPtr codeCave = Marshal.AllocHGlobal(128);

                        byte[] code = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + codeCave,
                                        "pushad",
                                        "push esi",
                                        "add eax, ebx",
                                        "sub eax, esi",
                                        "push eax",
                                        "call " + Marshal.GetFunctionPointerForDelegate(_hookDelegate),
                                        "popad",
                                        "mov [ecx+4], edi",
                                        "pop ebx",
                                        "mov [ecx], esi",
                                        "jmp " + (_hookAddress + 6)
                                });
                        Marshal.Copy(code, 0, codeCave, code.Length);

                        Jump(_hookAddress, codeCave);
                }

                private static int Hook(IntPtr buf, int len)
                {
                        byte[] buffer = new byte[len];
                        Marshal.Copy(buf, buffer, 0, len);
                        File.AppendAllText(filePath, BitConverter.ToString(buffer).Replace("-", " "));
                        return 0;
                }

                private static void Jump(IntPtr from, IntPtr to)
                {
                        byte[] hook = FasmNet.Assemble(new[]
                                {
                                        "use32",
                                        "org " + from,
                                        "jmp " + to,
                                        "nop"
                                });

                        uint dwOldProtection;
                        Kernel32.VirtualProtect(from, 5, 0x40, out dwOldProtection);
                        Marshal.Copy(hook, 0, from, 5);
                        Kernel32.VirtualProtect(from, 5, dwOldProtection, out dwOldProtection);
                }

                [UnmanagedFunctionPointer(CallingConvention.StdCall)]
                private delegate int HookType(IntPtr buf, int len);

        } 
}