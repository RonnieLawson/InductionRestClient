using System;
using System.Collections.Generic;
using System.Net;
using InductionRestAPI.Clients;
using System.IO;
using System.Configuration;
using InductionRestAPI.Models;

namespace InductionRestAPI
{
    public class RestApiClient
    {
        private readonly MessageSender _messageSender;
        private readonly MessageStatusChecker _messageStatusChecker;
        private readonly MessageInboxFetcher _messageInboxFetcher;
        private object _lastSentHeader;
        private static string _logDir;

        public RestApiClient(IApiBase messageSender, IApiBase messageStatusChecker, IApiBase messageInboxFetcher)
        {
            _logDir = ConfigurationManager.AppSettings["LogDirectory"] ?? @"C:\Logs";



            _messageSender = (MessageSender) messageSender;
            _messageStatusChecker = (MessageStatusChecker)messageStatusChecker;
            _messageInboxFetcher = (MessageInboxFetcher)messageInboxFetcher;
        }

        public HttpStatusCode SendMessage(string phoneNumber, string messageText)
        {
             WriteLine($"Sending Message: {messageText} To: {phoneNumber}");

            _messageSender.MessageToSend = new Message(phoneNumber, messageText);
            var result = _messageSender.Execute();

            if (result == HttpStatusCode.OK)
            {
                _lastSentHeader = _messageSender.MessageSenderHeaders.MessageHeader.Id.ToString();
                WriteLine($"Message Sent! Header:{_lastSentHeader}");
            }
            else
                WriteLine($"Message Send Failed! StatusCode: {result}");
            return result;
        }

        public HttpStatusCode CheckMessageStatus(string messageHeaderId)
        {
            WriteLine($"Checking Message Status: {messageHeaderId}");

            _messageStatusChecker.MessageHeaderId = messageHeaderId;
            var result = _messageStatusChecker.Execute();
            var messageHeader = _messageStatusChecker.MessageHeader;
            if (result == HttpStatusCode.OK)
            {
                WriteLine("Status Retrieved!");
                WriteOutMessage(messageHeader);
            }
            else
            {
                WriteLine($"Message Status Check Failed! StatusCode: {result}");
            }

            return result;
        }

        public HttpStatusCode CheckInbox(int? messageNumber)
        {
            if(messageNumber == null)
            WriteLine($"Checking Inbox for messages");
            var result = _messageInboxFetcher.Execute();
            DisplayMessageList(_messageInboxFetcher.MessageInboxResponse);
            return result;
        }

        private void DisplayMessageList(MessageInboxResponse messageInboxHeaders)
        {
            if (messageInboxHeaders == null)
            {
                WriteLine($"Invalid Message Inbox Response Object");
                return;
            }
            WriteLine($"Inbox contains {messageInboxHeaders.TotalCount} Messages, Displaying {messageInboxHeaders.Count}:");
            var headerNumber = 1;
            foreach (var messageInboxHeader in messageInboxHeaders.MessageHeaders)
            {
                WriteLine("-------------------------");
                   WriteLine($"Message {headerNumber}:");
                WriteOutMessage(messageInboxHeader);
                headerNumber++;
            }
            WriteLine("-------------------------");
        }

        private void WriteOutMessage(MessageHeader messageHeader)
        {
            WriteLine($"Id: {messageHeader.Id}");
            WriteLine($"Status: {messageHeader.Status}");
            WriteLine($"Direction: {messageHeader.Direction}");
            WriteLine($"From: {messageHeader.From.Value}");
            WriteLine($"To: {messageHeader.To.Value}");
            WriteLine($"Summary: {messageHeader.Summary}");
        }


        public void WriteLine(string message)
        {
            string TimestampedMessage = Utility.AddTimestampTo(message);
            Utility.Log(TimestampedMessage);
            Console.WriteLine(TimestampedMessage);
        }





    }
}