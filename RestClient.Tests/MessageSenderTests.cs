using System.Net;
using InductionRestAPI;
using InductionRestAPI.Interfaces;
using NSubstitute;
using NUnit.Framework;
using RestSharp.Authenticators;

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
                _messageSender = new MessageSender("http://someting.com", "", authenticator);
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
                //var restAuthenticator = new RestAuthenticator("Ronnie.Lawson+Induction@esendex.com", Utility.GetSecret("password"), "https://api.esendex.com", "/v1.0/session/constructor");
                _restAuthenticator = Substitute.For<IRestAuthenticator>();
                _restAuthenticator.IsAuthenticated.Returns(false);

                var messageSender = new MessageSender("http://someting.com", "", _restAuthenticator);
                var numberToSendTo = "07590360247";
                var messageToSend = "Test";
                messageSender.SendMessage(numberToSendTo, messageToSend, "");
            }

            [Test]
            public void ThenAuthenticateIsCalled()
            {
                _restAuthenticator.Received(1).Authenticate();
            }

            [Test]
            public void ThenGetEncodedSessionIsCalled()
            {
                _restAuthenticator.Received(1).GetEncodedSession();
            }


        }

        [TestFixture]
        public class GivenAMessageToSenWithValidCredentialsd
        {
            private IRestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;

            [OneTimeSetUp]
            public void WhenCallingSend()
            {
                var ApiBaseUrl = "https://api.esendex.com";
                _restAuthenticator = new RestAuthenticator(ApiBaseUrl, "/v1.0/session/constructor", "Ronnie.Lawson+Induction@esendex.com", Utility.GetSecret("password"));

                var messageSender = new MessageSender(ApiBaseUrl, "/v1.0/messagedispatcher", _restAuthenticator);
                var numberToSendTo = "07906701458";
                var messageToSend = "I love you";
                _result = messageSender.SendMessage(numberToSendTo, messageToSend, "EX0224195");
            }

            [Test]
            public void ThenItReturnsAccepted()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }
        }
    }
}
