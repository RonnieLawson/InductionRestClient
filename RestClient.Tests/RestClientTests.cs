using System;
using System.Net;
using InductionRestAPI;
using InductionRestAPI.Clients;
using InductionRestAPI.Interfaces;
using NUnit.Framework;
using NSubstitute;

namespace RestClient.Tests
{
    [TestFixture]
    public class RestClientTests
    {
        [TestFixture]
        public class GivenARestClient
        {
            private RestApiClient _restClient;

            [OneTimeSetUp]
            public void WhenCreatingTheRestClient()
            {
                var restAuthenticator = new RestAuthenticator("http://test/.com", "", "", "");
                _restClient = new RestApiClient(new MessageSender("", restAuthenticator, ""));
            }

            [Test]
            public void ThenTheClientIsCreated()
            {
                Assert.That(_restClient.GetType(), Is.EqualTo(typeof(RestApiClient)));
            }
        }

        [TestFixture]
        public class GivenARestClientWithAMessageSender
        {
            private RestApiClient _client;
            private HttpStatusCode _result;
            private MessageSender _messageSender;

            [OneTimeSetUp]
            public void WhenCheckingCredentials()
            {
                _messageSender = Substitute.For<MessageSender>("endpoint", Substitute.For<IRestAuthenticator>(), "reference");
                _messageSender.Execute().Returns(HttpStatusCode.OK);

                _client = new RestApiClient(_messageSender);
                _result = _client.SendMessage("test client message", "07590360247");
            }

            [Test]
            public void ThenANewSessionIsCreated()
            {
                Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
            }

            [Test]
            public void ThenExecuteIsCalledOnMessageSender()
            {
                _messageSender.Received(1).Execute();
            }


        }
    }
}
