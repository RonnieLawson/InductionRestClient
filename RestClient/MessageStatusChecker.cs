using System;
using System.Net;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using RestSharp;
using RestSharp.Serializers;

namespace InductionRestAPI
{
    public class MessageStatusChecker
    {
        private readonly string _requestResource;
        private readonly IRestAuthenticator _authenticator;
        private RestClient _restClient;
        public MessageHeader MessageHeader { get; private set; }

        public bool MessageStatusResponse { get; set; }

        public MessageStatusChecker(string apiBaseUri, string requestResource, IRestAuthenticator authenticator)
        {
            _requestResource = requestResource;
            _authenticator = authenticator;
            _restClient = new RestClient()
            {
                BaseUrl = new Uri(apiBaseUri)
            };

        }

        public HttpStatusCode CheckMessageStatus(Guid messageId)
        {
            if (!_authenticator.IsAuthenticated)
                _authenticator.Authenticate();
            var encodedSession = _authenticator.GetEncodedSession();
            var request = new RestRequest
            {
                Method = Method.GET,
                RequestFormat = DataFormat.Xml,
                XmlSerializer = new DotNetXmlSerializer()
            };

            request.AddHeader("Authorization", $"Basic {encodedSession}");
            request.Resource = _requestResource;

            var response = _restClient.Execute<MessageHeader>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageHeader = response.Data;
            return response.StatusCode;
        }
    }
}