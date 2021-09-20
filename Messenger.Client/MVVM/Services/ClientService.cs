using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Messenger.Shared;
using Newtonsoft.Json;

namespace Messenger.Client.MVVM.Services
{
    public class ClientService:IClientService
    {
        public async Task<ResponseProtocol> SendRequest(RequestProtocol requestProtocol)
        {
            using var client = new TcpClient();

            client.Connect(new IPEndPoint(IPAddress.Loopback, 4444));

            using var reader = new StreamReader(client.GetStream());
            await using var writer = new StreamWriter(client.GetStream());


            var factory = new ProtocolFactory();

            await writer.WriteLineAsync(JsonConvert.SerializeObject(requestProtocol));
            await writer.FlushAsync();

            var responseProtocol = await Task.Run(() =>
            {
                while (true)
                {
                    var jsonStr = reader?.ReadLine();

                    if (string.IsNullOrWhiteSpace(jsonStr))
                        continue;

                    return JsonConvert.DeserializeObject<ResponseProtocol>(jsonStr);
                }
            });

            return responseProtocol;
        }
    }
}