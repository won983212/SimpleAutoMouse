﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace AutoMouse.InputHandler
{
    public class GlobalKeyboardListener : IDisposable
    {
        private static IntPtr hookId = IntPtr.Zero;
        private readonly InterceptKeys.LowLevelKeyboardProc callback;

        [MethodImpl(MethodImplOptions.NoInlining)]
        private IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                return HookCallbackInner(nCode, wParam, lParam);
            }
            catch (Exception e)
            {
                Logger.Error("There was some error somewhere...");
                Logger.Error(e);
            }
            return InterceptKeys.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        private IntPtr HookCallbackInner(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                if (wParam == (IntPtr)InterceptKeys.WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    KeyDown?.Invoke(this, new RawKeyEventArgs(vkCode, false));
                }
                else if (wParam == (IntPtr)InterceptKeys.WM_KEYUP)
                {
                    int vkCode = Marshal.ReadInt32(lParam);
                    KeyUp?.Invoke(this, new RawKeyEventArgs(vkCode, false));
                }
            }
            return InterceptKeys.CallNextHookEx(hookId, nCode, wParam, lParam);
        }

        public event RawKeyEventHandler KeyDown;
        public event RawKeyEventHandler KeyUp;

        public GlobalKeyboardListener()
        {
            callback = HookCallback;
            hookId = InterceptKeys.SetHook(callback);
        }

        ~GlobalKeyboardListener()
        {
            Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
            InterceptKeys.UnhookWindowsHookEx(hookId);
        }

        #endregion
    }

    internal static class InterceptKeys
    {
        public delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        public static int WH_KEYBOARD_LL = 13;
        public static int WM_KEYDOWN = 0x0100;
        public static int WM_KEYUP = 0x0101;

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
    }

    public class RawKeyEventArgs : EventArgs
    {
        public int VKCode;
        public Key Key;
        public bool IsSysKey;

        public RawKeyEventArgs(int VKCode, bool isSysKey)
        {
            this.VKCode = VKCode;
            this.IsSysKey = isSysKey;
            this.Key = KeyInterop.KeyFromVirtualKey(VKCode);
        }
    }

    public delegate void RawKeyEventHandler(object sender, RawKeyEventArgs args);
}
