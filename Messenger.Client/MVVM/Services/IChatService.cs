using System.Threading.Tasks;
using Messenger.Domain;
using Messenger.Shared;

namespace Messenger.Client.MVVM.Services
{
    public interface IChatService
    {
        Task<ResponseProtocol> GetChats(User user);
    }
}