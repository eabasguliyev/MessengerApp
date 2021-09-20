using System.Net.Sockets;
using Autofac;
using Messenger.Client.MVVM.Services;
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
            builder.RegisterType<LoginViewModel>().As<ILoginViewModel>().SingleInstance();
            builder.RegisterType<MessengerViewModel>().As<IMessengerViewModel>().SingleInstance();
            builder.RegisterType<ChatsViewModel>().As<IChatsViewModel>().SingleInstance();

            builder.RegisterType<RegisterService>().As<IRegisterService>().SingleInstance();
            builder.RegisterType<LoginService>().As<ILoginService>().SingleInstance();

            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();

            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            
            return builder.Build();
        }
    }
}