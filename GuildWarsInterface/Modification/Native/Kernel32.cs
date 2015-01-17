#region

using System;
using System.Runtime.InteropServices;

#endregion

namespace GuildWarsInterface.Modification.Native
{
        /// <summary>
        ///         collection of all pinvoke calls to kernel32
        /// </summary>
        public static class Kernel32
        {
                [DllImport("kernel32.dll")]
                public static extern bool VirtualProtect(IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint lpflOldProtect);

                [DllImport("kernel32.dll")]
                public static extern IntPtr GetModuleHandle(string lpModuleName);

                [DllImport("kernel32.dll")]
                public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
        }
}