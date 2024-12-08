using Exam.Data;
using StaffManagerModels;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfAnimatedGif;
using Exam.Services;

namespace Exam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class AuthorizeWindow : Window
    {

        public AuthorizeWindow()
        {
            InitializeComponent();
            LoadGifAnimation();
        }
        private void LoadGifAnimation()
        {
            var gifUri = new Uri("pack://application:,,,/GIFs/Icon.gif", UriKind.Absolute);
            var image = new BitmapImage(gifUri);
            ImageBehavior.SetAnimatedSource(gifImage, image);
            ImageBehavior.SetRepeatBehavior(gifImage, new System.Windows.Media.Animation.RepeatBehavior(1));
            ImageBehavior.SetAnimationSpeedRatio(gifImage, 1.5);
        }
        private void AvailableControls(bool value)
        {
            //Logger.Instance.DEBUG($"Setting AvailableControls to {(value ? "enabled" : "disabled")}");
            logInControl.IsEnabled = value;
        }

        private void logInControl_OnLogIn(object? sender, EventArgs e)
        {
            //Logger.Instance.INFO("LogIn event triggered");
            Mouse.OverrideCursor = Cursors.Wait;
            AvailableControls(false);

            string _login = logInControl.Login, _password = logInControl.Password;
            Task.Run(async () =>
            {
                //Logger.Instance.DEBUG($"Attempting to validate password for login: {_login}");
                if (await DBController.Instance.CheckPasswordAsync(_login, _password))
                {
                    //Logger.Instance.INFO($"Password validation successful for login: {_login}");
                    Staff? staff = await DBController.Instance.GetStaffAsync(_login);
                    if (staff == null)
                    {
                        //Logger.Instance.ERROR($"Failed to retrieve staff information for login: {_login}");
                        MessageBox.Show("An error occurred while retrieving staff information!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        //Logger.Instance.INFO($"Staff information retrieved for login: {_login}");
                        await DBController.Instance.SelectCurrentStaffAsync(_login);
                        Dispatcher.Invoke(() =>
                        {
                            //Logger.Instance.INFO("Opening Menu window and closing login window");
                            Menu menu = new Menu();
                            menu.Show();
                            Close();
                        });
                    }
                }
                else
                {
                    //Logger.Instance.WARN($"Password validation failed for login: {_login}");
                    MessageBox.Show("Incorrect login or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Dispatcher.Invoke(() =>
                {
                    //Logger.Instance.DEBUG("Restoring cursor and re-enabling controls after login attempt");
                    Mouse.OverrideCursor = null;
                    AvailableControls(true);
                });
            });
        }

        private void logInControl_OnSignUp(object sender, EventArgs e)
        {
            //Logger.Instance.INFO("SignUp event triggered, switching to registration control");
            logInControl.Visibility = Visibility.Hidden;
            registerControl.Visibility = Visibility.Visible;
        }

        private void registerControl_OnSignUp(object sender, EventArgs e)
        {
            //Logger.Instance.INFO("Registration attempt started");
            Mouse.OverrideCursor = Cursors.Wait;
            AvailableControls(false);

            string _login = registerControl.Login,
                   _fullname = registerControl.Fullname,
                   _password = registerControl.Password,
                   _passwordC = registerControl.PasswordC,
                   _phone = registerControl.Phone,
                   _email = registerControl.Email;

            var person = new Person() { Login = _login, FullName = _fullname, Phone = _phone, Email = _email };

            Task.Run(async () =>
            {
                //Logger.Instance.DEBUG("Starting data validation for registration fields");
                var passwordValidatorMessages = DataValidators.ValidatePassword(_password, _passwordC);
                var personValidatorMessages = DataValidators.ValidatePerson(person);

                if (!passwordValidatorMessages.IsAllOK)
                {
                    //Logger.Instance.WARN("Password validation failed");
                    Dispatcher.Invoke(() =>
                    {
                        registerControl.password.SetValidationErrors(passwordValidatorMessages.Data[DataValidators.Fields.Password]);
                        registerControl.passwordC.SetValidationErrors(passwordValidatorMessages.Data[DataValidators.Fields.CPassword]);
                    });
                }

                if (!personValidatorMessages.IsAllOK)
                {
                    //Logger.Instance.WARN("Person information validation failed");
                    Dispatcher.Invoke(() =>
                    {
                        registerControl.fullname.SetValidationErrors(personValidatorMessages.Data[DataValidators.Fields.FullName]);
                        registerControl.login.SetValidationErrors(personValidatorMessages.Data[DataValidators.Fields.Login]);
                        registerControl.phone.SetValidationErrors(personValidatorMessages.Data[DataValidators.Fields.Phone]);
                        registerControl.email.SetValidationErrors(personValidatorMessages.Data[DataValidators.Fields.Email]);
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
                    //Logger.Instance.DEBUG("Restoring cursor and re-enabling controls after registration attempt");
                    Mouse.OverrideCursor = null;
                    AvailableControls(true);
                });
            });
        }

        private void registerControl_OnLogIn(object sender, EventArgs e)
        {
            //Logger.Instance.INFO("Switching back to login control");
            logInControl.Visibility = Visibility.Visible;
            registerControl.Visibility = Visibility.Hidden;
        }

        private void logInControl_OnDebugLogIn(object sender, EventArgs e)
        {
            //Logger.Instance.INFO("Debug login event triggered");
            Mouse.OverrideCursor = Cursors.Wait;
            AvailableControls(false);

            string _login = "Akka 207", _password = "!Qwerty1";
            Task.Run(async () =>
            {
                //Logger.Instance.DEBUG($"Attempting debug login for {_login}");
                if (await DBController.Instance.CheckPasswordAsync(_login, _password))
                {
                    //Logger.Instance.INFO("Debug login successful, retrieving staff information");
                    Staff? staff = await DBController.Instance.GetStaffAsync(_login);
                    if (staff == null)
                    {
                        //Logger.Instance.ERROR("Failed to retrieve staff information for debug login");
                        MessageBox.Show("An error occurred while retrieving staff information!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        //Logger.Instance.INFO("Staff information retrieved successfully for debug login");
                        await DBController.Instance.SelectCurrentStaffAsync(_login);
                        Dispatcher.Invoke(() =>
                        {
                            //Logger.Instance.INFO("Opening Menu window and closing login window after debug login");
                            Menu menu = new Menu();
                            menu.Show();
                            Close();
                        });
                    }
                }
                else
                {
                    //Logger.Instance.WARN("Debug login failed - incorrect credentials");
                    MessageBox.Show("Incorrect login or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Dispatcher.Invoke(() =>
                {
                    //Logger.Instance.DEBUG("Restoring cursor and re-enabling controls after debug login attempt");
                    Mouse.OverrideCursor = null;
                    AvailableControls(true);
                });
            });
        }

    }
}