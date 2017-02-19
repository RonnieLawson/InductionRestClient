using System;
using System.Net;
using CommonUtils;
using RestClient.Interfaces;
using RestClient.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace RestClient.Clients
{
    public class RestAuthenticator : IRestAuthenticator
    {
        public bool IsAuthenticated => SessionId != null;
        public Guid? SessionId { get; private set; }

        private readonly RestSharp.RestClient _restClient;
        private readonly string _requestResource;

        public RestAuthenticator(string apiBaseUri, string requestResource, string username, string password)
        {
            _requestResource = requestResource;
            _restClient = new RestSharp.RestClient()
            {
                Authenticator = new HttpBasicAuthenticator(username, password),
                BaseUrl = new Uri(apiBaseUri)
            };
        }

        public HttpStatusCode Execute()
        {
            var request = new RestRequest { Method = Method.POST };
            request.AddHeader("Accept", "application/xml");
            request.Resource = _requestResource;

            var response = _restClient.Execute<AuthenticationResponse>(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return response.StatusCode;
            SessionId = response.Data.id;

            return response.StatusCode;
        }

        public string GetEncodedSession()
        {

            return SessionId == null ? null : Utility.Encode(SessionId.ToString());
        }
    }
}
