using static System.Text.Encoding;

namespace RestClient
{
    public interface ICredentialsManager
    {
        string EncodeTo64(string toEncode);
    }

    public class CredentialsManager : ICredentialsManager
    {

        public string EncodeTo64(string toEncode)
        {
            var toEncodeAsBytes
                  = ASCII.GetBytes(toEncode);
            var returnValue
                  = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }
    }
}