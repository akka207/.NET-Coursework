﻿using Exam.Data;
using Exam.Windows;
using StaffManagerModels;
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

namespace Exam.AuthorizeControls
{
    /// <summary>
    /// Interaction logic for AdminEditControl.xaml
    /// </summary>
    public partial class AdminEditControl : UserControl, INotifyPropertyChanged
    {
        public Staff cStaff;
        public Staff CStaff
        {
            get { return cStaff; }
            set
            {
                if (cStaff != value)
                {
                    cStaff = value;
                    OnPropertyChanged(nameof(CStaff));
                }
            }
        }
        public AdminEditControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void PhoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (phoneTextBox.IsReadOnly)
            {
                phoneTextBox.IsReadOnly = false;
                phoneButton.Content = "Save";
            }
            else
            {
                phoneTextBox.IsReadOnly = true;
                phoneButton.Content = "Change";
                CStaff.Person.Phone = phoneTextBox.Text;
                DBController.EditPersonInfo(CStaff.Person);
            }
        }

        private void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            if (emailTextBox.IsReadOnly)
            {
                emailTextBox.IsReadOnly = false;
                emailButton.Content = "Save";
            }
            else
            {
                emailTextBox.IsReadOnly = true;
                emailButton.Content = "Change";
                CStaff.Person.Email = emailTextBox.Text;
                DBController.EditPersonInfo(CStaff.Person);
            }
        }
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow();
            changePasswordWindow.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}