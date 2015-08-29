#region

using System;
using System.Runtime.InteropServices;
using GuildWarsInterface.Modification.Native;

#endregion

namespace GuildWarsInterface.Modification.Hooks
{
        internal static class GetHostByNameHook
        {
                private static HookType _hookDelegate;
                private static HookType _originalDelegate;

                private static readonly IntPtr _hookAddress = (IntPtr) 0x005CC67C;

                public static string LastHostName { get; private set; }

                public static void Install()
                {
                        _hookDelegate = Hook;

                        IntPtr addr = Kernel32.GetProcAddress(Kernel32.GetModuleHandle("ws2_32.dll"), "WSAAsyncGetHostByName");

                        _originalDelegate = (HookType) Marshal.GetDelegateForFunctionPointer(addr, typeof (HookType));

                        HookHelper.Jump(_hookAddress, Marshal.GetFunctionPointerForDelegate(_hookDelegate));
                }

                private static int Hook(IntPtr hWnd, uint wMsg, IntPtr name, IntPtr buf, int buflen)
                {
                        LastHostName = Marshal.PtrToStringAnsi(name);

                        return _originalDelegate(hWnd, wMsg, name, buf, buflen);
                }

                [UnmanagedFunctionPointer(CallingConvention.StdCall)]
                private delegate int HookType(IntPtr hWnd, uint wMsg, IntPtr name, IntPtr buf, int buflen);
        }
}