using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AutoMouse
{
	/// <summary>
	/// MainWindow.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class MainWindow : Window
	{
		private Properties.Settings setting = Properties.Settings.Default;
		private System.Windows.Forms.NotifyIcon ni;

		public MainWindow()
		{
			InitializeComponent();

			ni = new System.Windows.Forms.NotifyIcon();
			ni.Text = "오토 마우스";
			ni.MouseDown += Ni_MouseDown;
			ni.DoubleClick += Ni_DoubleClick;

			using (Stream s = Application.GetResourceStream(new Uri("Resources/Icon.ico", UriKind.Relative)).Stream)
				ni.Icon = new System.Drawing.Icon(s);
			ni.Visible = true;

			LeftKeyButton.CurKey = (Key) Enum.Parse(typeof(Key), setting.LeftKey);
			RightKeyButton.CurKey = (Key) Enum.Parse(typeof(Key), setting.RightKey);
			ToggleCheck.IsChecked = setting.UseToggle;
			CountSlider.Value = setting.ClickSpeed;
			InfoCheck.IsChecked = setting.UseAlert;

			Hide();
		}

		private void Window_Loaded(object sender, EventArgs e)
		{
			((App)Application.Current).SetupMainWindow(this);
		}

		protected override void OnStateChanged(EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
				Hide();

			base.OnStateChanged(e);
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			Hide();
			e.Cancel = true;
			base.OnClosing(e);
		}

		private void Ni_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				ContextMenu menu = (ContextMenu)FindResource("NiCtxMenu");
				menu.IsOpen = true;
			}
		}

		private void Ni_DoubleClick(object sender, EventArgs e)
		{
			Show();
			WindowState = WindowState.Normal;
		}

		public void EnableControls()
		{
			CountSlider.IsEnabled = true;
			LeftKeyButton.IsEnabled = true;
			RightKeyButton.IsEnabled = true;
			ToggleCheck.IsEnabled = true;
		}

		public void DisableControls()
		{
			CountSlider.IsEnabled = false;
			LeftKeyButton.IsEnabled = false;
			RightKeyButton.IsEnabled = false;
			ToggleCheck.IsEnabled = false;
		}

		private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			MeterLabel.Content = "초당 " + e.NewValue + "번";
		}

		private void Test_Click(object sender, RoutedEventArgs e)
		{
			string text = (string)ClickCountLabel.Content;
			int count = int.Parse(text.Substring(0, text.Length - 1));
			ClickCountLabel.Content = (count + 1) + "회";
		}

		private void TestCountClear_Click(object sender, RoutedEventArgs e)
		{
			ClickCountLabel.Content = "0회";
		}

		private void Left_KeyChanged(object sender, Key e)
		{
			setting.LeftKey = e.ToString();
			setting.Save();
		}

		private void Right_KeyChanged(object sender, Key e)
		{
			setting.RightKey = e.ToString();
			setting.Save();
		}

		private void CheckChanged(object sender, RoutedEventArgs e)
		{
			setting.UseToggle = ToggleCheck.IsChecked == true;
			setting.Save();
		}

		private void CountSlider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
		{
			setting.ClickSpeed = (int)CountSlider.Value;
			setting.Save();
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			ni.Dispose();
			Application.Current.Shutdown();
		}

		private void InfoCheck_Click(object sender, RoutedEventArgs e)
		{
			setting.UseAlert = InfoCheck.IsChecked == true;
			setting.Save();
		}
	}
}
