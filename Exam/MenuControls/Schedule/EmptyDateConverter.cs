using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Exam.MenuControls
{
	class EmptyDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || (DateTime)value == DateTime.MinValue)
				return "none";

			DateTime dateTime = (DateTime)value;

			return $"{(dateTime.Day < 10 ? "0" : "") + dateTime.Day.ToString()}.{(dateTime.Month < 10 ? "0" : "") + dateTime.Month.ToString()}{(dateTime.Year != DateTime.Now.Year ? ("." + dateTime.Year) : "")}\n{(dateTime.Hour < 10 ? "0" + dateTime.Hour : dateTime.Hour)}:{(dateTime.Minute < 10 ? "0" + dateTime.Minute : dateTime.Minute)}";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
