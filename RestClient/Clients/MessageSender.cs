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

        public MessageHeaders MessageSenderHeaders { get; private set; }

        public MessageSender(string requestResource, IRestAuthenticator authenticator)
        {
            Authenticator = authenticator;
            RequestResource = requestResource;

        }
        public HttpStatusCode Execute()
        {
            Authenticate();

            var restClient = SetupClient();

            var request = SetupRequest(Method.POST, RequestResource);

            request.AddBody(new SendMessageBody(AccountReference, MessageToSend));

            var response = restClient.Execute<MessageHeaders>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageSenderHeaders = response.Data;
            return response.StatusCode;
        }
    }
}