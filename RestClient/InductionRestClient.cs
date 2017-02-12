using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using InductionClient.RestAuthenticator;
using RestClient;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;

namespace InductionRestClient
{
    public class InductionClient
    {
        private readonly IRestAuthenticator _restAuthenticator;

        public Guid SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        private Guid _sessionId;

        public InductionClient(IRestAuthenticator restAuthenticator)
        {
            _restAuthenticator = restAuthenticator;
        }

        public HttpWebResponse SendMessage()
        {
            throw new NotImplementedException();
        }

       
    }
}
