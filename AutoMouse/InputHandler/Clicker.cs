using AutoMouse.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace AutoMouse.InputHandler
{
    public class Clicker : IDisposable
    {
        private MouseButton button = MouseButton.Left;
        private readonly DispatcherTimer timer;
        private readonly GlobalKeyboardListener listener = new GlobalKeyboardListener();

        public event EventHandler<ClickingActiveState> OnClickEnableChanged;

        public Clicker()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Clicker_tick;

            listener.KeyDown += Listener_KeyDown;
            listener.KeyUp += Listener_KeyUp;
        }


        public bool IsEnabled { get; private set; } = false;


        private void Clicker_tick(object sender, EventArgs e)
        {
            if (!VirtualMouse.Click(button))
            {
                StopClick();
                Logger.Error("Invaild Click Type: " + button);
            }
        }

        private MouseButton? GetMouseType(Key activeKey)
        {
            if (activeKey == Key.None)
                return null;

            int keyPressed = (int)activeKey;
            if (keyPressed == Settings.Default.KeyLeftActive)
                return MouseButton.Left;
            else if (keyPressed == Settings.Default.KeyRightActive)
                return MouseButton.Right;

            return null;
        }

        private void StartClick()
        {
            timer.Interval = TimeSpan.FromMilliseconds(1000d / Settings.Default.ClickSpeed);
            timer.Start();

            IsEnabled = true;
            OnClickEnableChanged?.Invoke(this, new ClickingActiveState() { Button = button, IsEnabled = true });
        }

        private void StopClick()
        {
            timer.Stop();

            IsEnabled = false;
            OnClickEnableChanged?.Invoke(this, new ClickingActiveState() { Button = button, IsEnabled = false });
        }

        private void Listener_KeyDown(object sender, RawKeyEventArgs e)
        {
            Logger.Info(e.Key);
            MouseButton? mouseButton = GetMouseType(e.Key);

            if (mouseButton == null)
                return;

            if (Settings.Default.UseToggle && timer.IsEnabled)
            {
                if (button == mouseButton)
                    StopClick();
                else
                    button = mouseButton.Value;
                return;
            }

            button = mouseButton.Value;
            StartClick();
        }

        private void Listener_KeyUp(object sender, RawKeyEventArgs args)
        {
            if (!Settings.Default.UseToggle)
                StopClick();
        }

        public void Dispose()
        {
            listener.Dispose();
        }
    }

    public class ClickingActiveState
    {
        public MouseButton Button { get; set; }
        public bool IsEnabled { get; set; }

        public string GetButtonName()
        {
            switch (Button)
            {
                case MouseButton.Left:
                    return "왼쪽";
                case MouseButton.Right:
                    return "오른쪽";
                case MouseButton.Middle:
                    return "가운데";
                case MouseButton.XButton1:
                    return "확장1";
                case MouseButton.XButton2:
                    return "확장2";
                default:
                    Logger.Warn("Unkdown button detected: " + Button);
                    return "알 수 없음";
            }
        }
    }
}
