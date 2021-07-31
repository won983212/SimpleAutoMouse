using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AutoMouse.InputHandler;
using AutoMouse.Controls;
using AutoMouse.Properties;

namespace AutoMouse.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private readonly System.Windows.Forms.NotifyIcon notifyIcon;
        private readonly ContextMenu contextMenu;
        private readonly TransparentWindow activeAlertPanel;

        private bool isShowWindow = true;
        private bool isEnabled = true;


        public MainWindowViewModel()
        {
            contextMenu = new ContextMenu();
            contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;

            MenuItem item = new MenuItem() { Header = "종료" };
            item.Click += (s, e) =>
            {
                notifyIcon.Dispose();
                Application.Current.Shutdown();
            };
            contextMenu.Items.Add(item);


            notifyIcon = new System.Windows.Forms.NotifyIcon();
            notifyIcon.Text = "Auto Input";
            notifyIcon.MouseDown += NotifyIcon_MouseDown;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;

            using (Stream s = Application.GetResourceStream(new Uri("Resources/Icon.ico", UriKind.Relative)).Stream)
                notifyIcon.Icon = new System.Drawing.Icon(s);
            notifyIcon.Visible = true;

            // initialize app init
            activeAlertPanel = new TransparentWindow();
            activeAlertPanel.Left = 20;
            activeAlertPanel.Top = 20;

            ((App)Application.Current).ClickExecutor.OnClickEnableChanged += ClickExecutor_OnClickEnableChanged;
        }


        public bool IsShowWindow
        {
            get => isShowWindow;
            set => SetProperty(ref isShowWindow, value);
        }

        public bool IsEnabled
        {
            get => isEnabled;
            set => SetProperty(ref isEnabled, value);
        }


        private void ClickExecutor_OnClickEnableChanged(object sender, ClickingActiveState e)
        {
            isEnabled = e.IsEnabled;
            if (Settings.Default.UseAlert)
            {
                activeAlertPanel.InfoLabel.Content = e.GetButtonName() + " 버튼 클릭 활성화";
                if (e.IsEnabled)
                    activeAlertPanel.Show();
                else
                    activeAlertPanel.Hide();
            }
        }

        private void NotifyIcon_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            contextMenu.IsOpen = true;
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            IsShowWindow = true;
        }
    }
}
