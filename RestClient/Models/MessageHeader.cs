using System;
using RestSharp.Deserializers;

namespace InductionRestAPI.Models
{
    [DeserializeAs(Name = "messageheader")]
    public class MessageHeader
    {
        public string Uri { get; set; }
        public Guid Id { get; set; }
    }
}