using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace testmp3_1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
}
