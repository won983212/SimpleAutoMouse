using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoMouse.Controls
{
    public partial class KeyButton : Button
    {
        public static DependencyProperty SelectedKeyProperty
            = DependencyProperty.Register("SelectedKey", typeof(Key), typeof(KeyButton), new PropertyMetadata(Key.None, OnKeyChanged));

        private bool editing = false;


        public KeyButton()
        {
            InitializeComponent();
        }


        public Key SelectedKey
        {
            get
            {
                if (editing)
                    return Key.None;
                return (Key)GetValue(SelectedKeyProperty);
            }
            set
            {
                SetValue(SelectedKeyProperty, value);
            }
        }

        private void UpdateButtonText()
        {
            if (editing)
                Content = "...";
            else
                Content = SelectedKey.ToString();
        }

        protected override void OnClick()
        {
            base.OnClick();
            editing = true;
            UpdateButtonText();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (editing)
            {
                SelectedKey = e.Key;
                editing = false;
                UpdateButtonText();
                e.Handled = true;
                return;
            }
            base.OnKeyDown(e);
        }

        private static void OnKeyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((KeyButton)d).UpdateButtonText();
        }
    }
}
