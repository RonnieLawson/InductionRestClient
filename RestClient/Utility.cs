using System;
using System.Text;

namespace InductionRestAPI
{
    public static class Utility
    {
        public static string Encode(string toEncode)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(toEncode));
        }
    }
}