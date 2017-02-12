using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace InductionClient.RestAuthenticator
{


    public class RestAuthenticator : IRestAuthenticator
    {
        public bool IsAuthenticated => SessionId != null;
        public Guid? SessionId { get; private set; }

        private RestClient _restClient;
        private string _requestResource;


        public RestAuthenticator(string username, string password, string apiBaseUri, string requestResource)
        {
            _requestResource = requestResource;
            _restClient = new RestClient()
            {
                Authenticator = new HttpBasicAuthenticator(username, password),
                BaseUrl = new Uri(apiBaseUri)
            };
        }

        public HttpStatusCode Authenticate()
        {
            var request = new RestRequest { Method = Method.POST };
            request.AddHeader("Accept", "application/json");

            request.Resource = _requestResource;

            var response = _restClient.Execute(request);

            var jsonDeserialiser = new JsonDeserializer();
            var authenticationResponse = jsonDeserialiser.Deserialize<Dictionary<string, Guid>>(response);
            SessionId = authenticationResponse["id"];
            return response.StatusCode;
        }
    }
        



    
}
