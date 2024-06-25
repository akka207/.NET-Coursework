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

namespace Exam.AuthorizeControls
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
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
        public string Password
        {
            get
            {
                return password.textBox.Text;
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
    }
}