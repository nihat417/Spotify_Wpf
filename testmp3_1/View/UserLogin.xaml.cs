using System.Linq;
using System.Windows;
using System.Windows.Input;
using testmp3_1.Db;

namespace testmp3_1.View;

public partial class UserLogin : Window
{
    bool normalized = true;
    public UserLogin()
    {
        InitializeComponent();
    }

    #region Events

    /// <summary>
    /// Mouse Down Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    /// <summary>
    /// Minimizer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    /// <summary>
    /// Maximizer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnFullscreen_Click(object sender, RoutedEventArgs e)
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

    /// <summary>
    /// Closer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void exitBtn_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    /// <summary>
    /// logger
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                MessageBox.Show("There is no such user");
        }
    }

    /// <summary>
    /// Page changer
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void textBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Register registerpage = new Register();
        Close();
        registerpage.Show();
    }

    #endregion
}

