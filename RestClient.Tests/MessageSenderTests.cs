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
                _messageSender = new MessageSender(authenticator);
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

            [OneTimeSetUp]
            public void WhenCallingSend()
            {
                var restAuthenticator = new RestAuthenticator("Ronnie.Lawson+Induction@esendex.com", Utility.GetSecret("password"), "https://api.esendex.com", "/v1.0/session/constructor");
                var messageSender = new MessageSender(restAuthenticator);
                var numberToSendTo = "07590360247";
                var messageToSend = "Test";
                messageSender.SendMessage(numberToSendTo, messageToSend);

            }

            [Test]
            public void Then()
            {
                
            }
        }
    }
}
