using System;
using InductionRestAPI;
using NUnit.Framework;

namespace RestClient.Tests
{
    [TestFixture]
    public class GivenAString
    {
        [TestCase("","")]
        [TestCase("6d58cde8-fda4-4bf5-8d50-470a5f23afaa", "NmQ1OGNkZTgtZmRhNC00YmY1LThkNTAtNDcwYTVmMjNhZmFh")]
        [TestCase("20108e1e-e519-4078-9046-b4f6c0c175a6", "MjAxMDhlMWUtZTUxOS00MDc4LTkwNDYtYjRmNmMwYzE3NWE2")]
        public void ThenTheStringIsEncoded(string toEncode, string expected)
        {
            var result = Utility.Encode(toEncode);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}
