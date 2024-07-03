using Exam.CustomControls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exam.MenuControls
{
    /// <summary>
    /// Interaction logic for ScheduleControl.xaml
    /// </summary>
    public partial class ScheduleControl : UserControl
    {
        public Staff CurrentStaff;

        public enum ScheduleView
        {
            Day,
            Week,
            Month
        }

        public Day DayView;
        public Week WeekView;
        public Month MonthView;

        public DateTime DateToView = DateTime.Now;

        public ScheduleControl()
        {
            InitializeComponent();
        }


        private DateTime ToStartOfDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }
        private DateTime ToStartOfNextDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 0, 0, 0);
        }
        private DateTime GetNearestPastMonday(DateTime date)
        {
            int daysToSubtract = (int)date.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysToSubtract < 0)
            {
                daysToSubtract += 7;
            }
            DateTime pastMonday = date.Date.AddDays(-daysToSubtract);
            return new DateTime(pastMonday.Year, pastMonday.Month, pastMonday.Day, 0, 0, 0);
        }

        private Day GenerateDay(DateTime date)
        {
            List<Event> events = CurrentStaff.Schedule.Events
                    .Where(e => e.EndDateTime >= ToStartOfDay(date) && e.StartDateTime <= ToStartOfNextDay(date))
                    .OrderBy(e => e.StartDateTime)
                    .ThenBy(e => e.EndDateTime)
                    .ToList();

            return new Day() { Date = date, Events = events };
        }
        private Week GenerateWeek(DateTime date)
        {
            Week week = new Week();

            for (int i = 0; i < 7; i++)
            {
                week.Days.Add(GenerateDay(GetNearestPastMonday(date).AddDays(i)));
            }

            return week;
        }
        private Month GenerateMonth(DateTime date)
        {
            Month month = new Month();

            for (int i = 0; i < 4; i++)
            {
                month.Weeks.Add(GenerateWeek(date.AddDays(i * 7)));
            }

            return month;
        }


        private void ChangeContainment(ScheduleView view)
        {
            switch (view)
            {
                case ScheduleView.Day:
                    DayView = GenerateDay(DateToView);
                    daySchedule.Visibility = Visibility.Visible;
                    weekSchedule.Visibility = Visibility.Hidden;
                    monthSchedule.Visibility = Visibility.Hidden;
                    daySchedule.ItemsSource = DayView.Events;
                    break;
                case ScheduleView.Week:
                    WeekView = GenerateWeek(DateToView);
                    daySchedule.Visibility = Visibility.Hidden;
                    weekSchedule.Visibility = Visibility.Visible;
                    monthSchedule.Visibility = Visibility.Hidden;
                    weekSchedule.ItemsSource = WeekView.Days;
                    break;
                case ScheduleView.Month:
                    MonthView = GenerateMonth(DateToView);
                    daySchedule.Visibility = Visibility.Hidden;
                    weekSchedule.Visibility = Visibility.Hidden;
                    monthSchedule.Visibility = Visibility.Visible;
                    monthSchedule.ItemsSource = MonthView.Weeks;
                    break;
            }
        }

        private void dayButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeContainment(ScheduleView.Day);
        }
        private void weekButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeContainment(ScheduleView.Week);
        }
        private void monthButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeContainment(ScheduleView.Month);
        }

        private void control_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeContainment(ScheduleView.Day);
        }
    }
}
