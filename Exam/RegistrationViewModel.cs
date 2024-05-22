using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
namespace Exam
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged("Username");
                }
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public ICommand RegisterCommand { get; }

        public RegistrationViewModel()
        {
            RegisterCommand = new RelayCommand<object>(Register);
        }

        private void Register(object parameter)
        {
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Registered sucessfully");
            }
            else
            {
                MessageBox.Show("Incorrect input");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}