﻿using System.Net;
using InductionRestAPI.Interfaces;
using InductionRestAPI.Models;
using RestSharp;

namespace InductionRestAPI
{
    public class MessageStatusChecker: ApiBase
    {

        public MessageHeader MessageHeader { get; private set; }

        public bool MessageStatusResponse { get; set; }

        public MessageStatusChecker(string requestResource, IRestAuthenticator authenticator)
        {
            RequestResource = requestResource;
            Authenticator = authenticator;
        }

        public HttpStatusCode Execute()
        {
            var request = SetupRequest(Method.GET, RequestResource);

            var response = RestClient.Execute<MessageHeader>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                MessageHeader = response.Data;
            return response.StatusCode;
        }
    }
}