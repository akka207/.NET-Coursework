using Exam.Data;
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

namespace Exam.MenuControls
{
    /// <summary>
    /// Interaction logic for PersonnelControl.xaml
    /// </summary>
    public partial class PersonnelControl : UserControl, INotifyPropertyChanged
    {
        public Staff CurrentStaff;

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
    }
}
