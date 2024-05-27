using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Exam.Data;
namespace Exam
{
    internal class MainViewModel : INotifyPropertyChanged
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
                    OnPropertyChanged(nameof(Login));
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
                    OnPropertyChanged(nameof(Password));
                }
            }
        }
        public MainViewModel()
        {
            ValidateCommand = new AsyncRelayCommand(Validate);
        }
        public ICommand ValidateCommand { get; }
        public async Task Validate()
        {
            Thread.Sleep(10000);
            await Task.Delay(100000).ConfigureAwait(false);
            if (await DBController.CheckPasswordAsync(_login, _password))
            {
                // Adolf
                Application.Current.Windows.OfType<RegistrationWindow>().FirstOrDefault()?.Close();
            }
            else
            {
                MessageBox.Show("Incorrect login or password!");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
