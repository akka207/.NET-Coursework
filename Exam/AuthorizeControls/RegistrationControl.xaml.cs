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
    /// Interaction logic for RegistrationControl.xaml
    /// </summary>
    public partial class RegistrationControl : UserControl
    {
        public event EventHandler OnLogIn;
        public event EventHandler OnSignUp;
        public string Login
        {
            get
            {
                return login.textBox.Text;
            }
        }
        public string Fullname
        {
            get
            {
                return fullname.textBox.Text;
            }
        }
        public string Password
        {
            get
            {
                return password.textBox.Text;
            }
        }
        public string PasswordC
        {
            get
            {
                return passwordC.textBox.Text;
            }
        }
        public string Phone
        {
            get
            {
                return phone.textBox.Text;
            }
        }
        public string Email
        {
            get
            {
                return email.textBox.Text;
            }
        }


        public RegistrationControl()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            OnSignUp?.Invoke(sender, e);
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            OnLogIn?.Invoke(sender, e);
        }
    }
}
