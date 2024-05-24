using System.ComponentModel;
using System.Windows.Input;
using System.Windows;
namespace Exam
{
    public class RegistrationViewModel : INotifyPropertyChanged
    {
        private string _login;
        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
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

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged("Email");
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
            MessageBox.Show("Hello");
            if (!string.IsNullOrWhiteSpace(Login) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrWhiteSpace(Email))
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