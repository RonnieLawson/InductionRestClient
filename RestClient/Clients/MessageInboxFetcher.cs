using System.Net;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using RestSharp;

namespace InductionRestAPI.Clients
{
    public class MessageInboxFetcher : ApiBase
    {
        public MessageHeaders MessageInboxHeaders { get; private set; }

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

            var response = restClient.Execute<MessageHeaders>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageInboxHeaders = response.Data;

            return response.StatusCode;
        }
    }
}