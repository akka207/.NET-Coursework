﻿using System;
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
using Exam.Generators;
namespace Exam.AuthorizeControls
{
    /// <summary>
    /// Interaction logic for RegistrationControl.xaml
    /// </summary>
    public partial class AdminRegistrationControl : UserControl
    {
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


        public AdminRegistrationControl()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            OnSignUp?.Invoke(sender, e);
        }
        private void generateLoginButton_Click(object sender, RoutedEventArgs e)
        {
            login.textBox.Text = LoginGenerator.GenerateLogin(Fullname);
        }
        private void generatePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            password.textBox.Text = PasswordGenerator.GeneratePassword();
            passwordC.textBox.Text = password.textBox.Text;
        }
    }
}
