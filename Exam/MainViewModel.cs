using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
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

        public Dispatcher Dispatcher;

        public MainViewModel(Dispatcher dispatcher)
        {
            ValidateCommand = new RelayCommand(Validate);
            Dispatcher = dispatcher;
        }


        public ICommand ValidateCommand { get; }
        public void Validate()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            Task.Run(async () =>
            {
                Thread.Sleep(5000);
                if (await DBController.CheckPasswordAsync(_login, _password) == true)
                {
                    //Application.Current.Windows.OfType<RegistrationWindow>().FirstOrDefault()?.Close();
                    MessageBox.Show("Succesfull!");
                }
                else
                {
                    MessageBox.Show("Incorrect login or password!");
                }
                Dispatcher.Invoke(() => { Mouse.OverrideCursor = null; });
            });
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
