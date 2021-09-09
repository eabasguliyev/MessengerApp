using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Messenger.Client.MVVM.ViewModels;
using Messenger.Client.MVVM.Views;
using Messenger.Client.Startup;

namespace Messenger.Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var container = new Bootstrapper().Bootstrap();

            var mainWindow = container.Resolve<MainWindowView>();

            mainWindow.DataContext = container.Resolve<IMainWindowViewModel>();

            mainWindow.Show();
        }
    }
}
