using ConsoleClient.Common.Langs;
using ConsoleClient.Primitives;
using System;
using System.Collections.Generic;


namespace ConsoleClient.Commands.Factories
{
    public class DefaultCommandFactory
    {
        public List<Command> AllCommand { get; } = new List<Command>();


        public void InitDefaultCommands()
        {
            AllCommand.Add(new Command("set_lang", ChangeLangExecuteEventHandler, 1));
            AllCommand.Add(new Command("clear", ClearExecuteAction));
            AllCommand.Add(new Command("exit", ExitExecuteAction));
            AllCommand.Add(new Command("version", GetVersionExecuteAction));
            AllCommand.Add(new Command("help", HelpExecuteAction));
            AllCommand.Add(new Command("help", HelpExecuteAction, 1));

            Command get = new Command("get");
            {
                Command time = new Command("time");
                {
                    time.ChildCommand.Add(new Command("long", GetLongTimeExecuteAction));
                    time.ChildCommand.Add(new Command("short", GetShortTimeExecuteAction));
                }
                get.ChildCommand.Add(time);
                get.ChildCommand.Add(new Command("date", GetDateExecuteAction));
            }
            AllCommand.Add(get);
        }


        private void ClearExecuteAction()
        {
            Console.Clear();
        }


        private void GetVersionExecuteAction()
        {
            Console.WriteLine($"Version: {CommandLineHandler.Version}");
        }


        private void GetLongTimeExecuteAction()
        {
            Console.WriteLine(DateTime.Now.TimeOfDay);
        }


        private void GetShortTimeExecuteAction()
        {
            Console.WriteLine(DateTime.Now.ToString("HH:mm:ss"));
        }


        private void GetDateExecuteAction()
        {
            Console.WriteLine(DateTime.Now.ToShortDateString());
        }


        private void ExitExecuteAction()
        {
            Console.WriteLine(CommandLineHandler.MessagesLang.ByeBye);
            Environment.Exit(0);
        }


        private void HelpExecuteAction()
        {
            Console.WriteLine(CommandLineHandler.MessagesLang.ListCommand);

            foreach (var command in AllCommand)
            {
                Console.WriteLine(command.GetTitleWithCountParams());
            }

            return;
        }


        private void HelpExecuteAction(object o, CustomArgs e)
        {
            Command currentCommand = null;

            (_, currentCommand) = CommandLineHandler.GetByPathCommand(e.SendString);

            if (currentCommand is not null)
            {
                Console.WriteLine($"help for: {currentCommand.GetTitleWithCountParams()}");
                foreach (var command in currentCommand.ChildCommand)
                {
                    Console.WriteLine(command.GetTitleWithCountParams());
                }
                return;
            }
        
            Console.WriteLine(CommandLineHandler.MessagesLang.NotFoundCommand);            
        }


        private void ChangeLangExecuteEventHandler(object o, CustomArgs e)
        {
            switch (e.SendString)
            {
                case "ru":
                    CommandLineHandler.MessagesLang = RuMessages.GetInstance();
                    break;
                case "en":
                    CommandLineHandler.MessagesLang = EnMessages.GetInstance();
                    break;
                default:
                    Console.WriteLine(CommandLineHandler.MessagesLang.ErrorParams);
                    break;
            }
        }
    }
}
