using System.Net;
using InductionRestAPI.Clients;

namespace InductionRestAPI
{
    public class RestApiClient
    {
        private readonly MessageSender _messageSender;

        public RestApiClient(IApiBase messageSender)
        {
            _messageSender = (MessageSender) messageSender;
        }

        public HttpStatusCode SendMessage(string phoneNumber, string messageText)
        {
            _messageSender.MessageToSend = new Message(phoneNumber, messageText);
            return _messageSender.Execute();
        }
    }
}
