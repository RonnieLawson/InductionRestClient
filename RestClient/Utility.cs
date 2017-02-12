using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace InductionRestAPI
{
    public static class Utility
    {
        public static string Encode(string toEncode)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncode));
        }

        public static string GetSecret(string secret, string secretDirectory = @"C:\secrets\")
        {
            var xml = XDocument.Load(secretDirectory + @"default.secret");
            return xml.Root.Descendants("password").First().Value;
        }
    }
}