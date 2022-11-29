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
using testmp3_1.Models;

namespace testmp3_1.View
{
    /// <summary>
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        bool normalized = true;

        public Register()
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

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            using (UserDataContext context = new UserDataContext())
            {
                bool userfound = context.users.Any(user => user.Name == UserTb.Text);
                if (!userfound)
                {
                    User u = new(UserTb.Text, PasswordTb.Password);

                    context.users.Add(u);
                    context.SaveChanges();
                }
                else
                    MessageBox.Show("There is already such as User");
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            UserLogin loginpage = new UserLogin();
            this.Close();
            loginpage.Show();
        }
    }
}

