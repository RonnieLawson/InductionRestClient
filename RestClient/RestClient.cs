using System;
using System.Net;
using CommonUtils;
using RestClient.Clients;
using RestClient.Interfaces;
using RestClient.Models;

namespace RestClient
{
    public class Client
    {
        public IDisplay _display; 
        private readonly MessageSender _messageSender;
        private readonly MessageStatusChecker _messageStatusChecker;
        private readonly MessageInboxFetcher _messageInboxFetcher;
        private string _lastSentHeader;

        public Client(IApiBase messageSender, IApiBase messageStatusChecker, IApiBase messageInboxFetcher, IDisplay display)
        {
            _display = display;
            _messageSender = (MessageSender) messageSender;
            _messageStatusChecker = (MessageStatusChecker)messageStatusChecker;
            _messageInboxFetcher = (MessageInboxFetcher)messageInboxFetcher;
        }

        public HttpStatusCode SendMessage(string phoneNumber, string messageText)
        {
            _display.WriteLine($"Sending Message: {messageText} To: {phoneNumber}");

            _messageSender.MessageToSend = new Message(phoneNumber, messageText);
            var result = _messageSender.Execute();

            if (result == HttpStatusCode.OK)
            {
                _lastSentHeader = _messageSender.MessageSenderHeaders.MessageHeader.Id.ToString();
                _display.WriteLine($"Message Sent! Header:{_lastSentHeader}");
            }
            else
                _display.WriteLine($"Message Send Failed! StatusCode: {result}");
            return result;
        }

        public HttpStatusCode CheckMessageStatus(string messageHeaderId)
        {
            if (messageHeaderId == "last")
                messageHeaderId = _lastSentHeader;
            _display.WriteLine($"Checking Message Status: {messageHeaderId}");

            _messageStatusChecker.MessageHeaderId = messageHeaderId;
            var result = _messageStatusChecker.Execute();
            var messageHeader = _messageStatusChecker.MessageHeader;
            if (result == HttpStatusCode.OK)
            {
                _display.WriteLine("Status Retrieved!");
                WriteOutMessage(messageHeader);
            }
            else
            {
                _display.WriteLine($"Message Status Check Failed! StatusCode: {result}");
            }

            return result;
        }

        public HttpStatusCode CheckInbox()
        {
            _display.WriteLine("Checking Inbox for messages");
            var result = _messageInboxFetcher.Execute();
            DisplayMessageList(_messageInboxFetcher.MessageInboxResponse);
            return result;
        }

        private void DisplayMessageList(MessageInboxResponse messageInboxHeaders)
        {
            if (messageInboxHeaders == null)
            {
                _display.WriteLine($"Invalid Message Inbox Response Object");
                return;
            }
            _display.WriteLine($"Inbox contains {messageInboxHeaders.TotalCount} Messages, Displaying {messageInboxHeaders.Count}:");
            var headerNumber = 1;
            foreach (var messageInboxHeader in messageInboxHeaders.MessageHeaders)
            {
                _display.WriteLine("-------------------------");
                _display.WriteLine($"Message {headerNumber}:");
                WriteOutMessage(messageInboxHeader);
                headerNumber++;
            }
            _display.WriteLine("-------------------------");
        }

        private void WriteOutMessage(MessageHeader messageHeader)
        {
            _display.WriteLine($"Id: {messageHeader.Id}");
            _display.WriteLine($"Status: {messageHeader.Status}");
            _display.WriteLine($"Direction: {messageHeader.Direction}");
            _display.WriteLine($"From: {messageHeader.From.Value}");
            _display.WriteLine($"To: {messageHeader.To.Value}");
            _display.WriteLine($"Summary: {messageHeader.Summary}");
        }
    }
}