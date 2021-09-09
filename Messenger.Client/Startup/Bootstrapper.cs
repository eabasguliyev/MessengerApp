using System.Net.Sockets;
using Autofac;
using Messenger.Client.MVVM.ViewModels;
using Messenger.Client.MVVM.Views;
using Messenger.Client.MVVM.Views.Services;
using Prism.Events;

namespace Messenger.Client.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<MainWindowView>().SingleInstance();
            builder.RegisterType<MainWindowViewModel>().As<IMainWindowViewModel>().SingleInstance();

            builder.RegisterType<RegisterViewModel>().As<IRegisterViewModel>().SingleInstance();

            builder.RegisterType<MessengerClient>().AsSelf().SingleInstance();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>();
            
            return builder.Build();
        }
    }
}