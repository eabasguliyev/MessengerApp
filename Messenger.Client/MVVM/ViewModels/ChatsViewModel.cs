using System.Collections.ObjectModel;
using Messenger.Domain;

namespace Messenger.Client.MVVM.ViewModels
{
    public class ChatsViewModel:ObservableObject, IChatsViewModel
    {
        public ObservableCollection<Chat> Chats { get; set; }


        public ChatsViewModel()
        {
            
        }
    }

    public interface IChatsViewModel
    {

    }
}