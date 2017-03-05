using System.Net;
using RestClient.Interfaces;
using RestClient.Models;
using RestSharp;

namespace RestClient.Clients
{
    public class MessageStatusChecker: ApiBase
    {
        public MessageHeader MessageHeader { get; set; }

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

            var request = SetupRequest(Method.GET, $"{RequestResource}/{MessageHeaderId}");

            var response = restClient.Execute<MessageHeader>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageHeader = response.Data;
            return response.StatusCode;
        }
    }
}