using System.Net;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using RestSharp;

namespace InductionRestAPI.Clients
{
    public class MessageSender : ApiBase
    {
        private readonly string _accountReference;

        public Message MessageToSend { get; set; }

        public SentMessageHeaders MessageSenderHeaders { get; set; }

        public MessageSender(string requestResource, IRestAuthenticator authenticator, string accountReference)
        {
            _accountReference = accountReference;
            Authenticator = authenticator;
            RequestResource = requestResource;

        }
        public override HttpStatusCode Execute()
        {
            Authenticate();

            var restClient = SetupClient();

            var request = SetupRequest(Method.POST, RequestResource);

            request.AddBody(new SendMessageBody(_accountReference, MessageToSend));

            var response = restClient.Execute<SentMessageHeaders>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageSenderHeaders = response.Data;
            return response.StatusCode;
        }
    }
}