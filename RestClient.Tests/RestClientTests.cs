using System;
using System.Configuration;
using System.Net;
using InductionClient.RestAuthenticator;
using NSubstitute;
using NUnit.Framework;
using InductionRestClient;

namespace RestClient.Tests
{
    [TestFixture]
    public class RestClientTests
    {
        [TestFixture]
        public class GivenARestClient
        {
            private InductionRestClient.InductionClient _restClient;

            [OneTimeSetUp]
            public void WhenCreatingTheRestClient()
            {

                _restClient = new InductionRestClient.InductionClient(new RestAuthenticator("","", "http://test/.com", ""));
            }

            [Test]
            public void ThenTheClientIsCreated()
            {
                Assert.That(_restClient.GetType(), Is.EqualTo(typeof(InductionRestClient.InductionClient)));
            }
        }

      /*  [TestFixture]
        public class GivenARestClientWithoutASession
        {
            private InductionRestClient.InductionClient _client;
            private HttpStatusCode _response;

            [OneTimeSetUp]
            public void WhenCheckingCredentials()
            {
                var restAuthenticator = Substitute.For<IRestAuthenticator>();
                _client = new InductionRestClient.InductionClient(restAuthenticator);
                _response = _client.Authenticate();
            }

            [Test]
            public void ThenANewSessionIsCreated()
            {
                Assert.That(_response, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenTheSessionIdIsStored()
            {
                Assert.That(_client.SessionId, Is.Not.EqualTo(Guid.Empty));
            }
        }*/
    }
}
