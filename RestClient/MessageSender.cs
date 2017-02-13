using System;
using System.Net;
using System.Xml.Serialization;
using InductionRestAPI.Interfaces;
using RestSharp;
using RestSharp.Deserializers;
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

    [XmlRoot("messages")]
    public class SendMessageBody
    {
        public SendMessageBody(string accountReference, string numberToSendTo, string messageToSend)
        {
            Accountreference = accountReference;
            Message = new Message(numberToSendTo, messageToSend);
        }

        [XmlElement("accountreference")]
        public string Accountreference { get; set; }
        [XmlElement("message")]
        public Message Message { get; set; }

        public SendMessageBody()
        { }
    }

    [XmlRoot("message")]
    public class Message
    {
        public Message(string numberToSendTo, string messageToSend)
        {
            To = numberToSendTo;
            Body = messageToSend;
        }

        [XmlElement("to")]
        public string To { get; set; }
        [XmlElement("body")]
        public string Body { get; set; }

        public Message()
        { }
    }

    [DeserializeAs(Name = "messageheaders")]
    public class SendMessageResponse
    {
        public Guid BatchId { get; set; }
        public MessageHeader MessageHeader { get; set; }

    }

    [DeserializeAs(Name = "messageheader")]
    public class MessageHeader
    {
        public string Uri { get; set; }
        public Guid Id { get; set; }
    }
}