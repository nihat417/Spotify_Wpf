using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace testmp3_1.View
{
    /// <summary>
    /// Логика взаимодействия для UserLogin.xaml
    /// </summary>
    public partial class UserLogin : Window
    {
        bool normalized = true;
        public UserLogin()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void Btnminsize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Btnfullscreen_Click(object sender, RoutedEventArgs e)
        {
            if (normalized == true)
            {
                WindowState = WindowState.Maximized;
                normalized = false;
            }
            else
            {
                WindowState = WindowState.Normal;
                normalized = true;
            }

        }

        private void Exitbtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string Username = logintextbox.Text;
            string Password = passwordtextbox.Password;


            using (UserDataContext context = new UserDataContext())
            {
                bool userfound = context.users.Any(user => user.Name == Username && user.Password == Password);
            
                if (userfound)
                {
                    MainWindow mainWindow = new MainWindow();
                    Close();
                    mainWindow.Show();
                }
                else
                    MessageBox.Show("yoxdu bele user");
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Register registerpage = new Register();
            this.Close();
            registerpage.Show();
        }
    }
}

