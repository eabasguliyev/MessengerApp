namespace Messenger.Shared
{
    public class ProtocolFactory
    {
        public RequestProtocol CreateRequestProtocol(RequestType type, string content = null)
        {
            return new RequestProtocol()
            {
                RequestType = type,
                Content = content
            };
        }

        public ResponseProtocol CreateResponseProtocol(ServerResponseType type, string content = null)
        {
            return new ResponseProtocol()
            {
                ResponseType = type,
                Content = content
            };
        }
    }
} 