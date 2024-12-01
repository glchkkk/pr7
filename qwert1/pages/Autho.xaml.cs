using Microsoft.VisualBasic.ApplicationServices;
using qwert1.Servises;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Data.Entity;
using qwert1.Model;
using System.Runtime.Remoting.Contexts;
using System.Linq;
using System;
using System.Windows.Threading;



namespace qwert1.pages
{
    
    public partial class Autho : Page
    {
        private const int MaxFailedAttempts = 3;
        private int failedLoginAttempts = 0;
        private DispatcherTimer lockoutTimer;
        private TimeSpan lockoutDuration = TimeSpan.FromSeconds(10);

        int click;

        public Autho()
        {
            InitializeComponent();
            click = 0;
            lockoutTimer = new DispatcherTimer();
            lockoutTimer.Interval = TimeSpan.FromMilliseconds(100);
            
        }
        private void LockWindow(TimeSpan duration)
        {

            tbLogin.IsEnabled = false;
            tbPassword.IsEnabled = false;
            btnEnter.IsEnabled = false;
            btnEnterGuests.IsEnabled = false;
            tbCaptcha.IsEnabled = false;
            LockoutTimerTextBlock.Visibility = Visibility.Visible;
            LockoutTimerTextBlock.Text = $"Заблокировано на {duration.TotalSeconds} сек.";
            lockoutTimer.Tick += LockoutTimer_Tick;
            lockoutTimer.Start();
          
        }

        private void UnlockWindow()
        {

            tbLogin.IsEnabled = true;
            tbPassword.IsEnabled = true;
            btnEnter.IsEnabled = true;
            btnEnterGuests.IsEnabled= true;
            tbCaptcha.IsEnabled = true;
            LockoutTimerTextBlock.Visibility = Visibility.Collapsed;
            lockoutTimer.Tick -= LockoutTimer_Tick;
            lockoutTimer.Stop();
            failedLoginAttempts = 0;
            GenerateCapctcha();
            lockoutDuration = TimeSpan.FromSeconds(10);
        }

        private void LockoutTimer_Tick(object sender, EventArgs e)
        {
            lockoutDuration = lockoutDuration.Subtract(lockoutTimer.Interval);
            LockoutTimerTextBlock.Text = $"Заблокировано на {lockoutDuration.Seconds} сек.";
            if (lockoutDuration <= TimeSpan.Zero)
            {
                UnlockWindow();
            }
        }

        private void GenerateCapctcha()
        {
            try
            {
                tbCaptcha.Visibility = Visibility.Visible;
                tblCaptcha.Visibility = Visibility.Visible;

                string captchaText = CapthcaGenerator.GenerateCaptchaText(6);
                tblCaptcha.Text = captchaText;
                tblCaptcha.TextDecorations = TextDecorations.Strikethrough;
            }
            catch (Exception ex)
            {
 
                Console.WriteLine($"Ошибка при генерации капчи: {ex.Message}");
            }
        }



        private void btnEnterCLick(object sender, RoutedEventArgs e)
        {
            click += 1;
            string login = tbLogin.Text.Trim();
            string password = tbPassword.Text.Trim();
            string hashPassword = Hash.HashPassword(password);
            using (var context = new bakeryEntities())
            {
                var user = context.Authentication.Where(a => a.Login == login && a.Password == hashPassword).FirstOrDefault();

                if (click == 1)
                {
                    if (user != null)
                    {
                        MessageBox.Show("Вы вошли под: " + user.Role.Name); 
                        LoadPage(user.Role.Name, user);
                        
                    }
                    else
                    {
                        MessageBox.Show("Вы ввели логин или пароль неверно!");
                        GenerateCapctcha();

                    }
                }
                else if (click > 1)
                {
                    if (user != null && tbCaptcha.Text == tblCaptcha.Text)
                    {
                        MessageBox.Show("Вы вошли под: " + user.Role.Name.ToString());
                        LoadPage(user.Role.Name.ToString(), user);
                        failedLoginAttempts = 0;
                    }
                    else
                    {
                        failedLoginAttempts++;
                        MessageBox.Show("Вы ввели логин или пароль неверно!");

                        if (failedLoginAttempts >= MaxFailedAttempts)
                        {
                            LockWindow(lockoutDuration);
                        }
                        else
                        {
                            GenerateCapctcha();
                        }
                    }
                    
                }
               
            }
        }


        private void LoadPage(string _role, Authentication user)
        {
            click = 0;
            switch (_role)
            {
                case "client":
                    NavigationService.Navigate(new Client(user, _role));
                    break;
                case "admin":
                    NavigationService.Navigate(new Client(user, _role));
                    break;
                case "manager":
                    NavigationService.Navigate(new Client(user, _role));
                    break;
            }
        }


        private void btnEnterGuestsClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null, null));
        }

    }
}

