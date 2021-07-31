using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;

namespace AutoMouse.InputHandler
{
    internal static class VirtualMouse
    {
        [DllImport("user32.dll")]
        private static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        // left
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;

        // right
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;


        public static bool Click(MouseButton button)
        {
            int downFlag;
            int upFlag;

            switch (button)
            {
                case MouseButton.Left:
                    downFlag = MOUSEEVENTF_LEFTDOWN;
                    upFlag = MOUSEEVENTF_LEFTUP;
                    break;
                case MouseButton.Right:
                    downFlag = MOUSEEVENTF_RIGHTDOWN;
                    upFlag = MOUSEEVENTF_RIGHTUP;
                    break;
                default:
                    return false;
            }

            mouse_event(downFlag, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            mouse_event(upFlag, Control.MousePosition.X, Control.MousePosition.Y, 0, 0);
            return true;
        }
    }
}
