using ConsoleClient.Commands;
using ConsoleClient.Commands.Factories;
using ConsoleClient.Common.Langs;
using ConsoleClient.Primitives;
using ConsoleClient.Primitives.Enums;
using ConsoleClient.Primitives.Interfaces;
using System;
using System.Collections.Generic;

namespace ConsoleClient
{
    public static class CommandLineHandler
    {
        private static Command _currentCommand = null;


        public static int MaxParamsCount { get; } = 16;

        public static string Version { get; } = "v1.2";

        public static IMessage MessagesLang { get; set; } = EnMessages.GetInstance();

        public static DefaultCommandFactory CommandFactory { get; } = new DefaultCommandFactory();


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

                    int result;
                    (result, _currentCommand) = GetByPathCommand(line, " ");

                    if (_currentCommand is null)
                    {
                        Console.WriteLine(MessagesLang.NotFoundCommand);
                        break;
                    }

                    if (result < 0)
                    {
                        Console.WriteLine(MessagesLang.NullParams);
                        needExit = true;
                        break;
                    }

                    switch (_currentCommand.CommandType)
                    {
                        case CommandType.List:                            
                            continue;

                        case CommandType.Command:
                            if (_currentCommand.ParamsCount == 0)
                                _currentCommand.Execute();
                            else
                            {                                
                                if (result > 0)
                                {
                                    Console.WriteLine(MessagesLang.MoreThenMaxParams +
                                    " (" + _currentCommand.ParamsCount + ")");
                                    needExit = true;
                                    break;
                                }

                                string parametrs = string.Empty;
                                for (int j = i + 1; j < allLine.Length; ++j)
                                {
                                    parametrs = string.Concat(parametrs, allLine[j]);
                                }
                                _currentCommand.Execute((object)"sender", new CustomArgs() { SendString = parametrs });
                            }
                            needExit = true;
                            break;
                    }
                }
                needExit = false;
                _currentCommand = null;
            }
        }


        public static (int, Command) GetByPathCommand(string path, string separator = ".", Command rootCommand = null, int cmdPosition = 0)
        {
            Command currentCommand = null;

            int resultStatus = -1;

            string[] cmdPath = path.Split(separator);

            List<Command> listCommand = cmdPosition == 0 ? CommandFactory.AllCommand : rootCommand.ChildCommand;

            var currentParamsCount = cmdPath.Length - cmdPosition - 1;
            bool isLastCommand = cmdPosition == cmdPath.Length - 1;

            rootCommand = listCommand.Find(c => c.Title == cmdPath[cmdPosition] && c.ParamsCount == currentParamsCount);

            if (rootCommand is null)
            {
                for (int i = currentParamsCount, equalValue = 0; ;)
                {
                    rootCommand = rootCommand ?? listCommand.Find(c => c.Title == cmdPath[cmdPosition] && c.ParamsCount == i);

                    if (rootCommand is null)
                    {
                        if (i >= equalValue && equalValue == 0)
                        {
                            --i;
                            continue;
                        }

                        if (equalValue == 0)
                        {
                            i = 0;
                            equalValue = CommandLineHandler.MaxParamsCount;
                        }

                        if (i <= equalValue)
                        {
                            ++i;
                            continue;
                        }

                        break;
                    }
                    break;
                }
            }

            currentCommand = rootCommand;

            if (currentCommand is null)
                return (resultStatus, currentCommand);

            if (!isLastCommand)
            {
                if (currentCommand.CommandType == CommandType.List)                    
                    (resultStatus, currentCommand) = GetByPathCommand(path, separator, rootCommand, ++cmdPosition);
            }

            if (currentCommand?.CommandType is CommandType.Command)
            {
                if (rootCommand.ParamsCount == currentParamsCount)
                    resultStatus = 0;
                else if (rootCommand.ParamsCount < currentParamsCount)
                    resultStatus = 1;
            }

            return (resultStatus, currentCommand);
        }
           
    
        public static void GetByPathCommand_Old(ref Command currentCommand, string path, string separator = ".")
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
