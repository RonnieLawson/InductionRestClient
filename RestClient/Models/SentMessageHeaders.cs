using System;
using RestSharp.Deserializers;

namespace RestClient.Models
{
    [DeserializeAs(Name = "messageheaders")]
    public class SentMessageHeaders
    {
        public Guid BatchId { get; set; }
        public MessageHeader MessageHeader { get; set; }
    }
}