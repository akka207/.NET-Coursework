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

        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        public ChangePasswordWindow(bool requireOldPassword, Staff staffToEdit)
        {
            InitializeComponent();

            oldPasswordRequired = requireOldPassword;
            _staffToEdit = staffToEdit;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = OldPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!oldPasswordRequired)
            {
                OldPasswordBox.IsEnabled = false;
            }
        }
    }
}
