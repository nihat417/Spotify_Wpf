using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace testmp3_1.View;

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

