using InductionRestAPI;
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
                _messageSender = new MessageSender();
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
                MessageSender message = new MessageSender();
                    
            }

            [Test]
            public void Then()
            {
                
            }
        }
    }
}
