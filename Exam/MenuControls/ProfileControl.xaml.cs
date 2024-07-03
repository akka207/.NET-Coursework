using StaffManagerModels;
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

namespace Exam.MenuControls
{
    /// <summary>
    /// Interaction logic for ProfileControl.xaml
    /// </summary>
    public partial class ProfileControl : UserControl
    {
        public Staff CurrentStaff { get; set; }
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
                CurrentStaff.Person.Phone = phoneTextBox.Text;
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
                CurrentStaff.Person.Email = emailTextBox.Text;
            }
        }
    }

}

