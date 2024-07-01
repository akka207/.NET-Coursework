﻿using Exam.CustomControls;
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
        #region Attached properties area
        public Staff CurrentStaff
        {
            get { return (Staff)GetValue(CurrentStaffProperty); }
            set { SetValue(CurrentStaffProperty, value); }
        }

        public static readonly DependencyProperty CurrentStaffProperty =
            DependencyProperty.Register("CurrentStaff", typeof(Staff), typeof(ScheduleControl), new UIPropertyMetadata(null));
        #endregion


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
        //private DateTime GetNearestFutureMonday(DateTime date)
        //{
        //    int daysToAdd = ((int)DayOfWeek.Monday - (int)date.DayOfWeek + 7) % 7;
        //    if (daysToAdd == 0)
        //    {
        //        daysToAdd = 7;
        //    }
        //    DateTime futureMonday = date.Date.AddDays(daysToAdd);
        //    return new DateTime(futureMonday.Year, futureMonday.Month, futureMonday.Day, 0, 0, 0);
        //}

        private Day GenerateDay(DateTime date)
        {
            return new Day() { Date = date, Events = DBController.GetEvents(CurrentStaff, ToStartOfDay(date), ToStartOfNextDay(date)) };
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
                month.Weeks.Add(GenerateWeek(date.AddDays(i*7)));
            }

            return month;
        }


        private void ChangeContainment(ScheduleView view)
        {
            switch(view)
            {
                case ScheduleView.Day:
                    DayView = GenerateDay(DateToView);
                    break;
                case ScheduleView.Week:
                    WeekView = GenerateWeek(DateToView);
                    break;
                case ScheduleView.Month:
                    MonthView = GenerateMonth(DateToView);
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
    }
}