using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
                var context = new ApplicationContext();

                _ = Task.Run(() =>
                {
                    using var stream = client.GetStream();

                    using var reader = new StreamReader(stream);
                    using var writer = new StreamWriter(stream);

                    while (true)
                    {
                        var jsonStr = reader.ReadLine();

                        if(String.IsNullOrWhiteSpace(jsonStr))
                            continue;
                        
                        var requestProtocol = JsonConvert.DeserializeObject<RequestProtocol>(jsonStr);

                        if (requestProtocol == null)
                            throw new NullReferenceException("Protocol is null");
                        

                        ResponseProtocol responseProtocol = null;

                        switch (requestProtocol.RequestType)
                        {
                            case RequestType.Register:
                            {
                                UserCredential credential = JsonConvert.DeserializeObject<UserCredential>(requestProtocol.Content);

                                responseProtocol = UserRegistration(credential, context);
                            }
                                break;
                            case RequestType.Authentication:
                            {
                                UserCredential credential = JsonConvert.DeserializeObject<UserCredential>(requestProtocol.Content);

                                responseProtocol = UserAuthentication(credential, context);
                            }
                                break;
                            case RequestType.GetChats:
                            {
                                // Test here
                                User user = JsonConvert.DeserializeObject<User>(requestProtocol.Content);

                                if (user == null)
                                {
                                    responseProtocol =
                                        _protocolFactory.CreateResponseProtocol(ServerResponseType.UnknownUser);
                                }
                                else
                                {
                                    var chats = context.Chats.Where(c => c.Users.Any(u => u.Username == user.Username)).ToList();

                                    responseProtocol =
                                        _protocolFactory.CreateResponseProtocol(ServerResponseType.Success,
                                            JsonConvert.SerializeObject(chats));
                                }
                            }
                                break;
                            default:
                                break;
                        }

                        if (context.ChangeTracker.HasChanges())
                            context.SaveChanges();

                        writer.WriteLine(JsonConvert.SerializeObject(responseProtocol));
                        writer.Flush();
                    }
                });
            } while (true);
        }

        private ResponseProtocol UserAuthentication(UserCredential credential, ApplicationContext context)
        {
            if (credential == null)
                throw new NullReferenceException("Credential is null");

            var user = context.Users.SingleOrDefault(u => u.Username == credential.Username);

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

        private ResponseProtocol UserRegistration(UserCredential credential, ApplicationContext context)
        {
            if (credential == null)
                throw new NullReferenceException("Credential is null");

            var user = context.Users.SingleOrDefault(u => u.Username == credential.Username);

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
                
                context.Users.Add(newUser);

                return _protocolFactory.CreateResponseProtocol(ServerResponseType.RegisterSuccess,
                    JsonConvert.SerializeObject(newUser));
            }
        }
    }
}