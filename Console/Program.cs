using InductionRestAPI;
using InductionRestAPI.Clients;
using InductionRestAPI.Models;
using System;
using System.Configuration;
using System.IO;

namespace SMSConsole
{
    class Program
    {

        static string _lastSentHeader = "";
        static RestApiClient _restClient;

        static void Main(string[] args)
        {
            var AccountRef = ConfigurationManager.AppSettings["AccountRef"];
            var RestUsername = ConfigurationManager.AppSettings["RestUsername"];
            var secretDir = ConfigurationManager.AppSettings["SecretDirectory"];

            var BaseRestAPI = ConfigurationManager.AppSettings["BaseRestAPI"];
            var AuthenticationEndpoint = ConfigurationManager.AppSettings["AuthenticationEndpoint"];
            var MessageSendEndpoint = ConfigurationManager.AppSettings["MessageSendEndpoint"];
            var MessageStatusEndpoint = ConfigurationManager.AppSettings["MessageStatusEndpoint"];
            var MessageInboxEndpoint = ConfigurationManager.AppSettings["MessageInboxEndpoint"];

            string password = Utility.GetSecret("password", secretDir);

            RestAuthenticator authenticator = new RestAuthenticator(BaseRestAPI, AuthenticationEndpoint, RestUsername, password);
            
            MessageSender messageSender = new MessageSender(MessageSendEndpoint, authenticator, AccountRef);
            MessageStatusChecker messageStatusChecker = new MessageStatusChecker(MessageStatusEndpoint, authenticator);
            MessageInboxFetcher messageInboxFetcher = new MessageInboxFetcher(MessageInboxEndpoint, authenticator);

            _restClient = new RestApiClient(messageSender, messageStatusChecker, messageInboxFetcher);


            var command = "";
            ShowHelpText();
            while (command != "exit")
            {
                command = Console.ReadLine();
                Utility.Log(Utility.AddTimestampTo($"Command: {command}"));

                if (command != "exit")
                {
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
                                    var messageID = commands[1].ToLower() == "last" ? _lastSentHeader : commands[1];
                                    _restClient.CheckMessageStatus(messageID);
                                    break;
                                }
                            case "inbox":
                                {
                                    int parsedInboxNumber;
                                    int? inboxNumber = null;
                                    if (commands.Length > 1)
                                    {
                                        var isValidNumber = Int32.TryParse(commands[1], out parsedInboxNumber);
                                        if (!isValidNumber)
                                        {
                                            _restClient.WriteLine($"Error! {commands[1]} is not a valid message number!" );
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
                            case "help":
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
                else
                {
                    _restClient.WriteLine("Exiting");
                }
            }
        }


        private static void ShowHelpText()
        {
            Console.WriteLine("Usage: (replace bracketed words with your values)");
            Console.WriteLine("Send message: 'Send [phonenumber] [message]'");
            Console.WriteLine($"Check message: 'Check [messageID]' or 'Check Last' to see details of the last sent message");
            Console.WriteLine($"Check Inbox: 'Inbox' to get a list of messages or 'Inbox [messagenumber]' to see a specific message ");
        }

        private static string BuildMessage(string[] commands)
        {
            var message = "";
            for (int i = 2; i < commands.Length; i++)
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
