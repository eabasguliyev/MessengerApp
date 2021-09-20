namespace Messenger.Shared
{
    public enum RequestType
    {
        Register,
        Authentication,
        GetChats
    }

    public class RequestProtocol
    {
        public RequestType RequestType { get; set; }
        public string Content { get; set; }
    }
}