using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Transactions;
using Messenger.Shared;
using Newtonsoft.Json;

namespace Messenger.Client
{
    public class MessengerClient:IDisposable
    {
        private readonly int _port;
        private TcpClient _client;
        public MessengerClient(int port = 4444)
        {
            _port = port;
            _client = new TcpClient();

        }

        public void Dispose()
        {
            _client?.Dispose();
        }

        public async Task ConnectAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                _client.Connect(new IPEndPoint(IPAddress.Loopback, _port));
            });
        }

        public async Task DisconnectAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                _client.EndConnect(new CommittableTransaction());
            });
        }

        public async Task SendAsync(RequestProtocol requestProtocol)
        {
            await using var writer = new StreamWriter(_client.GetStream());

            await writer.WriteAsync(JsonConvert.SerializeObject(requestProtocol)).ConfigureAwait(false);
            //await writer.FlushAsync().ConfigureAwait(false);
        }

        public async Task<ResponseProtocol> ReceiveAsync()
        {
            while (true)
            {
                if (_client.Available > 0)
                {
                    using var reader = new StreamReader(_client.GetStream());

                    return JsonConvert.DeserializeObject<ResponseProtocol>(await reader.ReadToEndAsync());
                }
            }
            
        }
    }
}