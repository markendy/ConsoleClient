using ConsoleClient.Commands;
using ConsoleClient.Commands.Factories;
using ConsoleClient.Common.Langs;
using ConsoleClient.Primitives;
using ConsoleClient.Primitives.Enums;
using ConsoleClient.Primitives.Interfaces;
using System;


namespace ConsoleClient
{
    public static class CommandLineHandler
    {
        private static Command _currnetCommand = null;


        public static int MaxParamsCount { get; } = 16;

        public static IMessage MessagesLang { get; set; } = EnMessages.GetInstance();

        public static CommandFactory CommandFactory { get; } = new CommandFactory();


        public static void Start()
        {
            Console.WriteLine(MessagesLang.Hello);

            //load commands from db...
            CommandFactory.InitDefaultCommands();
           
            CommandFactory.AllCommand.Add(new WriteCommand());
            CommandFactory.AllCommand.Add(new TimeCommand());

            string line = string.Empty;

            while (true)
            {
                line = Console.ReadLine();

                string[] allLine = line.Split();

                bool needExit = false;

                for (int i = 0; i < allLine.Length && !needExit; ++i)
                {
                    if (string.IsNullOrEmpty(allLine[0]))
                    {
                        break;
                    }

                    var currentParamsCount = allLine.Length - i - 1;

                    GetByPathCommand(ref _currnetCommand, line, " ");

                    if (_currnetCommand is null)
                    {
                        Console.WriteLine(MessagesLang.NotFoundCommand);
                        break;
                    }

                    if (currentParamsCount < _currnetCommand.ParamsCount)
                    {
                        Console.WriteLine(MessagesLang.NullParams);
                        needExit = true;
                        break;
                    }

                    switch (_currnetCommand.CommandType)
                    {
                        case CommandType.List:
                            if (currentParamsCount == 0)
                            {
                                Console.WriteLine(MessagesLang.NullParams);
                                needExit = true;
                                break;
                            }
                            continue;

                        case CommandType.Command:
                            if (_currnetCommand.ParamsCount == 0)
                                _currnetCommand.Execute();
                            else
                            {
                                if (currentParamsCount == 0)
                                {
                                    Console.WriteLine(MessagesLang.NullParams);
                                    needExit = true;
                                    break;
                                }
                                else if (currentParamsCount > _currnetCommand.ParamsCount)
                                {
                                    Console.WriteLine(MessagesLang.MoreThenMaxParams +
                                    " (" + _currnetCommand.ParamsCount + ")");
                                    needExit = true;
                                    break;
                                }

                                string parametrs = string.Empty;
                                for (int j = i + 1; j < allLine.Length; ++j)
                                {
                                    parametrs = string.Concat(parametrs, allLine[j]);
                                }
                                _currnetCommand.Execute((object)"sender", new CustomArgs() { SendString = parametrs });
                            }
                            needExit = true;
                            break;
                    }
                }
                needExit = false;
                _currnetCommand = null;
            }
        }


        public static void GetByPathCommand(ref Command currentCommand, string path, string separator = ".")
        {
            string[] cmdPath = path.Split(separator);

            Command? rootCommand = null;
            int cmdPosition = 0;

            for (int i = CommandLineHandler.MaxParamsCount - 1; i >= 0; --i)
                if (cmdPosition < cmdPath.Length - 1)
                {
                    if (rootCommand is null)
                        rootCommand = CommandFactory.AllCommand.Find(c => c.Title == cmdPath[cmdPosition] && c.ParamsCount == i);
                    else
                        break;
                }
                else
                {
                    rootCommand = CommandFactory.AllCommand.Find(c => c.Title == cmdPath[cmdPosition] && c.ParamsCount == 0);
                    break;
                }

            if (rootCommand is null)
            {
                return;
            }

            currentCommand = rootCommand;
            Command? tempCommand = currentCommand;

            ++cmdPosition;
            for (int i = CommandLineHandler.MaxParamsCount - 1; i >= 0 && cmdPosition <= cmdPath.Length - 1;)
            {
                if (tempCommand.CommandType == CommandType.List)
                {
                    currentCommand = tempCommand.ChildCommand.Find(c => c.Title == cmdPath[cmdPosition] && c.ParamsCount == i);
                    tempCommand = currentCommand ?? tempCommand;
                }

                else if (tempCommand.CommandType == CommandType.Command)
                {
                    return;
                }

                --i;

                if (i < 0)
                {
                    ++cmdPosition;
                    if (cmdPosition == cmdPath.Length)
                    {
                        return;
                    }
                    else
                    {
                        i = CommandLineHandler.MaxParamsCount - 1;
                    }
                }
            }
        }


        [Obsolete]
        public static void GetCurrentCommand(ref Command currnetCommand, int i, string[] allLine, int paramsCount)
        {
            if (currnetCommand is null)
            {
                currnetCommand = CommandFactory.AllCommand.Find(c => c.Title == allLine[i] && c.ParamsCount == paramsCount);

                return;
            }

            currnetCommand = currnetCommand.ChildCommand.Find(c => c.Title == allLine[i] && c.ParamsCount == paramsCount);
        }
    }
}
