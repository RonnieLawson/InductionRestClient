using System.Net;
using RestSharp;

namespace RestClient.Clients
{
    public interface IApiBase
    {
        HttpStatusCode Execute();
        RestRequest SetupRequest(Method httpMethod, string resource);
        void Authenticate();
        RestSharp.RestClient SetupClient();
    }
}