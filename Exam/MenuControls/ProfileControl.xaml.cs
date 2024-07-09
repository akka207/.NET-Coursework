using StaffManagerModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Exam.Data;
using Exam.Windows;
namespace Exam.MenuControls
{
    /// <summary>
    /// Interaction logic for ProfileControl.xaml
    /// </summary>
    public partial class ProfileControl : UserControl, INotifyPropertyChanged
    {
        public Staff _currentStaff;
        public Staff CurrentStaff
        {
            get { return _currentStaff; }
            set
            {
                if (_currentStaff != value)
                {
                    _currentStaff = value;
                    OnPropertyChanged(nameof(CurrentStaff));
                }
            }
        }
        public ProfileControl()
        {
            InitializeComponent();
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
                DBController.CurrentStaff.Person.Phone = phoneTextBox.Text;
                DBController.EditPersonInfo(DBController.CurrentStaff.Person);
                // а‘узу би-ллахи мин аш-шайтан ар-раджим
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
                DBController.CurrentStaff.Person.Email = emailTextBox.Text;
                DBController.EditPersonInfo(DBController.CurrentStaff.Person);
                // а‘узу би-ллахи мин аш-шайтан ар-раджим
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

        private void control_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentStaff = DBController.CurrentStaff;
        }
    }

}

