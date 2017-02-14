using System;
using RestSharp.Deserializers;

namespace InductionRestAPI.Models
{
    [DeserializeAs(Name = "messageheaders")]
    public class MessageHeaders
    {
        public Guid BatchId { get; set; }
        public MessageHeader MessageHeader { get; set; }

    }
}