using System.Xml.Serialization;

namespace InductionRestAPI
{
    [XmlRoot("message")]
    public class Message
    {
        public Message(string numberToSendTo, string messageToSend)
        {
            To = numberToSendTo;
            Body = messageToSend;
        }

        [XmlElement("to")]
        public string To { get; set; }
        [XmlElement("body")]
        public string Body { get; set; }

        public Message()
        { }
    }
}