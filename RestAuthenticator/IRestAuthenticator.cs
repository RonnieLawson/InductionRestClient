using System.Net;

namespace InductionClient.RestAuthenticator
{
    public interface IRestAuthenticator
    {
        bool IsAuthenticated { get; }
        HttpStatusCode Authenticate();
    }
}