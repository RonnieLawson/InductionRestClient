using System.Net;
using RestClient.Interfaces;
using RestClient.Models;
using RestSharp;

namespace RestClient.Clients
{
    public class MessageInboxFetcher : ApiBase
    {
        public MessageInboxResponse MessageInboxResponse { get; private set; }

        public MessageInboxFetcher(string resource, IRestAuthenticator restAuthenticator)
        {
            RequestResource = resource;
            Authenticator = restAuthenticator;
        }

        public override HttpStatusCode Execute()
        {
            Authenticate();

            var restClient = SetupClient();

            var request = SetupRequest(Method.GET, RequestResource);

            var response = restClient.Execute<MessageInboxResponse>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageInboxResponse = response.Data;

            return response.StatusCode;
        }
    }
}