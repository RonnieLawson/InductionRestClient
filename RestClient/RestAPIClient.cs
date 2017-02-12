using System;
using System.Net;
using InductionRestAPI.Interfaces;

namespace InductionRestAPI
{
    public class RestAPIClient
    {
        private readonly IRestAuthenticator _restAuthenticator;

        public Guid SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        private Guid _sessionId;

        public RestAPIClient(IRestAuthenticator restAuthenticator)
        {
            _restAuthenticator = restAuthenticator;
        }

        public HttpWebResponse SendMessage()
        {
            throw new NotImplementedException();
        }
    }
}
