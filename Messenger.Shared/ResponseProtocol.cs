namespace Messenger.Shared
{
    public enum ServerResponseType
    {
        RegisterSuccess,
        UsernameIsExist,
        AuthenticationSuccess,
        UserNotFound,
        PasswordIsWrong,
    }

    public class ResponseProtocol
    {
        public ServerResponseType ResponseType { get; set; }
        public string Content { get; set; }
    }
}