using System.Windows.Input;
using Messenger.Client.Events;
using Messenger.Client.MVVM.Services;
using Messenger.Client.MVVM.Views.Services;
using Messenger.Domain;
using Messenger.Shared;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;

namespace Messenger.Client.MVVM.ViewModels
{
    public class LoginViewModel:ObservableObject, ILoginViewModel
    {
        private readonly ILoginService _loginService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IEventAggregator _eventAggregator;

        public LoginViewModel(ILoginService loginService, IMessageDialogService messageDialogService, IEventAggregator eventAggregator)
        {
            _loginService = loginService;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;

            LoginCommand = new DelegateCommand(Login);
            SwitchToRegisterCommand = new DelegateCommand(SwitchToRegisterExecute);
            UserCredential = new UserCredential();
        }


        public UserCredential UserCredential { get; set; }
        public ICommand LoginCommand { get; }
        public ICommand SwitchToRegisterCommand { get; }

        private async void Login()
        {
            var responseProtocol = await _loginService.Login(UserCredential);

            if(responseProtocol.ResponseType == ServerResponseType.UserNotFound)
                _messageDialogService.ShowInfoDialog("Username is not exist.", "Error");
            else if (responseProtocol.ResponseType == ServerResponseType.PasswordIsWrong)
                _messageDialogService.ShowInfoDialog("Password is wrong.", "Error");
            else if (responseProtocol.ResponseType == ServerResponseType.AuthenticationSuccess)
            {
                var user = JsonConvert.DeserializeObject<User>(responseProtocol.Content);

                _messageDialogService.ShowInfoDialog("Successfully logged in", "Info");
                _eventAggregator.GetEvent<OpenMessengerViewEvent>().Publish(user);
            }
        }

        private void SwitchToRegisterExecute()
        {
            _eventAggregator.GetEvent<OpenRegisterViewEvent>().Publish(nameof(RegisterViewModel));
        }
    }

    public interface ILoginViewModel
    {

    }
}