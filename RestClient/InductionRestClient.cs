using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using RestClient;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace InductionRestClient
{
    public class InductionClient
    {
        public ICredentialsManager CredentialsManager { get; }

        public Guid SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        private Guid _sessionId;

        public HttpWebResponse SendMessage()
        {
            throw new NotImplementedException();
        }

        public HttpStatusCode Authenticate()
        {
            var restClient = new RestSharp.RestClient
            {
                Authenticator =
                    new HttpBasicAuthenticator(ConfigurationManager.AppSettings["RestUsername"],
                        ConfigurationManager.AppSettings["RestPassword"]),
                BaseUrl = new Uri(ConfigurationManager.AppSettings["BaseRestAPI"])
            };

            var request = new RestRequest {Method = Method.POST};
            request.AddHeader("Accept", "application/json");
            request.Resource = ConfigurationManager.AppSettings["AuthenticationEndpoint"];

            var response = restClient.Execute(request);
     
            var jsonDeserialiser = new JsonDeserializer();
            var authenticationResponse = jsonDeserialiser.Deserialize<Dictionary<string, Guid>>(response);
            SessionId = authenticationResponse["id"];
            return response.StatusCode;
        }
    }
}
