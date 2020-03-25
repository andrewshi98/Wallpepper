using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace WallPepper
{
	//Some imported functions from user32.dll
	static class WinHelper
	{
		[Flags]
		public enum SendMessageTimeoutFlags : uint
		{
			SMTO_NORMAL = 0x0,
			SMTO_BLOCK = 0x1,
			SMTO_ABORTIFHUNG = 0x2,
			SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
			SMTO_ERRORONEXIT = 0x20
		}

		[Flags]
		public enum WindowsHookFlags : int
		{
			WH_KEYBOARD = 2,
			WH_KEYBOARD_LL = 13,
			WH_MOUSE = 7,
			WH_MOUSE_LL = 14
		}

		// Used to monitor mouse and keyboard activity
		public delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern int SetWindowsHookEx(
		   WindowsHookFlags idHook,
		   LowLevelMouseProc lpfn,
		   IntPtr hInstance,
		   int threadId
		   );

		[DllImport("kernel32.dll")]
		public static extern int GetProcessId(IntPtr handle);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);

		//Sets a window to be a child window of another window
		[DllImport("user32.dll")]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		[DllImport("user32.dll")]
		public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		//Sets window attributes
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, winStyle dwNewLong);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessageTimeout(IntPtr windowHandle, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags flags, uint timeout, out IntPtr result);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

		public static int GWL_STYLE = -16;

		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr SetFocus(IntPtr hWnd);

		[Flags]
		public enum winStyle : int
		{
			WS_VISIBLE = 0x10000000,
			WS_CHILD = 0x40000000, //child window
								   //WS_BORDER = 0x00800000, //window with border
								   //WS_DLGFRAME = 0x00400000, //window with double border but no title
								   //WS_CAPTION = WS_BORDER | WS_DLGFRAME //window with a title bar
		}
	}
}
