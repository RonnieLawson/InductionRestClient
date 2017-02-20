using CommonUtils;
using NUnit.Framework;

namespace FormPostClient.Tests
{
    [TestFixture]
    public class FormPostClientTests
    {
        [TestFixture]
        public class GivenAFormPostClient
        {
            private FormPostClient _formPostClient;

            [OneTimeSetUp]
            public void WhenCreating()
            {
                _formPostClient = new FormPostClient("ronnie.lawson+induction@esendex.com", Utility.GetSecret("password"), 
                    "EX0224195", "https://www.esendex.com/secure/messenger/formpost/SendSMS.aspx");
            }

            [Test]
            public void ThenTheCorrectObjectIsCreated()
            {
                Assert.That(_formPostClient.GetType(), Is.EqualTo(typeof(FormPostClient)));
            }
        }

        [TestFixture]
        public class GivenAFormPostClientWithValidCredentials
        {
            private object _result;
            private FormPostClient _formPostClient;

            [OneTimeSetUp]
            public void WhenCallingSendMessage()
            {
                _formPostClient = new FormPostClient("ronnie.lawson%2Binductionecho@esendex.com", Utility.GetSecret("password"), 
                    "EX0224195", "https://www.esendex.com/secure/messenger/formpost/SendSMS.aspx");

                _result = _formPostClient.SendMessage("07590360247", "Form Post Test Message");
            }

            [Test]
            public void ThenAResponseIsReturned()
            {
                Assert.That(_result, Is.Not.Null);
            }
            [Test]
            public void ThenTheResponseIsNotError()
            {
                Assert.That(_formPostClient.SendMessageResponse, Is.Not.EqualTo("Result=Error\r\nMessage=Authentication+Failed"));
            }
            [Test]
            public void ThenTheResponseContainsMessageIDs()
            {
                Assert.That(_formPostClient.SendMessageResponse.Contains("MessageIDs="), Is.EqualTo(true));
            }
        }
    }
}
