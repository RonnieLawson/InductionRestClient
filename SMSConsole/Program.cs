using InductionRestAPI;
using InductionRestAPI.Clients;
using System;
using System.Configuration;
using static System.Int32;

namespace SMSConsole
{
    internal class Program
    {
        private static RestApiClient _restClient;

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

            _restClient = new RestApiClient(messageSender, messageStatusChecker, messageInboxFetcher);

            var command = "";
            _restClient.WriteLine("Started SMS Console!");
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
                            int? inboxNumber = null;
                            if (commands.Length > 1)
                            {
                                int parsedInboxNumber;
                                var isValidNumber = TryParse(commands[1], out parsedInboxNumber);
                                if (!isValidNumber)
                                {
                                    _restClient.WriteLine($"Error! {commands[1]} is not a valid message number!");
                                }
                                else
                                {
                                    inboxNumber = parsedInboxNumber;
                                }
                            }

                            _restClient.CheckInbox(inboxNumber);
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
                    _restClient.WriteLine(ex.Message);
                    _restClient.WriteLine(ex.StackTrace);
                }
            }
            _restClient.WriteLine("Exiting");
        }

        private static void ShowHelpText()
        {
            Console.WriteLine("Usage: (replace bracketed words with your values)");
            Console.WriteLine("Send message: 'Send [phonenumber] [message]'");
            Console.WriteLine(
                "Check message: \'Check [messageID]\' or \'Check Last\' to see details of the last sent message");
            Console.WriteLine(
                "Check Inbox: \'Inbox\' to get a list of messages or \'Inbox [messagenumber]\' to see a specific message ");
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
