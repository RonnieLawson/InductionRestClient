using System;
using System.Net;
using RestClient.Clients;
using RestClient.Interfaces;
using RestSharp;
using RestSharp.Serializers;

namespace RestClient
{
    public abstract class ApiBase : IApiBase
    {
        protected IRestAuthenticator Authenticator;
        protected string RequestResource;
        protected Uri ApiBaseUri = new Uri("https://api.esendex.com");

        public RestRequest SetupRequest(Method httpMethod, string resource)
        {
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

        public void Authenticate()
        {
            if (!Authenticator.IsAuthenticated)
                Authenticator.Execute();
        }

        public RestSharp.RestClient SetupClient()
        {          
            return new RestSharp.RestClient
            {
                BaseUrl = ApiBaseUri
            };
        }

        public abstract HttpStatusCode Execute();

    }
}