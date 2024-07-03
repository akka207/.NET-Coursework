using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Exam.MenuControls
{
    class DateTimeRestyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                DateTime dateTime = (DateTime)value;
                return $"{(dateTime.Day < 10 ? "0" : "") + dateTime.Day.ToString()}.{(dateTime.Month < 10 ? "0" : "") + dateTime.Month.ToString()}.{dateTime.Year} - {(dateTime.Hour < 10 ? "0" : "") + dateTime.Hour.ToString()}:{(dateTime.Minute < 10 ? "0" : "") + dateTime.Minute.ToString()}";
            }
            else
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
