using System;
using CommonUtils;
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
            private EsendexSoapClient _soapClient;

            [OneTimeSetUp]
            public void WhenCreatingTheObject()
            {
                _soapClient = new EsendexSoapClient("EX0224195", "ronnie.lawson+induction@esendex.com", Utility.GetSecret("password"));
            }

            [Test]
            public void ThenTheRightObjectIsCreated()
            {
                Assert.That(_soapClient.GetType(), Is.EqualTo(typeof(EsendexSoapClient)));
            }
        }

        [TestFixture, Category("Costly")]
        public class GivenAValidSoapMessageSender
        {
            private string _result;

            [OneTimeSetUp]
            public void WhenCallingSendMessage()
            {
                var esendexSoapClient = new EsendexSoapClient("EX0224195", "ronnie.lawson+induction@esendex.com", Utility.GetSecret("password"));
                _result = esendexSoapClient.SendMessage("07590360247","test client message");
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
