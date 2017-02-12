using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace InductionClient.RestAuthenticator.Tests
{
    [TestFixture]
    public class RestAuthenticatorTests
    {
        [TestFixture]
        public class GivenAnAuthenticator
        {
            private RestAuthenticator _restAuthenticator;

            [OneTimeSetUp]
            public void WhenCreatingTheAuthenticator()
            {
                _restAuthenticator = new RestAuthenticator("anything","anything", "Http://nowhere.com", "");
            }

            [Test]
            public void ThenTheSessionGuidIsNull()
            {
                Assert.That(_restAuthenticator.SessionId, Is.Null);
            }

            [Test]
            public void ThenIsAuthenticatedIsFalse()
            {
                Assert.That(_restAuthenticator.IsAuthenticated, Is.False);
            }
        }

        [TestFixture]
        public class GivenValidCredentials
        {
            private RestAuthenticator _restAuthenticator;

            [OneTimeSetUp]
            public void WhenCallingGetSessionID()
            {
                _restAuthenticator = new RestAuthenticator("ronnie.lawson+inductionIntegration@esendex.com", "V0lDbllZ8YRb", "https://api.esendex.com", "/v1.0/session/constructor");
                var result = _restAuthenticator.Authenticate();
            }

            [Test]
            public void ThenTheServiceReturnsOK()
            {
                //Assert.That();
            }
        }
    }
}
