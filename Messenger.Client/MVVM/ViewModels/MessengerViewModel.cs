using Messenger.Domain;
using Prism.Events;

namespace Messenger.Client.MVVM.ViewModels
{
    public class MessengerViewModel:ObservableObject, IMessengerViewModel
    {
        private readonly IEventAggregator _eventAggregator;
        public User LoggedUser { get; set; }

        public MessengerViewModel(IChatsViewModel chatsViewModel, IEventAggregator eventAggregator)
        {
            ChatsViewModel = chatsViewModel;
            _eventAggregator = eventAggregator;
        }
        public IChatsViewModel ChatsViewModel { get; }
    }

    public interface IMessengerViewModel
    {
        public User LoggedUser { get; set; }
    }
}