﻿using System;
using System.Net;
using RestSharp;

namespace InductionClient.RestAuthenticator
{
    public interface IRestAuthenticator
    {
        bool IsAuthenticated { get; }
        HttpStatusCode Authenticate();
        Guid ParseSessionFromResponse(IRestResponse response);
        string GetEncodedSession();
    }
}