using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace Messenger.Domain
{
    public class User:DomainObject
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

        public List<Chat> Chats { get; set; }

        public User()
        {
            Chats = new List<Chat>();
        }
    }
}
