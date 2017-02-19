using System;
using RestSharp.Deserializers;

namespace RestClient.Models
{
    [DeserializeAs(Name = "messageheader")]
    public class MessageHeader
    {
        [DeserializeAs(Name = "uri")]
        public string Uri { get; set; }

        public Guid Id { get; set; }

        public string Status { get; set; }
        
        public DateTime LastStatusAt { get; set; }

        public string SubmittedAt { get; set; }

        public string Type { get; set; }

        public PhoneNumber To { get; set; }

        public PhoneNumber From { get; set; }

        public string Summary { get; set; }

        public Body Body { get; set; }

        public string Direction { get; set; }

        public int Parts { get; set; }

        public string Username { get; set; }

    }

    [DeserializeAs(Name = "body")]
    public class Body
    {
        public string Uri { get; set; }
    }

    [DeserializeAs(Name = "phonenumber")]
    public class PhoneNumber
    {
        [DeserializeAs(Name = "phonenumber")]
        public string Value { get; set; }
    }
}