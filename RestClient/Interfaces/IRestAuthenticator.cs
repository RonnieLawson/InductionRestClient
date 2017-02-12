using System;
using System.Net;
using RestSharp;

namespace InductionRestAPI.Interfaces
{
    public interface IRestAuthenticator
    {
        bool IsAuthenticated { get; }
        HttpStatusCode Authenticate();
        string GetEncodedSession();
    }
}