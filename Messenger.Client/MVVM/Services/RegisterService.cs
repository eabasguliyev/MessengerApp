using System.Threading.Tasks;
using Messenger.Shared;
using Newtonsoft.Json;

namespace Messenger.Client.MVVM.Services
{
    public class RegisterService : ClientService, IRegisterService
    {
        public async Task<ResponseProtocol> Register(UserCredential credentials)
        {
            var factory = new ProtocolFactory();

            var requestProtocol = factory.CreateRequestProtocol(RequestType.Register, JsonConvert.SerializeObject(credentials));

            return await SendRequest(requestProtocol);
        }
    }
}