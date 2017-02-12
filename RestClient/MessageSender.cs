using InductionRestAPI.Interfaces;

namespace InductionRestAPI
{
    public class MessageSender
    {
        private readonly IRestAuthenticator _restAuthenticator;

        public MessageSender(IRestAuthenticator restAuthenticator)
        {
            _restAuthenticator = restAuthenticator;
        }

        public void SendMessage(string numberToSendTo, string messageToSend)
        {
            throw new System.NotImplementedException();
        }
    }
}