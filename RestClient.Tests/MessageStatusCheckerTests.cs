using System.Net;
using NSubstitute;
using NUnit.Framework;
using System;
using CommonUtils;
using RestClient.Clients;
using RestClient.Interfaces;
using RestClient.Models;

namespace RestClient.Tests
{
    [TestFixture]
    internal class MessageStatusCheckerTests
    {
        [TestFixture]
        public class GivenAMessageStatusChecker
        {
            private MessageStatusChecker _messageStatusChecker;
            private IRestAuthenticator _restAuthenticator;

            [OneTimeSetUp]
            public void WhenCreatingTheObserver()
            {
                _restAuthenticator = Substitute.For<IRestAuthenticator>();
                _messageStatusChecker = new MessageStatusChecker("/v1.0/messageheaders", _restAuthenticator);
            }

            [Test]
            public void ThenTheCorrectTypeIsCreated()
            {
                Assert.That(_messageStatusChecker.GetType(), Is.EqualTo(typeof(MessageStatusChecker)));
            }
        }

        [TestFixture]
        public class GivenAMessageStatusCheckerWithoutAuthorisation
        {
            private MessageStatusChecker _messageStatusChecker;
            private IRestAuthenticator _restAuthenticator;

            [OneTimeSetUp]
            public void WhenCallingGetStatus()
            {
                _restAuthenticator = Substitute.For<IRestAuthenticator>();
                _restAuthenticator.IsAuthenticated.Returns(false);
                _messageStatusChecker = new MessageStatusChecker("/v1.0/messageheaders/", _restAuthenticator)
                {
                    MessageHeaderId = Guid.NewGuid().ToString()
                };
                _messageStatusChecker.Execute();
            }

            [Test]
            public void ThenAuthenticateIsCalled()
            {
                _restAuthenticator.Received(1).Execute();
            }

            [Test]
            public void ThenGetEncodedSessionIsCalled()
            {
                _restAuthenticator.Received(1).GetEncodedSession();
            }
        }

        [TestFixture, Category("Costly")]
        public class GivenAMessageStatusCheckerWithValidCredentials
        {
            private MessageStatusChecker _messageStatusChecker;
            private IRestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;
            private MessageSender _messageSender;

            [OneTimeSetUp]
            public void WhenCallingGetStatus()
            {
                const string apiBaseUrl = "https://api.esendex.com";
                _restAuthenticator = new RestAuthenticator(apiBaseUrl, "/v1.0/session/constructor",
                    "Ronnie.Lawson+Induction@esendex.com", Utility.GetSecret("password"));

                _messageSender = new MessageSender("/v1.0/messagedispatcher", _restAuthenticator, "EX0224195")
                {
                    MessageToSend = new Message("07590360247", "Get Status Test Message"),
                };
                
                _messageSender.Execute();

                var messageHeaderId = _messageSender.MessageSenderHeaders.MessageHeader.Id.ToString();
                _messageStatusChecker = new MessageStatusChecker($"/v1.0/messageheaders/", _restAuthenticator)
                {
                    MessageHeaderId = messageHeaderId
                };
                _result = _messageStatusChecker.Execute();
            }

            [Test]
            public void ThenTheResponseStatusIsOk()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenItReturnsAMessageHeader()
            {
                Assert.That(_messageStatusChecker.MessageHeader, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidId()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Id, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidUri()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Uri, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidStatus()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Status, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidLastStatusAt()
            {
                Assert.That(_messageStatusChecker.MessageHeader.LastStatusAt, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidSubmittedAt()
            {
                Assert.That(_messageStatusChecker.MessageHeader.SubmittedAt, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidType()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Type, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidToPhoneNumber()
            {
                Assert.That(_messageStatusChecker.MessageHeader.To, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidToPhoneNumberValue()
            {
                Assert.That(_messageStatusChecker.MessageHeader.To.Value, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidFromPhoneNumber()
            {
                Assert.That(_messageStatusChecker.MessageHeader.From, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidFromPhoneNumberValue()
            {
                Assert.That(_messageStatusChecker.MessageHeader.From.Value, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidSummary()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Summary, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidBody()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Body, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidDirection()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Direction, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidParts()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Parts, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidUsername()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Username, Is.Not.Null);
            }

            [Test]
            public void ThenItReturnsAValidBodyUri()
            {
                Assert.That(_messageStatusChecker.MessageHeader.Body.Uri, Is.Not.Null);
            }
        }
    }
}
