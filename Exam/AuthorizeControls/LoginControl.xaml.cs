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

namespace Exam.AuthorizeControls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public event EventHandler OnLogIn;
        public event EventHandler OnSignUp;
        public event EventHandler OnDebugLogIn;

        public string Login
        {
            get
            {
                return login.textBox.Text;
            }
        }

        public string Password
        {
            get
            {
                return showPasswordCheckBox.IsChecked == true ? passwordTextBox.textBox.Text : passwordBox.Password;
            }
        }

        public LoginControl()
        {
            InitializeComponent();
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            OnLogIn?.Invoke(sender, e);
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            OnSignUp?.Invoke(sender, e);
        }

        private void showPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            passwordTextBox.textBox.Text = passwordBox.Password;
            passwordTextBox.Visibility = Visibility.Visible;
            passwordBox.Visibility = Visibility.Collapsed;
        }

        private void showPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordBox.Password = passwordTextBox.textBox.Text;
            passwordTextBox.Visibility = Visibility.Collapsed;
            passwordBox.Visibility = Visibility.Visible;
        }

        private void debugButton_Click(object sender, RoutedEventArgs e)
        {
            OnDebugLogIn?.Invoke(sender, e);
        }
    }

}
