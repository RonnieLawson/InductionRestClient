using System;
using System.Net;
using System.Text;
using InductionRestAPI;
using InductionRestAPI.Clients;
using NUnit.Framework;

namespace RestClient.Tests
{
    [TestFixture]
    public class RestAuthenticatorTests
    {

        [TestFixture]
        public class GivenValidCredentials
        {
            private RestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;

            [OneTimeSetUp]
            public void WhenCallingGetAuthenticate()
            {
                var password = Utility.GetSecret("password");
                _restAuthenticator = new RestAuthenticator("https://api.esendex.com", "/v1.0/session/constructor", "ronnie.lawson+induction@esendex.com", password);
                _result = _restAuthenticator.Execute();
            }

            [Test]
            public void ThenTheServiceReturnsOk()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenIsAuthenticatedIsTrue()
            {
                Assert.That(_restAuthenticator.IsAuthenticated, Is.True);
            }

            [Test]
            public void ThenSessionIdIsStored()
            {
                Assert.That(_restAuthenticator.SessionId, Is.Not.Null);
            }

            [Test]
            public void ThenEncodedSessionIsCorrect()
            {
                Assert.That(_restAuthenticator.GetEncodedSession(), Is.EqualTo(Convert.ToBase64String(Encoding.ASCII.GetBytes(_restAuthenticator.SessionId.ToString()))));
            }
        }

        [TestFixture]
        public class GivenInvalidCredentials
        {
            private RestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;

            [OneTimeSetUp]
            public void WhenCallingGetAuthenticate()
            {
                _restAuthenticator = new RestAuthenticator("https://api.esendex.com", "/v1.0/session/constructor", "ronnie.lawson+inductionIntegration@esendex.com", "wrongpassword");
                _result = _restAuthenticator.Execute();
            }

            [Test]
            public void ThenTheServiceReturnsForbidden()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.Forbidden));
            }

            [Test]
            public void ThenIsAuthenticatedIsTrue()
            {
                Assert.That(_restAuthenticator.IsAuthenticated, Is.False);
            }

            [Test]
            public void ThenSessionIdIsStored()
            {
                Assert.That(_restAuthenticator.SessionId, Is.Null);
            }

            [Test]
            public void ThenGetEncodedSessionReturnsNull()
            {
                Assert.That(_restAuthenticator.GetEncodedSession(), Is.Null);
            }
        }
    }
}
