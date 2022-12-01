using System.Linq;
using System.Windows;
using System.Windows.Input;
using testmp3_1.Models;

namespace testmp3_1.View;

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
        if (UserTb.Text.Length < 4)
        {
            MessageBox.Show("Lenght of username is smaller than 3", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            UserTb.Text = string.Empty;
        }
        else if (PasswordTb.Password.Length < 5)
        {
            MessageBox.Show("Lenght of password is smaller than 5", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            PasswordTb.Password = string.Empty;
        }
        else
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
    }

    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        UserLogin loginpage = new UserLogin();
        this.Close();
        loginpage.Show();
    }
}

