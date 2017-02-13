using System;
using System.Net;
using System.Text;
using InductionRestAPI;
using InductionRestAPI.Models;
using NUnit.Framework;
using RestSharp;
using RestSharp.Deserializers;

namespace RestClient.Tests
{
    [TestFixture]
    public class RestAuthenticatorTests
    {
        [TestFixture]
        public class GivenAnAuthenticator
        {
            private InductionRestAPI.RestAuthenticator _restAuthenticator;

            [OneTimeSetUp]
            public void WhenCreatingTheAuthenticator()
            {
                _restAuthenticator = new InductionRestAPI.RestAuthenticator("Http://nowhere.com", "", "anything", "anything");
            }

        }

        [TestFixture]
        public class GivenValidCredentials
        {
            private InductionRestAPI.RestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;

            [OneTimeSetUp]
            public void WhenCallingGetAuthenticate()
            {
                var password = Utility.GetSecret("password");
                _restAuthenticator = new RestAuthenticator("https://api.esendex.com", "/v1.0/session/constructor", "ronnie.lawson+induction@esendex.com", password);
                _result = _restAuthenticator.Authenticate();
            }

            [Test]
            public void ThenTheServiceReturnsOK()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenIsAuthenticatedIsTrue()
            {
                Assert.That(_restAuthenticator.IsAuthenticated, Is.True);
            }

            [Test]
            public void ThenSessionIDIsStored()
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
            private InductionRestAPI.RestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;

            [OneTimeSetUp]
            public void WhenCallingGetAuthenticate()
            {
                _restAuthenticator = new RestAuthenticator("https://api.esendex.com", "/v1.0/session/constructor", "ronnie.lawson+inductionIntegration@esendex.com", "wrongpassword");
                _result = _restAuthenticator.Authenticate();
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
