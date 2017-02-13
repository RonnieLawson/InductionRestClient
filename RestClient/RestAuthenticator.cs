using System;
using System.Net;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using RestSharp;
using RestSharp.Authenticators;

namespace InductionRestAPI
{
    public class RestAuthenticator : IRestAuthenticator
    {
        public bool IsAuthenticated => SessionId != null;
        public Guid? SessionId { get; private set; }

        private RestSharp.RestClient _restClient;
        private string _requestResource;

        public RestAuthenticator(string apiBaseUri, string requestResource, string username, string password)
        {
            _requestResource = requestResource;
            _restClient = new RestSharp.RestClient()
            {
                Authenticator = new HttpBasicAuthenticator(username, password),
                BaseUrl = new Uri(apiBaseUri)
            };
        }

        public HttpStatusCode Authenticate()
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
