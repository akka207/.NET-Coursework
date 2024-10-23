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
        private ApplicationSettings _appSettings;
        private string _windowId = "AdminEditWindow";
        public AdminEditWindow()
        {
            InitializeComponent();
            _appSettings = SettingsManager.LoadSettings();
            ApplySettings();
        }
        private void ApplySettings()
        {
            if (_appSettings.Windows.TryGetValue(_windowId, out WindowSettings settings))
            {
                this.Width = settings.Width;
                this.Height = settings.Height;
                this.Top = settings.Top;
                this.Left = settings.Left;
                if (settings.IsMaximized)
                    this.WindowState = WindowState.Maximized;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            var settings = new WindowSettings
            {
                Width = this.Width,
                Height = this.Height,
                Top = this.Top,
                Left = this.Left,
                IsMaximized = (this.WindowState == WindowState.Maximized)
            };
            _appSettings.Windows[_windowId] = settings;
            SettingsManager.SaveSettings(_appSettings);
        }
        public void SetStaff()
        {
            adminEditControl.CStaff = xD;
        }
    }
}
