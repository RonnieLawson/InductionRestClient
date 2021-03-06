﻿using System;
using System.Net;
using System.Text;
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

        }

        [TestFixture]
        public class GivenValidCredentials
        {
            private RestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;

            [OneTimeSetUp]
            public void WhenCallingGetAuthenticate()
            {
                _restAuthenticator = new RestAuthenticator("ronnie.lawson+inductionIntegration@esendex.com", "V0lDbllZ8YRb", "https://api.esendex.com", "/v1.0/session/constructor");
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
            private RestAuthenticator _restAuthenticator;
            private HttpStatusCode _result;

            [OneTimeSetUp]
            public void WhenCallingGetAuthenticate()
            {
                _restAuthenticator = new RestAuthenticator("ronnie.lawson+inductionIntegration@esendex.com", "wrongpassword", "https://api.esendex.com", "/v1.0/session/constructor");
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
            public void ThenSessionIDIsStored()
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
