using System.Threading.Tasks;
using Messenger.Shared;

namespace Messenger.Client.MVVM.Services
{
    public interface IRegisterService:IClientService
    {
        Task<ResponseProtocol> Register(UserCredential credentials);
    }
}