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
using System.Windows.Shapes;
using Exam.Data;
using Exam.Generators;
using StaffManagerModels;
namespace Exam.Windows
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        private bool oldPasswordRequired = true;
        private Staff _staffToEdit = null;
        private ApplicationSettings _appSettings;
        private string _windowId = "ChangePasswordWindow";
        public ChangePasswordWindow()
        {
            InitializeComponent();
            _appSettings = SettingsManager.LoadSettings();
            ApplySettings();
        }
        private void ApplySettings()
        {
            if (_appSettings.Windows.TryGetValue(_windowId, out WindowSettings settings))
            {
                this.Width = settings.Width;
                this.Height = settings.Height;
                this.Top = settings.Top;
                this.Left = settings.Left;
                if (settings.IsMaximized)
                    this.WindowState = WindowState.Maximized;
            }
        }
        public ChangePasswordWindow(bool requireOldPassword, Staff staffToEdit)
        {
            InitializeComponent();

            oldPasswordRequired = requireOldPassword;
            _staffToEdit = staffToEdit;
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            var settings = new WindowSettings
            {
                Width = this.Width,
                Height = this.Height,
                Top = this.Top,
                Left = this.Left,
                IsMaximized = (this.WindowState == WindowState.Maximized)
            };
            _appSettings.Windows[_windowId] = settings;
            SettingsManager.SaveSettings(_appSettings);
        }
        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = OldPasswordBox.TextBoxText;
            string newPassword = NewPasswordBox.TextBoxText;
            if (DBController.ChangePassword(_staffToEdit == null ? DBController.CurrentStaff.Person.Login : _staffToEdit.Person.Login, oldPassword, newPassword, false))
            {
                MessageBox.Show("Password changed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Password change failed. Please check your credentials and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            NewPasswordBox.textBox.Text = PasswordGenerator.GeneratePassword();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!oldPasswordRequired)
            {
                OldPasswordBox.IsEnabled = false;
            }
        }
    }
}
