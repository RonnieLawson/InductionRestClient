using System;
using NUnit.Framework;
using SOAPClient;

namespace SoapClient.Tests
{
    [TestFixture]
    public class SoapClientTests
    {
        [TestFixture]
        public class GivenASoapMessageSender
        {
            private EsendexSoapClient _soapMessageSender;

            [OneTimeSetUp]
            public void WhenCreatingTheObject()
            {
                _soapMessageSender = new EsendexSoapClient();
            }

            [Test]
            public void ThenTheRightObjectIsCreated()
            {
                Assert.That(_soapMessageSender.GetType(), Is.EqualTo(typeof(EsendexSoapClient)));
            }
        }

        [TestFixture, Category("Costly")]
        public class GivenAValidSoapMessageSender
        {
            private string _result;

            [OneTimeSetUp]
            public void WhenCallingSendMessage()
            {
                var soapMessageSender = new EsendexSoapClient();
                _result = soapMessageSender.SendMessage("test client message");
            }

            [Test]
            public void ThenItReturnsAValidGuid()
            {
                Guid guid;
                var isGuid = Guid.TryParse(_result, out guid);
                Assert.That(isGuid, Is.True);
            }
        }
    }
}
