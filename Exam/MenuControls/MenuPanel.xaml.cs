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

namespace Exam.MenuControls
{
    /// <summary>
    /// Interaction logic for MenuPanel.xaml
    /// </summary>
    public partial class MenuPanel : UserControl
    {
        public enum MenuType
        {
            Schedule,
            Personnel,
            Profile
        }

        public event EventHandler<MenuType> OnMenuSelected;



        public MenuPanel()
        {
            InitializeComponent();
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

        private void forall_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(originBorder.IsMouseOver)
            {
                originBorder.Width = control.MaxWidth;
            }
            else
            {
                originBorder.Width = control.MinWidth;
            }
        }

        private void control_Loaded(object sender, RoutedEventArgs e)
        {
            originBorder.Width = control.MinWidth;
        }
    }
}
