using System.Collections.Generic;
using System.Linq;

namespace Messenger.Domain
{
    public class Chat:DomainObject
    {
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }

        public string LastMessage => Messages?.LastOrDefault().Content;

        public Chat()
        {
            Users = new List<User>();
            Messages = new List<Message>();
        }
    }
}
