using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using testmp3_1.View;

namespace testmp3_1;

public partial class MainWindow : Window
{
    bool normalized = true;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed)
            DragMove();
    }

    private void Btn_Click(object sender, RoutedEventArgs e)
    {
        var btn = sender as Button;

        if(btn == btnMinimize)
            WindowState = WindowState.Minimized;
        else if(btn == btnMaximize) 
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
        else if(btn == btnClose)
            Application.Current.Shutdown();
    }

    private void btnLogOut_Click(object sender, RoutedEventArgs e)
    {
        UserLogin user = new();
        Close();
        user.Show();
    }
}
