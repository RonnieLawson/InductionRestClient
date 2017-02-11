using System;
using System.Net;
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
            private InductionClient _restClient;

            [OneTimeSetUp]
            public void WhenCreatingTheRestClient()
            {
                //var calculator = Substitute.For<ICalculator>();
                var credentialdManager = Substitute.For<ICredentialsManager>();
                _restClient = new InductionClient(credentialdManager);
            }

            [Test]
            public void ThenTheClientIsCreated()
            {
                Assert.That(_restClient.GetType(), Is.EqualTo(typeof(InductionClient)));
            }
        }



        [TestFixture]
        public class GivenARestClientWithoutASession
        {
            private InductionClient _client;
            private HttpStatusCode _response;

            [OneTimeSetUp]
            public void WhenCheckingCredentials()
            {
                
                var credentialsManager = new CredentialsManager();
                
                _client = new InductionClient(credentialsManager);
                _response = _client.Authenticate();
            }

            [Test]
            public void ThenANewSessionIsCreated()
            {
                Assert.That(_response, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenTheSessionIDIsStored()
            {
                Assert.That(_client.SessionId, Is.Not.EqualTo(Guid.Empty));
            }
        }
    }
}
