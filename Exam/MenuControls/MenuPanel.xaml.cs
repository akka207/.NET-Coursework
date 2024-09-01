using Exam.CustomControls;
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
using StaffManagerModels;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Exam.Data;
namespace Exam.MenuControls
{
	/// <summary>
	/// Interaction logic for MenuPanel.xaml
	/// </summary>
	public partial class MenuPanel : UserControl, INotifyPropertyChanged
	{
		public enum MenuType
		{
			Schedule,
			Personnel,
			Profile,
			UserAdd
		}
		public event EventHandler<MenuType> OnMenuSelected;
		public event EventHandler OnLogout;

		public int MenuHeight { get; set; } = 250;
		public int MenuItemHeight { get; set; } = 0;

		public MenuPanel()
		{
			InitializeComponent();
			if (DBController.CurrentStaff.Role.Name == "Admin")
			{
				MenuHeight = 330;
				MenuItemHeight = 80;
			}
		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (DBController.CurrentStaff.Role.Name != "Admin")
			{
				AddUser.Visibility = Visibility.Collapsed;
			}
		}
		private void sheduleMenuButton_OnClick(object sender, EventArgs e)
		{
			OnMenuSelected?.Invoke(this, MenuType.Schedule);
		}

		private void personnelMenuButton_OnClick(object sender, EventArgs e)
		{
			OnMenuSelected?.Invoke(this, MenuType.Personnel);
		}

		private void profileMenuButton_OnClick(object sender, EventArgs e)
		{
			OnMenuSelected?.Invoke(this, MenuType.Profile);
		}
		private void addMenuButton_OnClick(object sender, EventArgs e)
		{
			OnMenuSelected?.Invoke(this, MenuType.UserAdd);
		}
		private void control_Loaded(object sender, RoutedEventArgs e)
		{
			originBorder.Width = control.MinWidth;
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private void logoutButton_OnClick(object sender, EventArgs e)
		{
			OnLogout?.Invoke(sender, e);
		}
	}
}
