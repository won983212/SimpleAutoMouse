using AutoMouse.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace AutoMouse
{
	/// <summary>
	/// App.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class App : Application
	{
		private Properties.Settings setting = Settings.Default;
		private bool isLeftMode = true;
		private DispatcherTimer clicker;
		private GlobalKeyboardListener listener = new GlobalKeyboardListener();
		private TransparentWindow infoPanel;
		private MainWindow Wnd = null;

		public App()
		{
			clicker = new DispatcherTimer();
			clicker.Interval = TimeSpan.FromMilliseconds(10);
			clicker.Tick += Clicker_tick;
		}

		public void SetupMainWindow(MainWindow wnd)
		{
			Wnd = wnd;
			infoPanel = new TransparentWindow();
			infoPanel.Left = 20;
			infoPanel.Top = 20;
		}

		private void Clicker_tick(object sender, EventArgs e)
		{
			if (isLeftMode)
				VirtualMouse.LeftClick();
			else
				VirtualMouse.RightClick();
		}

		private void StartClick()
		{
			clicker.Interval = TimeSpan.FromMilliseconds(1000d / setting.ClickSpeed);
			clicker.Start();
			Wnd.DisableControls();

			if (setting.UseAlert)
			{
				infoPanel.InfoLabel.Content = isLeftMode ? "왼쪽 클릭 활성화" : "오른쪽 클릭 활성화";
				infoPanel.Show();
			}
		}

		private void StopClick()
		{
			clicker.Stop();
			Wnd.EnableControls();

			if (setting.UseAlert)
				infoPanel.Hide();
		}

		private void Listener_KeyDown(object sender, RawKeyEventArgs e)
		{
			if (setting.UseToggle)
			{
				if (e.Key.ToString() == setting.LeftKey)
				{
					if (clicker.IsEnabled)
					{
						if (isLeftMode)
							StopClick();
						else
							isLeftMode = true;
					}
					else
					{
						isLeftMode = true;
						StartClick();
					}
				}
				else if (e.Key.ToString() == setting.RightKey)
				{
					if (clicker.IsEnabled)
					{
						if (!isLeftMode)
							StopClick();
						else
							isLeftMode = false;
					}
					else
					{
						isLeftMode = false;
						StartClick();
					}
				}
			}
			else
			{
				if (e.Key.ToString() == setting.LeftKey)
				{
					isLeftMode = true;
					StartClick();
				}
				else if (e.Key.ToString() == setting.RightKey)
				{
					isLeftMode = false;
					StartClick();
				}
			}
		}

		private void Listener_KeyUp(object sender, RawKeyEventArgs args)
		{
			if (!setting.UseToggle)
				StopClick();
		}

		private void Application_Startup(object sender, StartupEventArgs e)
		{
			listener.KeyDown += Listener_KeyDown;
			listener.KeyUp += Listener_KeyUp;
		}

		private void Application_Exit(object sender, ExitEventArgs e)
		{
			listener.Dispose();
		}
	}
}
