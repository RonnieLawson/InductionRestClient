using System.Net;
using RestSharp;

namespace InductionRestAPI.Clients
{
    public interface IApiBase
    {
        HttpStatusCode Execute();
        RestRequest SetupRequest(Method httpMethod, string resource);
        void Authenticate();
        RestClient SetupClient();
    }
}