using System.Threading.Tasks;
using Messenger.Shared;

namespace Messenger.Client.MVVM.Services
{
    public interface IClientService
    {
        Task<ResponseProtocol> SendRequest(RequestProtocol requestProtocol);
    }
}