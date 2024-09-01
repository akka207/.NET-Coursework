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
            logInControl.IsEnabled = value;
        }

        private void logInControl_OnLogIn(object? sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            AvailableControls(false);

            string _login = logInControl.Login, _password = logInControl.Password;
            Task.Run(() =>
            {
                if (DBController.CheckPassword(_login, _password) == true)
                {
                    Staff? staff = DBController.GetStaff(_login, _password);
                    if (staff == null)
                    {
                        MessageBox.Show("Occured error while getting staff information!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        DBController.SelectCurrentStaff(_login, _password);
                        Dispatcher.Invoke(() => 
                        { 
                            Menu menu = new Menu();
                            menu.Show();
                            Close();
                        });
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect login or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                    AvailableControls(true);
                });
            });
        }

        private void logInControl_OnSignUp(object sender, EventArgs e)
        {
            logInControl.Visibility = Visibility.Hidden;
            registerControl.Visibility = Visibility.Visible;
        }

        private void registerControl_OnSignUp(object sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            AvailableControls(false);

            string _login = registerControl.Login,
                _fullname = registerControl.Fullname,
                _password = registerControl.Password,
                _passwordC = registerControl.PasswordC,
                _phone = registerControl.Phone,
                _email = registerControl.Email;

            var person = new Person() { Login = _login, FullName = _fullname, Phone = _phone, Email = _email };

            Task.Run(() =>
            {
                var passwordValidatorMessages = DataValidators.ValidatePassword(_password, _passwordC);
                if (passwordValidatorMessages.Count > 0)
                {
                    MessageBox.Show(String.Join('\n', passwordValidatorMessages), "Invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var personValidatorMessages = DataValidators.ValidatePerson(person);
                    if (personValidatorMessages.Count > 0)
                    {
                        MessageBox.Show(String.Join('\n', personValidatorMessages), "Invalid data", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        DBController.RegisterPerson(person, _password);
                        MessageBox.Show("Succesfull!", "Progress", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }

                Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                    AvailableControls(true);
                });
            });
        }

        private void registerControl_OnLogIn(object sender, EventArgs e)
        {
            logInControl.Visibility = Visibility.Visible;
            registerControl.Visibility = Visibility.Hidden;
        }

        private void logInControl_OnDebugLogIn(object sender, EventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            AvailableControls(false);

            string _login = "Akka 207", _password = "!Qwerty1";
            Task.Run(() =>
            {
                if (DBController.CheckPassword(_login, _password) == true)
                {
                    Staff? staff = DBController.GetStaff(_login, _password);
                    if (staff == null)
                    {
                        MessageBox.Show("Occured error while getting staff information!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        DBController.SelectCurrentStaff(_login, _password);
                        Dispatcher.Invoke(() =>
                        {
                            Menu menu = new Menu();
                            menu.Show();
                            Close();
                        });
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect login or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Dispatcher.Invoke(() =>
                {
                    Mouse.OverrideCursor = null;
                    AvailableControls(true);
                });
            });
        }
    }
}