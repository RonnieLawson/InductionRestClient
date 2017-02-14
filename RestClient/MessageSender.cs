using System;
using System.Net;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using RestSharp;

namespace InductionRestAPI
{
    public class MessageSender : ApiBase
    {
        public string AccountReference { get; set; }

        public Message MessageToSend { get; set; }

        public SendMessageResponse MessageSenderResponse { get; private set; }

        public MessageSender(string requestResource, IRestAuthenticator authenticator)
        {
            Authenticator = authenticator;
            RequestResource = requestResource;

        }
        public HttpStatusCode Execute()
        {
            var request = SetupRequest(Method.POST, RequestResource);

            var sendMessageBody = new SendMessageBody(AccountReference, MessageToSend);
            
            request.AddBody(sendMessageBody);

            var response = RestClient.Execute<SendMessageResponse>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageSenderResponse = response.Data;
            return response.StatusCode;
        }
    }
}