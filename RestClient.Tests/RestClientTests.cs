using System;
using System.Net;
using InductionRestAPI;
using InductionRestAPI.Clients;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using NUnit.Framework;
using NSubstitute;

namespace RestClient.Tests
{
    [TestFixture]
    public class RestClientTests
    {
        [TestFixture]
        public class GivenARestClient
        {
            private RestApiClient _restClient;

            [OneTimeSetUp]
            public void WhenCreatingTheRestClient()
            {
                var restAuthenticator = new RestAuthenticator("http://test/.com", "", "", "");
                _restClient = new RestApiClient(new MessageSender("", restAuthenticator, ""), new MessageStatusChecker("", restAuthenticator), new MessageInboxFetcher("", restAuthenticator));
            }

            [Test]
            public void ThenTheClientIsCreated()
            {
                Assert.That(_restClient.GetType(), Is.EqualTo(typeof(RestApiClient)));
            }
        }

        [TestFixture]
        public class GivenARestClientWithAMessageSender
        {
            private RestApiClient _client;
            private HttpStatusCode _result;
            private MessageSender _messageSender;

            [OneTimeSetUp]
            public void WhenCallingSendMessage()
            {
                IRestAuthenticator restAuthenticator = Substitute.For<IRestAuthenticator>();
                _messageSender = Substitute.For<MessageSender>("endpoint", restAuthenticator, "reference");
                _messageSender.Execute().Returns(HttpStatusCode.OK);
                var messageSenderHeaders = new SentMessageHeaders() {MessageHeader = new MessageHeader() {Id = Guid.NewGuid()} };
                _messageSender.MessageSenderHeaders = messageSenderHeaders;

                _client = new RestApiClient(_messageSender, new MessageStatusChecker("", restAuthenticator), 
                    new MessageInboxFetcher("", restAuthenticator));
                _result = _client.SendMessage("test client message", "07590360247");
            }

            [Test]
            public void ThenItReturnsOK()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenExecuteIsCalledOnMessageSender()
            {
                _messageSender.Received(1).Execute();
            }
        }

        [TestFixture]
        public class GivenARestClientWithAMessageStatusChecker
        {
            private RestApiClient _client;
            private HttpStatusCode _result;
            private MessageStatusChecker _messageStatusChecker;

            [OneTimeSetUp]
            public void WhenCallingCheckMessageStatus()
            {
                IRestAuthenticator restAuthenticator = Substitute.For<IRestAuthenticator>();
                _messageStatusChecker = Substitute.For<MessageStatusChecker>("endpoint", restAuthenticator);
                _messageStatusChecker.Execute().Returns(HttpStatusCode.OK);
                _messageStatusChecker.MessageHeader = new MessageHeader()
                {
                    Id = Guid.NewGuid(),
                    Status = "TestStatus",
                    Direction = "Test Direction",
                    From = new PhoneNumber() {Value = "01234567980"},
                    To = new PhoneNumber() {Value = "9876543210"},
                    Summary = "Test Summary"
                };



                _client = new RestApiClient(new MessageSender("", restAuthenticator, ""), 
                    _messageStatusChecker, new MessageInboxFetcher("", restAuthenticator));
                _result = _client.CheckMessageStatus(Guid.NewGuid().ToString());
            }

            [Test]
            public void ThenItReturnsOK()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenExecuteIsCalledOnMessageStatusChecker()
            {
                _messageStatusChecker.Received(1).Execute();
            }
        }

        [TestFixture]
        public class GivenARestClientWithAMessageInboxFetcher
        {
            private RestApiClient _client;
            private HttpStatusCode _result;
            private MessageInboxFetcher _messageInboxFetcher;

            [OneTimeSetUp]
            public void WhenCallingCheckInbox()
            {
                IRestAuthenticator restAuthenticator = Substitute.For<IRestAuthenticator>();
                _messageInboxFetcher = Substitute.For<MessageInboxFetcher>("endpoint", restAuthenticator);
                _messageInboxFetcher.Execute().Returns(HttpStatusCode.OK);

                _client = new RestApiClient(new MessageSender("", restAuthenticator, ""),
                    new MessageStatusChecker("", restAuthenticator), _messageInboxFetcher);
                _result = _client.CheckInbox(null);
            }

            [Test]
            public void ThenItReturnsOK()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenExecuteIsCalledOnMessageInboxFetcher()
            {
                _messageInboxFetcher.Received(1).Execute();
            }
        }
    }
}
