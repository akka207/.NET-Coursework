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

namespace Exam
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void MenuPanel_OnMenuSelected(object sender, MenuControls.MenuPanel.MenuType e)
        {
            switch(e)
            {
                case MenuControls.MenuPanel.MenuType.Schedule:
                    schedule.Visibility = Visibility.Visible;
                    personnel.Visibility = Visibility.Hidden;
                    profile.Visibility = Visibility.Hidden;
                    add.Visibility = Visibility.Hidden;
                    break;
                case MenuControls.MenuPanel.MenuType.Personnel:
                    schedule.Visibility = Visibility.Hidden;
                    personnel.Visibility = Visibility.Visible;
                    profile.Visibility = Visibility.Hidden;
                    add.Visibility = Visibility.Hidden;
                    break;
                case MenuControls.MenuPanel.MenuType.Profile:
                    schedule.Visibility = Visibility.Hidden;
                    personnel.Visibility = Visibility.Hidden;
                    profile.Visibility = Visibility.Visible;
                    add.Visibility = Visibility.Hidden;
                    break;
                case MenuControls.MenuPanel.MenuType.UserAdd:
                    schedule.Visibility = Visibility.Hidden;
                    personnel.Visibility = Visibility.Hidden;
                    profile.Visibility = Visibility.Hidden;
                    add.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Title = $"{DBController.CurrentStaff.Person.Login} | {DBController.CurrentStaff.Role.Name}";
        }
    }
}
