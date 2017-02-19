using CommonUtils;
using NUnit.Framework;

namespace RestClient.Tests
{
    [TestFixture]
    public class GivenAString
    {
        [TestCase("","")]
        [TestCase("6d58cde8-fda4-4bf5-8d50-470a5f23afaa", "NmQ1OGNkZTgtZmRhNC00YmY1LThkNTAtNDcwYTVmMjNhZmFh")]
        public void ThenTheStringIsEncoded(string toEncode, string expected)
        {
            var result = Utility.Encode(toEncode);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
