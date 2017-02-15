using System.Net;
using InductionRestAPI;
using InductionRestAPI.Clients;
using InductionRestAPI.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace RestClient.Tests
{
    [TestFixture]
    public class MessageSenderTests
    {
        [TestFixture]
        public class GivenAMessageSender
        {
            private MessageSender _messageSender;

            [OneTimeSetUp]
            public void WhenCreating()
            {
                var authenticator = Substitute.For<IRestAuthenticator>();
                _messageSender = new MessageSender("", authenticator, "");
            }

            [Test]
            public void ThenTheCorrectObjectIsCreated()
            {
                Assert.That(_messageSender.GetType(), Is.EqualTo(typeof(MessageSender)));
            }
        }

        [TestFixture]
        public class GivenAMessageToSend
        {
            private IRestAuthenticator _restAuthenticator;

            [OneTimeSetUp]
            public void WhenCallingSend()
            {
                _restAuthenticator = Substitute.For<IRestAuthenticator>();
                _restAuthenticator.IsAuthenticated.Returns(false);

                var messageSender = new MessageSender("", _restAuthenticator, "")
                {
                    MessageToSend = new Message("07590360247", "Test")
                };
                messageSender.Execute();
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

        [TestFixture, Category("Costly"), Ignore("Costly")]
        public class GivenAMessageToSenWithValidCredentialsd
        {
            private IRestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;
            private MessageSender _messageSender;

            [OneTimeSetUp]
            public void WhenCallingSend()
            {
                var ApiBaseUrl = "https://api.esendex.com";
                _restAuthenticator = new RestAuthenticator(ApiBaseUrl, "/v1.0/session/constructor", "Ronnie.Lawson+Induction@esendex.com", Utility.GetSecret("password"));

                _messageSender = new MessageSender("/v1.0/messagedispatcher", _restAuthenticator, "EX0224195")
                {
                    MessageToSend = new Message("07590360247", "Test Message"),
                };
                _result = _messageSender.Execute();
            }

            [Test]
            public void ThenItReturnsAccepted()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenTheResponseIsPopulated()
            {
                Assert.That(_messageSender.MessageSenderHeaders, Is.Not.Null);
            }

            [Test]
            public void ThenTheBatchIdIsPopulated()
            {
                Assert.That(_messageSender.MessageSenderHeaders.BatchId, Is.Not.Null);
            }

            [Test]
            public void ThenTheMEssageHeaderIsPopulated()
            {
                Assert.That(_messageSender.MessageSenderHeaders.MessageHeader, Is.Not.Null);
            }
        }
    }
}
