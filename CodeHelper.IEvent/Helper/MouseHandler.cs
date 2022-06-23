using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Helper
{
    public  class MouseHandler
    {
        public MouseHandler()
        {
            MouseProc = HookCallback;
        }

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        private static extern bool SetCursorPos(int x, int y);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string name);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hook, int nCode, IntPtr wp, IntPtr lp);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hook);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private LowLevelMouseProc MouseProc { get; set; }
        private static IntPtr hookId = IntPtr.Zero;

        private static bool IsVisible;
        public event EventHandler<MouseEventArgs>? MouseActivity;
        public event EventHandler<Info>? StateInfo;


        private const int WH_MOUSE_LL = 14;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_LBUTTONDBLCLK = 0x0203,
            WM_RBUTTONUP = 0x0205
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        private IntPtr Hook(LowLevelMouseProc process)
        {
            using Process currentProcess = Process.GetCurrentProcess();
            using ProcessModule currentModule = currentProcess.MainModule;
            StateInfo(this, new Info
            {
                Message = "ProcessModule Started"
            });
            return SetWindowsHookEx(WH_MOUSE_LL, process, GetModuleHandle(currentModule.ModuleName), 0);
        }
        private  IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (IsVisible)
                {
                    StateInfo(this, new Info
                    {
                        Message = "Is Visible"
                    });
                    MouseButtons button = MouseButtons.None;
                    MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                    short mouseDelta = 0;
                    switch ((MouseMessages)wParam)
                    {
                        case MouseMessages.WM_LBUTTONDOWN:
                            button = MouseButtons.Left;
                            break;
                        case MouseMessages.WM_RBUTTONDOWN:
                            button = MouseButtons.Right;
                            break;
                        case MouseMessages.WM_MOUSEWHEEL:
                            mouseDelta = (short)((hookStruct.mouseData >> 16) & 0xffff);
                            break;
                    }
                    int clickCount = 0;
                    if (button != MouseButtons.None)
                        if ((MouseMessages)wParam == MouseMessages.WM_LBUTTONDBLCLK) clickCount = 2;
                        else clickCount = 1;
                    MouseActivity(null, new MouseEventArgs(button, clickCount, hookStruct.pt.x, hookStruct.pt.y, mouseDelta));
                }
                else return (IntPtr)1;
            }
            StateInfo(new object(), new Info
            {
                Message = "Call next hook"
            });
            return CallNextHookEx(hookId, nCode, wParam, lParam);
        }
        public void MouseHook(int left, int top)
        {
            SetCursorPos(left, top);
            hookId = Hook(MouseProc);
        }
        public  void Start(bool isVisible = true, int left = 0, int top = 0)
        {
            StateInfo(this, new Info
            {
                Message = "Main start"
            });

            IsVisible = isVisible;
            if (!IsVisible) SetCursorPos(left, top);
            Hook(MouseProc);
        }
        public void UnHook()
        {
            UnhookWindowsHookEx(hookId);
        }
    }
}
