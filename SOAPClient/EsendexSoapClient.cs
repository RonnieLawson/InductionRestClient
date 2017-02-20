using SOAPClient.EsendexSoapService;

namespace SOAPClient
{
    public class EsendexSoapClient
    {
        private string _accountReference;
        private string _username;
        private string _password;

        public EsendexSoapClient(string accountReference, string username, string password)
        {
            _accountReference = accountReference;
            _username = username;
            _password = password;
        }

        public string SendMessage(string sendTo, string message)
        {
            var client = new SendServiceSoapClient();

            var messengerHeader = new MessengerHeader
            {
                Account = _accountReference,
                Username = _username,
                Password = _password
            };
            
            var result = client.SendMessage(messengerHeader, sendTo, message, MessageType.Text);
            return result;
        }
    }
}
