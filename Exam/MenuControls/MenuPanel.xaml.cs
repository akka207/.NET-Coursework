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

		private int menuItemHeight = 0;
		public int MenuItemHeight { 
			get
			{
				return menuItemHeight;
			}
			set
			{
				if(value !=  menuItemHeight)
				{
					menuItemHeight = value;
					OnPropertyChanged(nameof(MenuItemHeight));
				}
			}
		}


		public MenuPanel()
		{
			InitializeComponent();
		}
		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			if (DBController.CurrentStaff.Role.Name != "Admin")
			{
				addBorder.Visibility = Visibility.Collapsed;
			}
			else
			{
				MenuItemHeight = 80;
			}
			highlightMenuItem(MenuType.Schedule);
		}

		private void highlightMenuItem(MenuType menu)
		{
			switch(menu)
			{
				case MenuType.Schedule:
					scheduleBorder.Highlight = true;
					personnelBorder.Highlight = false;
					addBorder.Highlight = false;
					profileBorder.Highlight = false;
					break;
				case MenuType.Personnel:
					scheduleBorder.Highlight = false;
					personnelBorder.Highlight = true;
					addBorder.Highlight = false;
					profileBorder.Highlight = false;
					break;
				case MenuType.UserAdd:
					scheduleBorder.Highlight = false;
					personnelBorder.Highlight = false;
					addBorder.Highlight = true;
					profileBorder.Highlight = false;
					break;
				case MenuType.Profile:
					scheduleBorder.Highlight = false;
					personnelBorder.Highlight = false;
					addBorder.Highlight = false;
					profileBorder.Highlight = true;
					break;
			}

		}

		private void sheduleMenuButton_OnClick(object sender, EventArgs e)
		{
			OnMenuSelected?.Invoke(this, MenuType.Schedule);
			highlightMenuItem(MenuType.Schedule);
		}
		private void personnelMenuButton_OnClick(object sender, EventArgs e)
		{
			OnMenuSelected?.Invoke(this, MenuType.Personnel);
			highlightMenuItem(MenuType.Personnel);
		}
		private void profileMenuButton_OnClick(object sender, EventArgs e)
		{
			OnMenuSelected?.Invoke(this, MenuType.Profile);
			highlightMenuItem(MenuType.Profile);
		}
		private void addMenuButton_OnClick(object sender, EventArgs e)
		{
			OnMenuSelected?.Invoke(this, MenuType.UserAdd);
			highlightMenuItem(MenuType.UserAdd);
		}
		private void control_Loaded(object sender, RoutedEventArgs e)
		{
			originBorder.Width = control.MinWidth;
		}
		private void logoutButton_OnClick(object sender, EventArgs e)
		{
			OnLogout?.Invoke(sender, e);
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
