using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Exam.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomPasswordBox.xaml
    /// </summary>
    public partial class CustomPasswordBox : UserControl
    {
        #region Attached properties area
        public string PassPlaceholder
        {
            get { return (string)GetValue(PassPlaceholderProperty); }
            set { SetValue(PassPlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PassPlaceholderProperty =
            DependencyProperty.Register("PassPlaceholder", typeof(string), typeof(CustomPasswordBox), new UIPropertyMetadata("passplaceholder"));
        #endregion
        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public CustomPasswordBox()
        {
            InitializeComponent();
            border.Highlight = false;
            DataContext = this;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void OnPasswordChanged()
        {
            passwordBox.Password = Password;
        }
        private void passwordBox_TextChanged(object sender, RoutedEventArgs e)
        {
            Password = passwordBox.Password;
            HandlePlaceholder();
        }
        private void passwordBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            border.Highlight = passwordBox.IsKeyboardFocused;
            HandlePlaceholder();
        }
        private void HandlePlaceholder()
        {
            if (passwordBox.IsKeyboardFocused || passwordBox.Password != string.Empty)
                placeholder.Visibility = Visibility.Hidden;
            else
                placeholder.Visibility = Visibility.Visible;
        }
    }
}
