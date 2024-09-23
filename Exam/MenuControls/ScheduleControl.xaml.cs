using Exam.CustomControls;
using Exam.Data;
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

namespace Exam.MenuControls
{
    /// <summary>
    /// Interaction logic for ScheduleControl.xaml
    /// </summary>
    public partial class ScheduleControl : UserControl, INotifyPropertyChanged
    {
        private Event selectedEvent = null;
        public Event SelectedEvent
        {
            get
            {
                return selectedEvent;
            }
            set
            {
                if (value != selectedEvent)
                {
                    selectedEvent = value;
                    OnPropertyChanged(nameof(SelectedEvent));
                }
            }
        }

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

            return ToStartOfDay(dateTime).AddDays(1); ;
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
            List<Event> events = DBController.CurrentStaff.Schedule.Events
                    .Where(e => (e.EndDateTime != null ? e.EndDateTime : e.StartDateTime) >= ToStartOfDay(date)
                                && e.StartDateTime < ToStartOfNextDay(date))
                    .OrderBy(e => e.StartDateTime)
                    .ThenBy(e => e.EndDateTime)
                    .ToList();

            return new Day() { Date = date, Events = events, IsSelected = date.Date == DateTime.Today };
        }
        private Week GenerateWeek(DateTime date)
        {
            Week week = new Week();
            week.EventsCount = 0;

            for (int i = 0; i < 7; i++)
            {
                Day day = GenerateDay(GetNearestPastMonday(date).AddDays(i));
                week.Days.Add(day);
                week.EventsCount += day.Events.Count;
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


        private void ChangeContainment(ScheduleView view, bool doGenerate = true)
        {
            switch (view)
            {
                case ScheduleView.Day:
                    DateToView = DateTime.Now;
                    if (doGenerate)
                    {
                        DayView = GenerateDay(DateToView);
                        daySchedule.ItemsSource = DayView.Events;
                    }
                    daySchedule.Visibility = Visibility.Visible;
                    weekSchedule.Visibility = Visibility.Hidden;
                    monthSchedule.Visibility = Visibility.Hidden;
                    arrowUpButton.Visibility = Visibility.Hidden;
                    arrowDownButton.Visibility = Visibility.Hidden;
                    break;
                case ScheduleView.Week:
                    if (doGenerate)
                    {
                        WeekView = GenerateWeek(DateToView);
                        weekSchedule.ItemsSource = WeekView.Days;
                    }
                    daySchedule.Visibility = Visibility.Hidden;
                    weekSchedule.Visibility = Visibility.Visible;
                    monthSchedule.Visibility = Visibility.Hidden;
                    arrowUpButton.Visibility = Visibility.Visible;
                    arrowDownButton.Visibility = Visibility.Visible;
                    break;
                case ScheduleView.Month:
                    if (doGenerate)
                    {
                        MonthView = GenerateMonth(DateToView);
                        monthSchedule.ItemsSource = MonthView.Weeks;
                    }
                    daySchedule.Visibility = Visibility.Hidden;
                    weekSchedule.Visibility = Visibility.Hidden;
                    monthSchedule.Visibility = Visibility.Visible;
                    arrowUpButton.Visibility = Visibility.Visible;
                    arrowDownButton.Visibility = Visibility.Visible;
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
            SelectedEvent = DBController.CurrentStaff.Schedule.Events
                .Where(e => e.StartDateTime > DateTime.Now)
                .OrderBy(e => e.StartDateTime)
                .FirstOrDefault();
        }

        private void dayofWeek_CustomBorder_Loaded(object sender, RoutedEventArgs e)
        {
            var border = sender as CustomBorder;
            border.Highlight = (border.DataContext as Day).IsSelected;
        }

        private void dayofWeek_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SelectedEvent = (sender as CustomBorder).DataContext as Event;
        }








        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void dayofWeekofMonth_Loaded(object sender, RoutedEventArgs e)
        {
            var border = sender as CustomBorder;
            border.Highlight = (border.DataContext as Day).IsSelected;
        }

        private void arrowUpButton_OnClick(object sender, EventArgs e)
		{
			DateToView = DateToView.AddDays(7);
			MonthView = GenerateMonth(DateToView);
			WeekView = MonthView.Weeks.First();
			weekSchedule.ItemsSource = WeekView.Days;
			monthSchedule.ItemsSource = MonthView.Weeks;
		}

        private void arrowDownButton_OnClick(object sender, EventArgs e)
        {
			DateToView = DateToView.AddDays(-7);
			MonthView = GenerateMonth(DateToView);
			WeekView = MonthView.Weeks.First();
			weekSchedule.ItemsSource = WeekView.Days;
			monthSchedule.ItemsSource = MonthView.Weeks;
        }

        private void weekOfMonth_MouseUp(object sender, MouseButtonEventArgs e)
        {
            WeekView = (sender as CustomBorder).DataContext as Week;
            weekSchedule.ItemsSource = WeekView.Days;
            ChangeContainment(ScheduleView.Week, false);
        }
    }
}
