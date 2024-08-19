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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exam.CustomControls
{
    /// <summary>
    /// Interaction logic for CustomTextBox.xaml
    /// </summary>
    public partial class CustomTextBox : UserControl, INotifyPropertyChanged
    {
        #region Attached properties area
        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(CustomTextBox), new UIPropertyMetadata("placeholder"));
        #endregion

        public event TextChangedEventHandler TextChanged;
        private string textBoxText = string.Empty;
        public string TextBoxText
        {
            get { return textBoxText; }
            set
            {
                if (textBoxText != value)
                {
                    textBoxText = value;
                    OnPropertyChanged(nameof(TextBoxText));
                }
            }
        }


        public CustomTextBox()
        {
            InitializeComponent();
            border.Highlight = false;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxText = textBox.Text;
            HandlePlaceholder();
            TextChanged?.Invoke(this, e);
        }
        private void textBox_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            border.Highlight = textBox.IsKeyboardFocused;
            HandlePlaceholder();
        }

        private void HandlePlaceholder()
        {
            if (textBox.IsKeyboardFocused || textBox.Text != string.Empty)
            {
                placeholder.Visibility = Visibility.Hidden;
            }
            else
            {
                placeholder.Visibility = Visibility.Visible;
     
            }
        }
    }
}
