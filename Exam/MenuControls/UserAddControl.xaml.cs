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
using Exam.AuthorizeControls;
using Exam.Data;
using Exam.Generators;
using StaffManagerModels;
namespace Exam.MenuControls
{
    /// <summary>
    /// Interaction logic for UserAddControl.xaml
    /// </summary>
    public partial class UserAddControl : UserControl
    {
        public UserAddControl()
        {
            InitializeComponent();
        }

        private async void regControl_OnSignUp(object sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            regControl.IsEnabled = false;

            string _login = regControl.Login,
                   _fullname = regControl.Fullname,
                   _password = regControl.Password,
                   _passwordC = regControl.PasswordC,
                   _phone = regControl.Phone,
                   _email = regControl.Email;

            var person = new Person() { Login = _login, FullName = _fullname, Phone = _phone, Email = _email };

            Task.Run(async () =>
            {
                var passwordValidatorMessages = DataValidators.ValidatePassword(_password, _passwordC);
                var personValidatorMessages = DataValidators.ValidatePerson(person);

                if (!passwordValidatorMessages.IsAllOK)
                {
                    Dispatcher.Invoke(() =>
                    {
                        regControl.password.SetValidationErrors(passwordValidatorMessages.Data[DataValidators.Fields.Password]);
                        regControl.passwordC.SetValidationErrors(passwordValidatorMessages.Data[DataValidators.Fields.CPassword]);
                    });
                }

                if (!personValidatorMessages.IsAllOK)
                {
                    Dispatcher.Invoke(() =>
                    {
                        regControl.fullname.SetValidationErrors(personValidatorMessages.Data[DataValidators.Fields.FullName]);
                        regControl.login.SetValidationErrors(personValidatorMessages.Data[DataValidators.Fields.Login]);
                        regControl.phone.SetValidationErrors(personValidatorMessages.Data[DataValidators.Fields.Phone]);
                        regControl.email.SetValidationErrors(personValidatorMessages.Data[DataValidators.Fields.Email]);
                    });
                }

                if (passwordValidatorMessages.IsAllOK && personValidatorMessages.IsAllOK)
                {
                    //Logger.Instance.INFO($"Registration data valid, proceeding with registration for login: {_login}");
                    await DBController.Instance.RegisterPersonAsync(person, _password);
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //Logger.Instance.ERROR("Registration failed due to invalid data");
                    MessageBox.Show("Data didn't pass validation (see tips for each field)", "Invalid Data", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                    regControl.IsEnabled = true;
                });
            });
        }
    }
}
