using System.Threading.Tasks;
using Messenger.Shared;

namespace Messenger.Client.MVVM.Services
{
    public interface ILoginService:IClientService
    {
        Task<ResponseProtocol> Login(UserCredential credential);
    }
}