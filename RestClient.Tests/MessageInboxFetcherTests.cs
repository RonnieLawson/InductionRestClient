using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using InductionRestAPI;
using InductionRestAPI.Interfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace RestClient.Tests
{
    [TestFixture]
    public class MessageInboxFetcherTests
    {
        [TestFixture]
        public class GivenAMessageInbopxFetcher
        {
            private MessageInboxFetcher _fetcher;

            [OneTimeSetUp]
            public void WhenCreatingTheFetcher()
            {
                var restAuthenticator = new RestAuthenticator("http://test.com", "", "", "");
                _fetcher = new MessageInboxFetcher("resource", restAuthenticator);
            }

            [Test]
            public void ThenTheCorrectObjectIsCreated()
            {
                Assert.That(_fetcher.GetType(), Is.EqualTo(typeof(MessageInboxFetcher)));

            }
        }

        [TestFixture]
        public class GivenAMessageInboxFetcherWithoutASession
        {
            private IRestAuthenticator _restAuthenticator;

            [OneTimeSetUp]
            public void WhenCallingExecute()
            {
                _restAuthenticator = Substitute.For<IRestAuthenticator>();
                _restAuthenticator.IsAuthenticated.Returns(false);

                var fetcher = new MessageInboxFetcher("", _restAuthenticator);

                fetcher.Execute();
            }

            [Test]
            public void ThenAuthenticateIsCalled()
            {
                _restAuthenticator.Received(1).Execute();
            }

            [Test]
            public void ThenGetEncodedSessionIsCalled()
            {
                _restAuthenticator.Received(1).GetEncodedSession();
            }


            [TestFixture]
            public class GivenAMessageInboxFetcherWIthValidCredentials
            {
                private IRestAuthenticator _restAuthenticator;
                private HttpStatusCode _result;
                private MessageInboxFetcher _messageInboxFetcher;

                [OneTimeSetUp]
                public void WhenCallingExecute()
                {
                    var ApiBaseUrl = "https://api.esendex.com";
                    _restAuthenticator = new RestAuthenticator(ApiBaseUrl, "/v1.0/session/constructor",
                        "Ronnie.Lawson+Induction@esendex.com", Utility.GetSecret("password"));

                    _messageInboxFetcher = new MessageInboxFetcher("/v1.0/inbox/messages ", _restAuthenticator);

                    _result = _messageInboxFetcher.Execute();
                }

                [Test]
                public void ThenItReturnsAccepted()
                {
                    Assert.That(_result, Is.EqualTo(HttpStatusCode.OK));
                }

                [Test]
                public void ThenTheResponseIsPopulated()
                {
                    Assert.That(_messageInboxFetcher.MessageInboxHeaders, Is.Not.Null);
                }
            }
        }
    }
}
