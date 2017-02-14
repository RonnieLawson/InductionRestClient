using System;
using InductionRestAPI.Interfaces;
using RestSharp;
using RestSharp.Serializers;

namespace InductionRestAPI
{
    public class ApiBase
    {
        protected IRestAuthenticator Authenticator;
        protected string RequestResource;
        protected Uri ApiBaseUri = new Uri("https://api.esendex.com");
        protected RestClient RestClient;

        protected RestRequest SetupRequest(Method httpMethod, string resource)
        {
            SetupClient();

            Authenticate();

            var request = new RestRequest
            {
                Method = httpMethod,
                RequestFormat = DataFormat.Xml,
                XmlSerializer = new DotNetXmlSerializer()
            };

            request.AddHeader("Authorization", $"Basic {Authenticator.GetEncodedSession()}");
            request.Resource = resource;
            return request;
        }

        private void Authenticate()
        {
            if (!Authenticator.IsAuthenticated)
                Authenticator.Execute();
        }

        private void SetupClient()
        {
            RestClient = new RestClient
            {
                BaseUrl = ApiBaseUri
            };
        }
    }
}