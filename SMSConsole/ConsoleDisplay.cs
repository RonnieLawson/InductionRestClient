using System;
using CommonUtils;
using RestClient.Interfaces;

namespace SMSConsole
{
    public class ConsoleDisplay : IDisplay
    {
        public void WriteLine(string textToDisplay)
        {
            var timestampedMessage = Utility.AddTimestampTo(textToDisplay);
            Utility.Log(timestampedMessage);
            Console.WriteLine(timestampedMessage);
        }
    }
}