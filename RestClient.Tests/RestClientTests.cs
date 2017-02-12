using System;
using System.Net;
using InductionRestAPI;
using NUnit.Framework;

namespace RestClient.Tests
{
    [TestFixture]
    public class RestClientTests
    {
        [TestFixture]
        public class GivenARestClient
        {
            private RestAPIClient _restClient;

            [OneTimeSetUp]
            public void WhenCreatingTheRestClient()
            {

                _restClient = new RestAPIClient(new RestAuthenticator("","", "http://test/.com", ""));
            }

            [Test]
            public void ThenTheClientIsCreated()
            {
                Assert.That(_restClient.GetType(), Is.EqualTo(typeof(RestAPIClient)));
            }
        }

       [TestFixture, Ignore("setting up")]
        public class GivenARestClientWithoutASession
        {
            private RestAPIClient _client;
            private HttpStatusCode _response;

            [OneTimeSetUp]
            public void WhenCheckingCredentials()
            {
                //_client = new RestAPIClient(restAuthenticator);
                
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
        }
    }
}
