using InductionRestAPI.Interfaces;
using System.Net;
using InductionRestAPI.Models;
using RestSharp;

namespace InductionRestAPI
{
    public class MessageInboxFetcher : ApiBase
    {
        private readonly string _resource;
        private readonly IRestAuthenticator _restAuthenticator;

        public MessageHeaders MessageInboxHeaders { get; private set; }

        public MessageInboxFetcher(string resource, IRestAuthenticator restAuthenticator)
        {
            RequestResource = resource;
            Authenticator = restAuthenticator;
        }

        public HttpStatusCode Execute()
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