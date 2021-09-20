using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Messenger.Shared;
using Newtonsoft.Json;

namespace Messenger.Client.MVVM.Services
{
    public class LoginService:ClientService, ILoginService
    {
        public async Task<ResponseProtocol> Login(UserCredential credentials)
        {
            var factory = new ProtocolFactory();

            var requestProtocol = factory.CreateRequestProtocol(RequestType.Authentication, JsonConvert.SerializeObject(credentials));

            return await SendRequest(requestProtocol);
        }
    }
}