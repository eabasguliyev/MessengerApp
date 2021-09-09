using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Messenger.Domain;
using Messenger.EntityFrameworkCore;
using Messenger.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Messenger.Server
{
    public class MessengerServer
    {
        private readonly ProtocolFactory _protocolFactory;

        public MessengerServer()
        {
            _protocolFactory = new ProtocolFactory();
        }

        public void Start(int port = 4444)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, port);

            TcpListener server = new TcpListener(endPoint);

            Console.WriteLine("Server is running");

            server.Start();

            do
            {
                TcpClient client = server.AcceptTcpClient();

                Console.WriteLine($"Client is connected. Ip address is {client.Client.RemoteEndPoint}");

                _ = Task.Run(async () =>
                {
                    await using var stream = client.GetStream();

                    using var reader = new StreamReader(stream);
                    await using var writer = new StreamWriter(stream);

                    var jsonStr = await reader.ReadToEndAsync().ConfigureAwait(false);

                    var requestProtocol = JsonConvert.DeserializeObject<RequestProtocol>(jsonStr);

                    if (requestProtocol == null)
                        throw new NullReferenceException("Protocol is null");
                    
                    await using var context = new ApplicationContext();

                    ResponseProtocol responseProtocol = null;

                    switch (requestProtocol.RequestType)
                    {
                        case RequestType.Register:
                        {
                            UserCredential credential = JsonConvert.DeserializeObject<UserCredential>(requestProtocol.Content);

                            responseProtocol = await UserRegistration(credential, context);
                        }
                            break;
                        case RequestType.Authentication:
                        {
                            UserCredential credential = JsonConvert.DeserializeObject<UserCredential>(requestProtocol.Content);

                            responseProtocol = await UserAuthentication(credential, context);
                        }
                            break;
                        default:
                            break;
                    }

                    if (context.ChangeTracker.HasChanges())
                        await context.SaveChangesAsync();

                    await writer.WriteAsync(JsonConvert.SerializeObject(responseProtocol)).ConfigureAwait(false);
                    //await writer.FlushAsync().ConfigureAwait(false);

                    //client.Dispose();
                });
            } while (true);
        }

        private async Task<ResponseProtocol> UserAuthentication(UserCredential credential, ApplicationContext context)
        {
            if (credential == null)
                throw new NullReferenceException("Credential is null");

            var user = await context.Users.SingleAsync(u => u.Username == credential.Username).ConfigureAwait(false);

            if (user == null)
                return _protocolFactory.CreateResponseProtocol(ServerResponseType.UserNotFound);
            else
            {
                if (user.Password != credential.Password)
                    return _protocolFactory.CreateResponseProtocol(ServerResponseType.PasswordIsWrong);
                else
                {
                    user.Token = Guid.NewGuid().ToString();

                    return _protocolFactory.CreateResponseProtocol(ServerResponseType.AuthenticationSuccess,
                        JsonConvert.SerializeObject(user));
                }
            }
            
        }

        private async Task<ResponseProtocol> UserRegistration(UserCredential credential, ApplicationContext context)
        {
            if (credential == null)
                throw new NullReferenceException("Credential is null");

            var user = await context.Users.SingleAsync(u => u.Username == credential.Username).ConfigureAwait(false);

            if (user != null)
                return _protocolFactory.CreateResponseProtocol(ServerResponseType.UsernameIsExist);
            else
            {
                User newUser = new User()
                {
                    Username = credential.Username,
                    Password = credential.Password,
                    Token = Guid.NewGuid().ToString(),
                };
                
                await context.Users.AddAsync(newUser).ConfigureAwait(false);

                return _protocolFactory.CreateResponseProtocol(ServerResponseType.RegisterSuccess,
                    JsonConvert.SerializeObject(newUser));
            }
        }
    }
}