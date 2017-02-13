using System.Xml.Serialization;

namespace InductionRestAPI
{
    [XmlRoot("messages")]
    public class SendMessageBody
    {
        public SendMessageBody(string accountReference, string numberToSendTo, string messageToSend)
        {
            Accountreference = accountReference;
            Message = new Message(numberToSendTo, messageToSend);
        }

        [XmlElement("accountreference")]
        public string Accountreference { get; set; }
        [XmlElement("message")]
        public Message Message { get; set; }

        public SendMessageBody()
        { }
    }
}