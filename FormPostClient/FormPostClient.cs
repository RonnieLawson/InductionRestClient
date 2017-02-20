using System.IO;
using System.Net;
using System.Text;

namespace FormPostClient
{
    public class FormPostClient
    {
        private string _accountReference;
        private string _password;
        private string _username;
        private string _formPostApiAddress;
        public string SendMessageResponse { get; private set; }

        public FormPostClient(string username, string password, string accountReference, string formPostApiAddress)
        {
            _formPostApiAddress = formPostApiAddress;
            _username = username;
            _password = password;
            _accountReference = accountReference;
        }

        public HttpStatusCode SendMessage(string recipient, string message)
        {
            var request = WebRequest.Create(_formPostApiAddress);
            request.Method = "POST";
            var byteArray = Encoding.UTF8.GetBytes($"username={_username}&password={_password}&account={_accountReference}&recipient={recipient}&body={message}&plaintext=1");
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            using (var dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            HttpStatusCode statusCode;
            using (var response = request.GetResponse())
            {
                var httpWebResponse = (HttpWebResponse)response;
                statusCode = httpWebResponse.StatusCode;

                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        SendMessageResponse = reader.ReadToEnd();
                    }
                }
            }
            return statusCode;
        }
    }
}
