using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Input;
using Messenger.Client.Events;
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
        private readonly MessengerClient _client;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IEventAggregator _eventAggregator;

        public RegisterViewModel(MessengerClient client, IMessageDialogService messageDialogService, IEventAggregator eventAggregator)
        {
            _client = client;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;

            UserCredential = new UserCredential();

            RegisterCommand = new DelegateCommand(RegisterAsync);
        }

        public UserCredential UserCredential { get; set; }

        public ICommand RegisterCommand { get; }
        private async void RegisterAsync()
        {
            await _client.ConnectAsync();

            var protocolFactory = new ProtocolFactory();

            // Todo: Validate UserCredential

            var requestProtocol =
                protocolFactory.CreateRequestProtocol(RequestType.Register,
                    JsonConvert.SerializeObject(UserCredential));

            await _client.SendAsync(requestProtocol);

            var responseProtocol = await _client.ReceiveAsync();

            if (responseProtocol.ResponseType == ServerResponseType.UsernameIsExist)
            {
                _messageDialogService.ShowInfoDialog("Username is exist", "Info");
                
            }
            else
            {
                var user = JsonConvert.DeserializeObject<User>(responseProtocol.Content);

                _messageDialogService.ShowInfoDialog($"Account created. Welcome {user.Username}", "Info");

                _eventAggregator.GetEvent<OpenChatsViewEvent>().Publish(user);
            }

        }
    }
}