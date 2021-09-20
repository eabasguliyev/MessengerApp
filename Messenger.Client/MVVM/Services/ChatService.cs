using System.Threading.Tasks;
using Messenger.Domain;
using Messenger.Shared;
using Newtonsoft.Json;

namespace Messenger.Client.MVVM.Services
{
    // TODO: Not tested
    public class ChatService:ClientService, IChatService
    {
        public async Task<ResponseProtocol> GetChats(User user)
        {
            var factory = new ProtocolFactory();

            var requestProtocol = factory.CreateRequestProtocol(RequestType.GetChats, JsonConvert.SerializeObject(user));

            var responseProtocol = await SendRequest(requestProtocol);

            return responseProtocol;
        }
    }
}