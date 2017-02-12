using System;
using System.Net;
using RestSharp;

namespace InductionRestAPI.Interfaces
{
    public interface IRestAuthenticator
    {
        bool IsAuthenticated { get; }
        HttpStatusCode Authenticate();
        Guid ParseSessionFromResponse(IRestResponse response);
        string GetEncodedSession();
    }
}