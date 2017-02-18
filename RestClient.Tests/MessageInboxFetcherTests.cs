using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using InductionRestAPI;
using InductionRestAPI.Clients;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;

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
                    Assert.That(_messageInboxFetcher.MessageInboxResponse, Is.Not.Null);
                }

            }

            [TestFixture]
            public class GivenAMessageInboxFetcherWithAResponse
            {
                private MessageInboxResponse _messageInboxResponse;

                [OneTimeSetUp]
                public void WhenDeserializingTheResponse()
                {
                     string content = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                           "<messageheaders startindex=\"0\" count=\"2\" totalcount=\"2\" xmlns=\"http://api.esendex.com/ns/\">" +
                                           "<messageheader id=\"11483b43-072a-418a-b59c-d0261c63fc56\" uri=\"https://api.esendex.com/v1.0/messageheaders/11483b43-072a-418a-b59c-d0261c63fc56\">" +
                                           "<reference>EX0224195</reference><status>Submitted</status><sentat>2017-02-15T08:07:05.183Z</sentat>" +
                                           "<laststatusat>2017-02-15T08:07:05.183Z</laststatusat><submittedat>2017-02-15T08:07:05.183Z</submittedat>" +
                                           "<receivedat>2017-02-15T08:07:05.183Z</receivedat><type>SMS</type>" +
                                           "<to><phonenumber>447422128264</phonenumber></to><from><phonenumber>447590360247</phonenumber></from><summary>Test reply</summary>" +
                                           "<body id=\"11483b43-072a-418a-b59c-d0261c63fc56\" uri=\"https://api.esendex.com/v1.0/messageheaders/11483b43-072a-418a-b59c-d0261c63fc56/body\" />" +
                                           "<direction>Inbound</direction><parts>1</parts><readat>0001-01-01T00:00:00Z</readat></messageheader>" +
                                           "<messageheader id=\"874673b3-3bf5-4506-9cb6-c2022c89d9f7\" uri=\"https://api.esendex.com/v1.0/messageheaders/874673b3-3bf5-4506-9cb6-c2022c89d9f7\">" +
                                           "<reference>EX0224195</reference><status>Submitted</status><sentat>2017-02-13T00:58:15.223Z</sentat>" +
                                           "<laststatusat>2017-02-13T00:58:15.223Z</laststatusat><submittedat>2017-02-13T00:58:15.223Z</submittedat>" +
                                           "<receivedat>2017-02-13T00:58:15.223Z</receivedat><type>SMS</type>" +
                                           "<to><phonenumber>447422128264</phonenumber></to><from><phonenumber>447590360247</phonenumber></from><summary>Aaaaaaahh!</summary>" +
                                           "<body id=\"874673b3-3bf5-4506-9cb6-c2022c89d9f7\" uri=\"https://api.esendex.com/v1.0/messageheaders/874673b3-3bf5-4506-9cb6-c2022c89d9f7/body\" />" +
                                           "<direction>Inbound</direction><parts>1</parts><readat>0001-01-01T00:00:00Z</readat></messageheader>" +
                                           "</messageheaders>";
                    var xml = new XmlDeserializer();
                    var restResponse = new RestResponse { Content = content };
                    _messageInboxResponse = xml.Deserialize<MessageInboxResponse>(restResponse);
                }

                [Test]
                public void ThenTheFirstMessageHasTheCorrectId()
                {
                    Assert.That(_messageInboxResponse.MessageHeaders[0].Id.ToString(), Is.EqualTo("11483b43-072a-418a-b59c-d0261c63fc56"));
                }

                [Test]
                public void ThenTheMessageHeadersListIsPopulatedWithTheCorrectCount()
                {
                    Assert.That(_messageInboxResponse.MessageHeaders.Count, Is.EqualTo(2));
                }

                [Test]
                public void ThenTheMessageHeadersListIsPopulated()
                {
                    Assert.That(_messageInboxResponse.MessageHeaders, Is.Not.Null);
                }

                [Test]
                public void ThenTheMessageInboxResponseTotalCountIsCorrect()
                {
                    Assert.That(_messageInboxResponse.TotalCount, Is.EqualTo(2));
                }

                [Test]
                public void ThenTheMessageInboxResponseCountIsCorrect()
                {
                    Assert.That(_messageInboxResponse.Count, Is.EqualTo(2));
                }

                [Test]
                public void ThenTheMessageInboxResponseIsPopulated()
                {
                    Assert.That(_messageInboxResponse, Is.Not.Null);
                }
            }
        }
    }
}
