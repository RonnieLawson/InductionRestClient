using System;
using System.Configuration;
using CommonUtils;
using RestClient;
using RestClient.Clients;
using RestClient.Interfaces;

namespace SMSConsole
{
    internal class Program
    {
        private static Client _restClient;
        private static IDisplay _display = new ConsoleDisplay();

        static void Main(string[] args)
        {
            var accountRef = ConfigurationManager.AppSettings["AccountRef"];
            var restUsername = ConfigurationManager.AppSettings["RestUsername"];
            var secretDir = ConfigurationManager.AppSettings["SecretDirectory"];

            var baseRestApi = ConfigurationManager.AppSettings["BaseRestAPI"];
            var authenticationEndpoint = ConfigurationManager.AppSettings["AuthenticationEndpoint"];
            var messageSendEndpoint = ConfigurationManager.AppSettings["MessageSendEndpoint"];
            var messageStatusEndpoint = ConfigurationManager.AppSettings["MessageStatusEndpoint"];
            var messageInboxEndpoint = ConfigurationManager.AppSettings["MessageInboxEndpoint"];

            var password = Utility.GetSecret("password", secretDir);

            var authenticator = new RestAuthenticator(baseRestApi, authenticationEndpoint, restUsername, password);

            var messageSender = new MessageSender(messageSendEndpoint, authenticator, accountRef);
            var messageStatusChecker = new MessageStatusChecker(messageStatusEndpoint, authenticator);
            var messageInboxFetcher = new MessageInboxFetcher(messageInboxEndpoint, authenticator);

            _restClient = new Client(messageSender, messageStatusChecker, messageInboxFetcher, _display);

            var command = "";
            _display.WriteLine("Started SMS Console!");
            ShowHelpText();
            while (command != "exit")
            {
                command = Console.ReadLine();
                Utility.Log(Utility.AddTimestampTo($"Command: {command}"));

                if (string.IsNullOrEmpty(command))
                    continue;

                try
                {
                    var commands = command.Split(' ');
                    switch (commands[0].ToLower())
                    {
                        case "send":
                        {
                            _restClient.SendMessage(commands[1], BuildMessage(commands));
                            break;
                        }
                        case "check":
                        {
                            _restClient.CheckMessageStatus(commands[1].ToLower());
                            break;
                        }
                        case "inbox":
                        {
                            _restClient.CheckInbox();
                            break;
                        }
                        default:
                        {
                            ShowHelpText();
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _display.WriteLine(ex.Message);
                    _display.WriteLine(ex.StackTrace);
                }
            }
            _display.WriteLine("Exiting");
        }

        private static void ShowHelpText()
        {
            _display.WriteLine("Usage: (replace bracketed words with your values)");
            _display.WriteLine("Send message: 'Send [phonenumber] [message]'");
            _display.WriteLine(
                "Check message: \'Check [messageID]\' or \'Check Last\' to see details of the last sent message");
            _display.WriteLine("Check Inbox: \'Inbox\' to get a list of messages");
            //" or \'Inbox [messagenumber]\' to see a specific message ");
        }

        private static string BuildMessage(string[] commands)
        {
            var message = "";
            for (var i = 2; i < commands.Length; i++)
            {
                message = message + commands[i];
                if (i < commands.Length - 1)
                {
                    message = message + " ";
                }
            }
            return message;
        }
    }
}
