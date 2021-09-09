using System;

namespace Messenger.Domain
{
    public class Message:DomainObject
    {
        public User User { get; set; }
        public Chat Chat { get; set; }
        public DateTime CreationDate { get; set; }
        public string Content { get; set; }
    }


}