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

namespace Exam.CustomControls
{
    /// <summary>
    /// Interaction logic for ControlBox.xaml
    /// </summary>
    public partial class ControlBox : UserControl
    {
        public event EventHandler OnDrag;
        public event EventHandler OnMinimize;
        public event EventHandler OnClose;

        public ControlBox()
        {
            InitializeComponent();
        }

        private void DragPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnDrag?.Invoke(this, e);
        }

        private void minimize_OnClick(object sender, EventArgs e)
        {
            OnMinimize?.Invoke(this, e);
        }

        private void close_OnClick(object sender, EventArgs e)
        {
            OnClose?.Invoke(this, e);
        }
    }
}
