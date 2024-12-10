using Exam.Data;
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
using System.Windows.Shapes;

namespace Exam.MenuControls.PersonnelControls
{
    /// <summary>
    /// Interaction logic for AddEventWindow.xaml
    /// </summary>
    public partial class AddEventWindow : Window
    {
        public Staff SelectedStaff { get; set; }

        public AddEventWindow()
        {
            InitializeComponent();
        }
        private async void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDateTime;
            DateTime? endDateTime = null;


            if (!DateTime.TryParse(StartDatePicker.Text + " " + StartTimeTextBox.TextBoxText, out startDateTime))
            {
                MessageBox.Show("Invalid Start Date Time");
                return;
            }

            if (HasEndDateTimeCheckBox.IsChecked == true)
            {
                DateTime endDateTimeTemp;
                if (DateTime.TryParse(EndDatePicker.Text + " " + EndTimeTextBox.TextBoxText, out endDateTimeTemp))
                {
                    endDateTime = endDateTimeTemp;
                }
                else
                {
                    MessageBox.Show("Invalid End Date Time");
                    return;
                }
            }

            var newEvent = new Event
            {
                Name = NameTextBox.TextBoxText,
                Description = DescriptionTextBox.TextBoxText,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime
            };

            await DBController.Instance.AddEventAsync(SelectedStaff, newEvent);

            MessageBox.Show("Event added successfully");
        }

        private void HasEndDateTimeCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.Visibility = Visibility.Visible;
            EndDatePickerLabel.Visibility = Visibility.Visible;
            EndTimeTextBox.Visibility = Visibility.Visible;
            EndTimeTextBoxLabel.Visibility = Visibility.Visible;
            window.Height = 440;
        }

        private void HasEndDateTimeCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            EndDatePicker.Visibility = Visibility.Collapsed;
            EndDatePickerLabel.Visibility = Visibility.Collapsed;
            EndTimeTextBox.Visibility = Visibility.Collapsed;
            EndTimeTextBoxLabel.Visibility = Visibility.Collapsed;
            window.Height = 340;
        }

        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.textBox.Clear();
            DescriptionTextBox.textBox.Clear();
            StartDatePicker.SelectedDate = null;
            StartTimeTextBox.textBox.Clear();
            EndDatePicker.SelectedDate = null;
            EndTimeTextBox.textBox.Clear();
            HasEndDateTimeCheckBox.IsChecked = false;
        }

        private void ControlBox_OnClose(object sender, EventArgs e)
        {
            Close();
        }

        private void ControlBox_OnDrag(object sender, EventArgs e)
        {
            DragMove();
        }

        private void ControlBox_OnMinimize(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
