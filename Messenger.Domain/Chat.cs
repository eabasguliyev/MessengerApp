using System.Collections.Generic;

namespace Messenger.Domain
{
    public class Chat:DomainObject
    {
        public List<User> Users { get; set; }
        public List<Message> Messages { get; set; }
    }
}
