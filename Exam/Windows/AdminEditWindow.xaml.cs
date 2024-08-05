using Exam.Data;
using StaffManagerModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Exam.Windows
{
    /// <summary>
    /// Interaction logic for AdminEditWindow.xaml
    /// </summary>
    public partial class AdminEditWindow : Window
    {
        public Staff xD;
        public AdminEditWindow()
        {
            InitializeComponent();

        }
        public void SetStaff()
        {
            adminEditControl.CStaff = xD;
        }
    }
}
