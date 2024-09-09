using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Exam.CustomControls
{
    internal class CustomBorder: Border
	{
		#region Attached properties area
		public bool Highlight
		{
			get { return (bool)GetValue(HighlightProperty); }
			set { SetValue(HighlightProperty, value); }
		}

		public static readonly DependencyProperty HighlightProperty =
			DependencyProperty.Register("Highlight", typeof(bool), typeof(CustomBorder), new UIPropertyMetadata(false));

		public bool ValidationState
		{
			get { return (bool)GetValue(ValidationStateProperty); }
			set { SetValue(ValidationStateProperty, value); }
		}

		public static readonly DependencyProperty ValidationStateProperty =
			DependencyProperty.Register("ValidationState", typeof(bool), typeof(CustomBorder), new UIPropertyMetadata(true));

		#endregion
	}
}
