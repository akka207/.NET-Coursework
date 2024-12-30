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
    public partial class ChangePasswordWindow : Window
    {
        private bool oldPasswordRequired;
        private Staff _staffToEdit = null;
        private ApplicationSettings _appSettings;
        private string _windowId = "ChangePasswordWindow";
        public ChangePasswordWindow(bool requireOldPassword)
        {
            InitializeComponent();
            _appSettings = SettingsManager.LoadSettings();
            oldPasswordRequired = requireOldPassword;
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

            _appSettings = SettingsManager.LoadSettings();
            ApplySettings();
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
        private async void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = OldPasswordBox.TextBoxText;
            string newPassword = NewPasswordBox.TextBoxText;
            if (await DBController.Instance.ChangePasswordAsync(_staffToEdit == null ? DBController.Instance.CurrentStaff.Person.Login : _staffToEdit.Person.Login, oldPassword, newPassword, oldPasswordRequired))
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
                textLabel.Text = $"Change for: {_staffToEdit.Person.Login}";
                OldPasswordBox.IsEnabled = false;
                OldPasswordBox.Visibility = Visibility.Hidden;
                textLabel.Visibility = Visibility.Visible;
            }
        }

        private void ControlBox_OnDrag(object sender, EventArgs e)
        {
            DragMove();
        }

        private void ControlBox_OnClose(object sender, EventArgs e)
        {
            Close();
        }

        private void ControlBox_OnMinimize(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
