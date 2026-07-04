using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MarkdownPreviewer
{
    internal static class IconUtility
    {
        internal const int IDI_APPLICATION = 32512;

        public static Icon GetExeIcon()
        {
            return Icon.FromHandle(LoadIcon(GetModuleHandle(null), new IntPtr(IDI_APPLICATION)));
        }

        [DllImport("Kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("User32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern IntPtr LoadIcon(IntPtr handle, IntPtr iconName);
    }
}