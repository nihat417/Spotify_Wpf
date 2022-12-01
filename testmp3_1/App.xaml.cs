using Microsoft.EntityFrameworkCore.Infrastructure;
using System.IO;
using System.Windows;

namespace testmp3_1;

public partial class App : Application
{
    public static string path = Directory.GetCurrentDirectory();
    public App()
    {
        byte b = 0;
        while (b++ < 4)
            path = Directory.GetParent(path)!.ToString();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        DatabaseFacade facade = new DatabaseFacade(new UserDataContext());

        facade.EnsureCreated();
    }
}
