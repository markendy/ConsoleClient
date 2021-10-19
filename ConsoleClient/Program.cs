using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleClient
{
    public class Program
    {
        private static CommandFactory _commandFactory { get; } = new CommandFactory();


        public static void Main(string[] args)
        {
            Command? currnetCommand = null;

            Console.WriteLine(CommandFactory.MessagesLang.Hello);

            //load commands from db...
            _commandFactory.InitDefaultCommands();
            _commandFactory.AllCommand.Add(new WriteCommand());

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

                    if (currnetCommand is not null)
                    {
                        currnetCommand = currnetCommand.ChildCommand.Find(c => c.Title == allLine[i]);
                    }
                    else
                    {
                        currnetCommand = _commandFactory.AllCommand.Find(c => c.Title == allLine[i]);
                    }

                    if (currnetCommand is null)
                    {
                        Console.WriteLine(CommandFactory.MessagesLang.NotFoundCommand);
                        break;
                    }

                    var remainderCount = allLine.Length - i - 1;
                    
                    if (remainderCount < currnetCommand.ParamsCount)
                    {
                        Console.WriteLine(CommandFactory.MessagesLang.NullParams);
                        needExit = true;
                        break;
                    }

                    switch (currnetCommand.CommandType)
                    {
                        case CommandType.List:
                            if (remainderCount == 0)
                            {
                                Console.WriteLine(CommandFactory.MessagesLang.NullParams);
                                needExit = true;
                                break;
                            }
                            continue;                            
                        
                        case CommandType.Command:
                            if (currnetCommand.ParamsCount == 0)
                                currnetCommand.Execute();
                            else
                            {
                                if (remainderCount == 0)
                                {
                                    Console.WriteLine(CommandFactory.MessagesLang.NullParams);
                                    needExit = true;
                                    break;
                                }

                                else if (remainderCount > currnetCommand.ParamsCount)
                                {
                                    Console.WriteLine(CommandFactory.MessagesLang.MoreThenMaxParams +
                                        " (" + currnetCommand.ParamsCount + ")");
                                    needExit = true;
                                    break;
                                }

                                string parametrs = string.Empty;
                                for (int j = i + 1; j < allLine.Length; ++j)
                                {
                                    parametrs = string.Concat(parametrs, allLine[j]);
                                }
                                currnetCommand.Execute((object)"sender", new CustomArgs() { sendString = parametrs });
                                needExit = true;
                            }
                            break;
                    }
                }
                needExit = false;
                currnetCommand = null;
            }
        }
    }


    public class WriteCommand : Command
    {
        public WriteCommand()
        {}

        public WriteCommand(string title, Action? action, int paramsCount) : base (title, action, paramsCount)
        {}

        public WriteCommand(string title, EventHandler<CustomArgs>? eventHandler, int paramsCount, CommandType commandType = default) 
            : base (title, eventHandler, paramsCount, commandType)
        {}


        public override void Init()
        {
            base.Init();

            Title = "write";
            ParamsCount = 1;
        }


        public override void Execute(object o, CustomArgs e)
        {
            ClearExecuteEvents();
            base.Execute(o, e);      

            Console.WriteLine($"{e.sendString}");
        }
    }


    public class CommandFactory
    {
        public static IMessage MessagesLang { get; set; } = EnMessages.GetInstance();

        public List<Command> AllCommand { get; } = new List<Command>();


        public void InitDefaultCommands()
        {
            AllCommand.Add(new Command("set_lang", ChangeLangExecuteEventHandler, 1));
            AllCommand.Add(new Command("clear", ClearExecuteAction , 0));            
            AllCommand.Add(new Command("exit", ExitExecuteAction, 0));
            AllCommand.Add(new Command("help", HelpExecuteAction, 0));

            Command get = new Command("get", CommandType.List);
            {
                Command time = new Command("time", CommandType.List);
                {
                    time.ChildCommand.Add(new Command("long", GetLongTimeExecuteAction, 0));
                    time.ChildCommand.Add(new Command("short", GetShortTimeExecuteAction, 0));
                }
                get.ChildCommand.Add(time);
                get.ChildCommand.Add(new Command("date", GetDateExecuteAction, 0));
            }
            AllCommand.Add(get);
        }


        private void ClearExecuteAction()
        {
            Console.Clear();
        }


        private void GetLongTimeExecuteAction()
        {
            Console.WriteLine(DateTime.Now.TimeOfDay);
        }


        private void GetShortTimeExecuteAction()
        {
            Console.WriteLine(DateTime.Now.ToString("hh:mm:ss"));
        }


        private void GetDateExecuteAction()
        {
            Console.WriteLine(DateTime.Now.ToShortDateString());
        }


        private void ExitExecuteAction()
        {
            Console.WriteLine(CommandFactory.MessagesLang.ByeBye);
            Environment.Exit(0);
        }


        private void HelpExecuteAction()
        {
            Console.WriteLine(CommandFactory.MessagesLang.ListCommand);

            foreach (var command in AllCommand)
            {
                Console.WriteLine(command.Title);
            }
        }


        private void ChangeLangExecuteEventHandler(object o, CustomArgs e)
        {
            switch (e.sendString)
            {
                case "ru":
                    MessagesLang = RuMessages.GetInstance(); 
                    break;
                case "en":
                    MessagesLang = EnMessages.GetInstance();
                    break;
                default:
                    Console.WriteLine(CommandFactory.MessagesLang.ErrorParams);
                    break;
            }
        }
    }


    public class CustomArgs : EventArgs
    {
        public string sendString { get; set; }
    }


    public interface IMessage
    {
        public string NullParams { get; }

        public string ErrorParams { get; }

        public string MoreThenMaxParams { get; }

        public string NotFoundCommand { get; }

        public string GeneralizedError { get; }

        public string Hello { get; }

        public string ByeBye { get; }

        public string ListCommand { get; }

        public static RuMessages GetInstance;
    }


    public class RuMessages : IMessage
    {
        private static RuMessages _instanse;


        private RuMessages()
        {}


        public string NullParams { get; } = "параметры команды пусты";

        public string ErrorParams { get; } = "ошибка в параметре";

        public string MoreThenMaxParams { get; } = "параметров больше чем нужно";

        public string NotFoundCommand { get; } = "команда не найдена";

        public string GeneralizedError { get; } = "непрописанная ошибка";

        public string Hello { get; } = "Привет! Стартуем!";

        public string ByeBye { get; } = "До встречи, классно поработали!";

        public string ListCommand { get; } = "Лист комманд:";


        public static RuMessages GetInstance()
        {
            if (_instanse is null)
            {
                _instanse = new RuMessages();
            }

            return _instanse;
        }
    }


    public class EnMessages : IMessage
    {
        private static EnMessages _instanse;


        private EnMessages()
        { }


        public string NullParams { get; } = "params command null";

        public string ErrorParams { get; } = "error in params";

        public string MoreThenMaxParams { get; } = "more parameters than needed";

        public string NotFoundCommand { get; } = "command not found";

        public string GeneralizedError { get; } = "unwritten error";

        public string Hello { get; } = "Hello! Start!";

        public string ByeBye { get; } = "See you, we did a great job!";

        public string ListCommand { get; } = "List commands:";

        public static EnMessages GetInstance()
        {
            if (_instanse is null)
            {
                _instanse = new EnMessages();
            }

            return _instanse;
        }
    }


    public class Command
    {        
        private Action _executeAction;

        private EventHandler<CustomArgs> _executeEventHandler;


        protected Command()
        {
            Init();
        }

        private Command(string title)
        {
            Title = title;
        }

        public Command(string title, CommandType commandType) : this(title)
        {
            CommandType = commandType;

            Init();
        }

        public Command(string title, Action? action, int paramsCount, CommandType commandType = default) : this(title)
        {
            _executeAction += action;
            ParamsCount = paramsCount;

            Init();
        }

        public Command(string title, EventHandler<CustomArgs>? eventHandler, int paramsCount, CommandType commandType = default) : this(title)
        {
            _executeEventHandler += eventHandler;
            ParamsCount = paramsCount;

            Init();
        }
       

        public string Title { get; set; }

        public List<Command> ChildCommand { get; } = new List<Command>();

        public int ParamsCount { get; set; }

        public CommandType CommandType { get; set; }


        public virtual void Init()
        {
            
        }


        public void ClearExecuteEvents()
        {
            _executeAction = null;
            _executeEventHandler = null;
        }


        public virtual void BeforeExecute()
        {

        }


        public virtual void Execute()
        {
            BeforeExecute();
            
            if (_executeAction is null)
            {
                System.Diagnostics.Debug.WriteLine($"NULL ACTION command={Title}-{CommandFactory.MessagesLang.GeneralizedError}");
                return;
            }

            _executeAction?.Invoke();

            AfterExecute();
        }


        public virtual void AfterExecute()
        {

        }


        public virtual void BeforeExecute(object o, CustomArgs e)
        {

        }


        public virtual void Execute(object o, CustomArgs e)
        {
            if (string.IsNullOrEmpty(e.sendString))
            {
                Console.WriteLine(CommandFactory.MessagesLang.NullParams);
                return;
            }

            BeforeExecute(o, e);


            if (_executeEventHandler is null)
            {
                System.Diagnostics.Debug.WriteLine($"NULL ACTION command with params={Title}-{CommandFactory.MessagesLang.GeneralizedError}");
                return;
            }
            _executeEventHandler?.Invoke(o, e);

            AfterExecute(o, e);
        }


        public virtual void AfterExecute(object o, CustomArgs e)
        {

        }        
    }


    public enum CommandType
    {
        Command = 0,
        List = 1,
    }
}
