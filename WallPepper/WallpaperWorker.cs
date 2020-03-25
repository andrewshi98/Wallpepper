using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace WallPepper
{
    static class WallpaperWorker
    {
        private static int hMouseHook;
        public static void getWorkerHandle(out IntPtr handle)
        {

            // Fetch the Progman window
            IntPtr progman = WinHelper.FindWindow("Progman", null);
            // Send 0x052C to Progman. This message directs Progman to spawn a 
            // WorkerW behind the desktop icons. If it is already there, nothing 
            // happens.
            WinHelper.SendMessageTimeout(progman,
                           0x052C,
                           new IntPtr(0),
                           IntPtr.Zero,
                           WinHelper.SendMessageTimeoutFlags.SMTO_NORMAL,
                           1000,
                           out handle);

            IntPtr workerw = IntPtr.Zero;
            WinHelper.EnumWindows(new WinHelper.EnumWindowsProc((tophandle, topparamhandle) =>
            {
                IntPtr p = WinHelper.FindWindowEx(tophandle,
                                            IntPtr.Zero,
                                            "SHELLDLL_DefView",
                                            IntPtr.Zero);

                if (p != IntPtr.Zero)
                {
                    // Gets the WorkerW Window after the current one.
                    workerw = WinHelper.FindWindowEx(IntPtr.Zero,
                                               tophandle,
                                               "WorkerW",
                                               IntPtr.Zero);
                }

                return true;
            }), IntPtr.Zero);

            while (workerw == IntPtr.Zero) { }
            handle = workerw;
        }

        public static void registerMouseHandler(int procId)
        {
            WinHelper.LowLevelMouseProc mouseProc = new WinHelper.LowLevelMouseProc(handleMouseActivity);
            //hMouseHook = WinHelper.SetWindowsHookEx(WinHelper.WindowsHookFlags.WH_MOUSE_LL, mouseProc, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
        }

        public static IntPtr handleMouseActivity(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return IntPtr.Zero;
            if (nCode < 0)
            {
                return WinHelper.CallNextHookEx((IntPtr)hMouseHook, nCode, wParam, lParam);
            }
            //MessageBox.Show("Called");
            return WinHelper.CallNextHookEx((IntPtr)hMouseHook, nCode, wParam, lParam);
        }
    }
}