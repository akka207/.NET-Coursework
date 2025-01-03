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
        public int selectedIndex = -1;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    OnPropertyChanged(nameof(SelectedIndex));
                }
            }
        }

        public List<Role> Roles { get; set; } = new List<Role>() { Role.User, Role.Manager, Role.Admin };


        public AdminEditControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void PhoneButton_Click(object sender, RoutedEventArgs e)
        {
            if (phoneTextBox.IsReadOnly)
            {
                phoneTextBox.IsReadOnly = false;
                phoneButton.Content = "Save";
                phoneTextBox.Focusable = true;
                Keyboard.Focus(phoneTextBox);
            }
            else
            {
                phoneTextBox.IsReadOnly = true;
                phoneButton.Content = "Change";
                phoneTextBox.Focusable = false;
                CStaff.Person.Phone = phoneTextBox.Text;

                if (CStaff.Person.Phone != phoneTextBox.Text)
                {
                    CStaff.Person.Phone = phoneTextBox.Text;
                    await DBController.Instance.EditPersonInfoAsync(CStaff.Person);
                }
            }
        }

        private async void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            if (emailTextBox.IsReadOnly)
            {
                emailTextBox.IsReadOnly = false;
                emailButton.Content = "Save";
                emailTextBox.Focusable = true;
                Keyboard.Focus(emailTextBox);
            }
            else
            {
                emailTextBox.IsReadOnly = true;
                emailButton.Content = "Change";
                emailTextBox.Focusable = false;
                CStaff.Person.Email = emailTextBox.Text;

                if (CStaff.Person.Email != emailTextBox.Text)
                {
                    CStaff.Person.Email = emailTextBox.Text;
                    await DBController.Instance.EditPersonInfoAsync(CStaff.Person);
                }
            }
        }
        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow = new ChangePasswordWindow(false, CStaff);
            changePasswordWindow.ShowDialog();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            switch (CStaff.Role.Name)
            {
                case "User":
                    SelectedIndex = 0;
                    break;
                case "Manager":
                    SelectedIndex = 1;
                    break;
                case "Admin":
                    SelectedIndex = 2;
                    break;
            }
        }

        private async void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(SelectedIndex != -1)
                await DBController.Instance.ChangeRoleAsync(CStaff, (sender as ComboBox).SelectedItem as Role);
        }
    }
}
