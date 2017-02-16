using System;
using System.Net;
using InductionRestAPI.Clients;

namespace InductionRestAPI
{
    public class RestApiClient
    {
        private readonly MessageSender _messageSender;
        private readonly MessageStatusChecker _messageStatusChecker;
        private readonly MessageInboxFetcher _messageInboxFetcher;

        public RestApiClient(IApiBase messageSender, IApiBase messageStatusChecker, IApiBase messageInboxFetcher)
        {
            _messageSender = (MessageSender) messageSender;
            _messageStatusChecker = (MessageStatusChecker)messageStatusChecker;
            _messageInboxFetcher = (MessageInboxFetcher)messageInboxFetcher;
        }

        public HttpStatusCode SendMessage(string phoneNumber, string messageText)
        {
            _messageSender.MessageToSend = new Message(phoneNumber, messageText);
            return _messageSender.Execute();
        }

        public HttpStatusCode CheckMessageStatus(string messageHeaderId)
        {
            _messageStatusChecker.MessageHeaderId = messageHeaderId;
            return _messageStatusChecker.Execute();
        }

        public HttpStatusCode CheckInbox()
        {
            return _messageInboxFetcher.Execute();
        }
    }
}
