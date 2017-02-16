using System;
using System.Net;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using RestSharp;

namespace InductionRestAPI.Clients
{
    public class MessageStatusChecker: ApiBase
    {

        public MessageHeader MessageHeader { get; private set; }

        public bool MessageStatusResponse { get; set; }
        public string MessageHeaderId { get; set; }

        public MessageStatusChecker(string requestResource, IRestAuthenticator authenticator)
        {
            RequestResource = requestResource;
            Authenticator = authenticator;
        }

        public override HttpStatusCode Execute()
        {
            Authenticate();

            var restClient = SetupClient();

            var request = SetupRequest(Method.GET, RequestResource + MessageHeaderId.ToString());

            var response = restClient.Execute<MessageHeader>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageHeader = response.Data;
            return response.StatusCode;
        }
    }
}