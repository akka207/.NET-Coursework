using Exam.AuthorizeControls;
using Exam.Data;
using Exam.MenuControls.PersonnelControls;
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
using Exam.Windows;
namespace Exam.MenuControls
{
	/// <summary>
	/// Interaction logic for PersonnelControl.xaml
	/// </summary>
	public partial class PersonnelControl : UserControl, INotifyPropertyChanged
	{
		private ObservableCollection<Staff> staffList = null;
		public ObservableCollection<Staff> StaffList
		{
			get
			{
				return staffList;
			}
			set
			{
				if (value != staffList)
				{
					staffList = value;
					OnPropertyChanged(nameof(StaffList));
				}
			}
		}

		public PersonnelControl()
		{
			InitializeComponent();
		}

		private void control_Loaded(object sender, RoutedEventArgs e)
		{
			StaffList = new ObservableCollection<Staff>(DBController.GetAllStaff());
		}




		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void staff_ListBoxItem_Loaded(object sender, RoutedEventArgs e)
		{
			Button addEventButton = null;
			Button editStaffButton = null;

			foreach (object element in ((sender as Border).Child as Grid).Children)
			{
				if(element is Button && (element as Button).Name == "addEventButton")
				{
					addEventButton = element as Button;
				}
				else if(element is Button && (element as Button).Name == "editStaffButton")
				{
					editStaffButton = element as Button;
				}
			}

			switch (DBController.CurrentStaff.Role.Name)
			{
				case "User":
					addEventButton.Visibility = Visibility.Hidden;
					editStaffButton.Visibility = Visibility.Hidden;
					break;
				case "Manager":
					addEventButton.Visibility = Visibility.Visible;
					editStaffButton.Visibility = Visibility.Hidden;
					break;
				case "Admin":
					addEventButton.Visibility = Visibility.Visible;
					editStaffButton.Visibility = Visibility.Visible;
					break;
			}
		}

		private void addEventButton_Click(object sender, RoutedEventArgs e)
		{
			AddEventWindow window = new AddEventWindow();
			window.SelectedStaff = (sender as Button).DataContext as Staff;
			window.ShowDialog();
			if (window.SelectedStaff.Id == DBController.CurrentStaff.Id)
			{
				DBController.UpdateCurrentStaff();
			}
		}

		private void editStaffButton_Click(object sender, RoutedEventArgs e)
		{
			AdminEditWindow window = new AdminEditWindow();
			window.xD = (sender as Button).DataContext as Staff;
			window.SetStaff();
			window.ShowDialog();
		}
	}
}
