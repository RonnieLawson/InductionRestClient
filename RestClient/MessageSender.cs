using System;
using System.Net;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using RestSharp;
using RestSharp.Serializers;

namespace InductionRestAPI
{
    public class MessageSender
    {
        private readonly IRestAuthenticator _restAuthenticator;
        private readonly string _requestResource;
        private RestSharp.RestClient _restClient;

        public MessageSender(string apiBaseUri, string requestResource, IRestAuthenticator restAuthenticator)
        {
            _restAuthenticator = restAuthenticator;
            _requestResource = requestResource;
            _restClient = new RestSharp.RestClient()
            {
                BaseUrl = new Uri(apiBaseUri)
            };
        }

        public HttpStatusCode SendMessage(string numberToSendTo, string messageToSend, string accountReference)
        {
            if (!_restAuthenticator.IsAuthenticated)
                _restAuthenticator.Authenticate();
            var encodedSession = _restAuthenticator.GetEncodedSession();
            var request = new RestRequest
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Xml,
                XmlSerializer = new DotNetXmlSerializer()
            };

            request.AddHeader("Authorization", $"Basic {encodedSession}");
            request.Resource = _requestResource;
            var sendMessageBody = new SendMessageBody(accountReference, numberToSendTo, messageToSend);
            
            request.AddBody(sendMessageBody);

            var response = _restClient.Execute<SendMessageResponse>(request);

            return response.StatusCode;
        }
    }
}