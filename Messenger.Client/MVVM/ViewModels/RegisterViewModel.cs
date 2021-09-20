using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
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
    public class RegisterViewModel:ObservableObject, IRegisterViewModel
    {
        private readonly IRegisterService _registerService;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IEventAggregator _eventAggregator;

        public RegisterViewModel(IRegisterService registerService, IMessageDialogService messageDialogService, IEventAggregator eventAggregator)
        {
            _registerService = registerService;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;

            UserCredential = new UserCredential();

            RegisterCommand = new DelegateCommand(RegisterExecute);
            SwitchToLoginCommand = new DelegateCommand(SwitchToLoginExecute);
        }

        public UserCredential UserCredential { get; set; }

        public ICommand RegisterCommand { get; }
        public ICommand SwitchToLoginCommand { get; }
        private async void RegisterExecute()
        {
            var responseProtocol = await _registerService.Register(UserCredential);

            if (responseProtocol.ResponseType == ServerResponseType.RegisterSuccess)
            {
                _messageDialogService.ShowInfoDialog("Registration is successfully", "Info");
            }
            else if (responseProtocol.ResponseType == ServerResponseType.UsernameIsExist)
            {
                _messageDialogService.ShowInfoDialog("Username is already exist. Try different username", "Error");
            }
        }

        private void SwitchToLoginExecute()
        {
            _eventAggregator.GetEvent<OpenLoginViewEvent>().Publish(nameof(LoginViewModel));
        }
    }
}