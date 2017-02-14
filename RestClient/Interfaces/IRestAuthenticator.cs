using System;
using System.Net;
using RestSharp;

namespace InductionRestAPI.Interfaces
{
    public interface IRestAuthenticator
    {
        bool IsAuthenticated { get; }
        HttpStatusCode Execute();
        string GetEncodedSession();
    }
}