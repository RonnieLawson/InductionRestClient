using System.Collections.Generic;
using RestSharp.Deserializers;

namespace InductionRestAPI.Models
{
    [DeserializeAs(Name = "messageheaders")]
    public class MessageInboxResponse
    {
        public int Startindex { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }

        public List<MessageHeader> MessageHeaders { get; set; }

    }
}