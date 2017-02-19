using CommonUtils;
using SOAPClient.EsendexSoapService;

namespace SOAPClient
{
    public class EsendexSoapClient
    {
        public string SendMessage(string message)
        {
            var client = new SendServiceSoapClient();
            var messengerHeader = new MessengerHeader
            {
                Account = "EX0224195",
                Username = "ronnie.lawson+induction@esendex.com",
                Password = Utility.GetSecret("password")
            };
            
            var result = client.SendMessage(messengerHeader, "07590360247", message, MessageType.Text);
            return result;
        }
    }
}
