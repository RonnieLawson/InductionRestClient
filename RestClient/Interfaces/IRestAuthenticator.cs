using System.Net;

namespace RestClient.Interfaces
{
    public interface IRestAuthenticator
    {
        bool IsAuthenticated { get; }
        HttpStatusCode Execute();
        string GetEncodedSession();
    }
}