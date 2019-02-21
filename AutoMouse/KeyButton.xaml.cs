using System;
using System.Collections.Generic;
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

namespace AutoMouse
{
	/// <summary>
	/// UserControl1.xaml에 대한 상호 작용 논리
	/// </summary>
	public partial class KeyButton : Button
	{
		public static DependencyProperty CurKeyProperty 
			= DependencyProperty.Register("CurKey", typeof(Key), typeof(KeyButton), new PropertyMetadata(Key.None, OnKeyChanged));

		private bool is_editing = false;
		public Key CurKey
		{
			get
			{
				if (is_editing)
					return Key.None;
				return (Key)GetValue(CurKeyProperty);
			}
			set
			{
				SetValue(CurKeyProperty, value);
			}
		}

		public event EventHandler<Key> KeyChanged;

		public KeyButton()
		{
			InitializeComponent();
		}

		protected override void OnClick()
		{
			base.OnClick();
			Content = "...";
			is_editing = true;
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			if (is_editing)
			{
				CurKey = e.Key;
				is_editing = false;
				KeyChanged?.Invoke(this, e.Key);
			}
		}

		private static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			((KeyButton)d).Content = e.NewValue;
		}
	}
}
