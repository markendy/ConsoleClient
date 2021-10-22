using System;
using System.Linq;
using System.Collections.Generic;


namespace ConsoleClient
{
    public class Program
    {     
        public static void Main(string[] args)
        {
            CommandLineHandler.Start();
        }
    }


    public static class CommandLineHandler
    {
        private static Command? _currnetCommand = null;


        public static int MaxParamsCount { get; } = 16;

        public static IMessage MessagesLang { get; set; } = EnMessages.GetInstance();

        public static CommandFactory CommandFactory { get; } = new CommandFactory();


        public static void Start()
        {           
            Console.WriteLine(MessagesLang.Hello);

            //load commands from db...
            CommandFactory.InitDefaultCommands();
            CommandFactory.AllCommand.Add(new WriteCommand());

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
            for (int i = CommandLineHandler.MaxParamsCount - 1; i >= 0  && cmdPosition <= cmdPath.Length - 1;)
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


    public class WriteCommand : Command
    {
        public WriteCommand()
        {
            Init(eventHandler: LocalExecute);
        }


        protected override void Init(
            EventHandler<CustomArgs> beforeEventHandler = null, EventHandler<CustomArgs> eventHandler = null, EventHandler<CustomArgs> afterEventHandler = null)
        {
            base.Init(beforeEventHandler, eventHandler, afterEventHandler);

            ParamsCount = 1;
            Title = "write";       
        }


        private void LocalExecute(object o, CustomArgs e)
        {
            Console.WriteLine($"{e.SendString}");
        }
    }


    public class CommandFactory
    {        
        public List<Command> AllCommand { get; } = new List<Command>();


        public void InitDefaultCommands()
        {
            AllCommand.Add(new Command("set_lang", ChangeLangExecuteEventHandler, 1));
            AllCommand.Add(new Command("clear", ClearExecuteAction));
            AllCommand.Add(new Command("exit", ExitExecuteAction));
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

            CommandLineHandler.GetByPathCommand(ref currentCommand, e.SendString);

            if (currentCommand is not null)
            {
                Console.WriteLine($"help for: {currentCommand.GetTitleWithCountParams()}");
                foreach (var command in currentCommand.ChildCommand)
                {                    
                    Console.WriteLine(command.GetTitleWithCountParams());
                }
                return;
            }
            else
            {
                Console.WriteLine(CommandLineHandler.MessagesLang.NotFoundCommand);
            }

            
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


    public class CustomArgs : EventArgs
    {
        public string SendString { get; set; }
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
        { }


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
        public static int AllCommandCount = 0;

        private Action _executeBeforeAction;
        private Action _executeAction;
        private Action _executeAfterAction;

        private EventHandler<CustomArgs> _executeBeforeEventHandler;
        private EventHandler<CustomArgs> _executeEventHandler;
        private EventHandler<CustomArgs> _executeAfterEventHandler;


        protected Command()
        {
            Init();
        }

        public Command(string title)
        {
            Title = title;

            CommandType = CommandType.List;

            Init();
        }


        public Command(string title, Action action, Action beforeAction = null, Action afterAction = null)
        {
            Title = title;            

            ParamsCount = 0;
            CommandType = CommandType.Command;

            Init(beforeAction, action, afterAction);
        }


        public Command(string title, EventHandler<CustomArgs> eventHandler, int paramsCount, 
            EventHandler<CustomArgs> beforeEventHandler = null, EventHandler<CustomArgs> afterEventHandler = null)
        {
            Title = title;

            ParamsCount = paramsCount;
            CommandType = CommandType.Command;

            Init(beforeEventHandler, eventHandler, afterEventHandler);
        }


        public int Id { get; private set; }


        public string Title { get; set; }

        public List<Command> ChildCommand { get; } = new List<Command>();

        public int ParamsCount { get; set; }

        public CommandType CommandType { get; set; }


        protected virtual void Init()
        {
            InitId();
        }


        protected virtual void Init(Action beforeAction = null, Action action = null, Action afterAction = null)
        {
            InitId();

            _executeBeforeAction += beforeAction;
            _executeAction += action;
            _executeAfterAction += afterAction;
        }


        protected virtual void Init(
            EventHandler<CustomArgs> beforeEventHandler = null, EventHandler<CustomArgs> eventHandler = null, EventHandler<CustomArgs> afterEventHandler = null)
        {
            InitId();

            _executeBeforeEventHandler += beforeEventHandler;
            _executeEventHandler += eventHandler;
            _executeAfterEventHandler += afterEventHandler;
        }


        public void ClearBeforeExecuteEvents()
        {
            _executeBeforeAction = null;
            _executeBeforeAction = null;
        }


        public void ClearExecuteEvents()
        {
            _executeAction = null;
            _executeEventHandler = null;
        }


        public void ClearAfterExecuteEvents()
        {
            _executeAfterAction = null;
            _executeAfterEventHandler = null;
        }


        public void ClearAllExecuteEvents()
        {
            ClearBeforeExecuteEvents();
            ClearExecuteEvents();
            ClearAfterExecuteEvents();
        }


        public string GetTitleWithCountParams()
        {
            return Title + $"({ParamsCount})";
        }


        public override string ToString()
        {
            return $"help for : {Title}";
        }


        private void InitId()
        {
            Id = ++AllCommandCount;
        }


        private void BeforeExecute()
        {
            _executeBeforeAction?.Invoke();
        }


        public void Execute()
        {
            BeforeExecute();

            if (_executeAction is null)
            {
                System.Diagnostics.Debug.WriteLine($"NULL ACTION command={Title}-{CommandLineHandler.MessagesLang.GeneralizedError}");
                return;
            }

            _executeAction?.Invoke();

            AfterExecute();
        }


        private void AfterExecute()
        {
            _executeAfterAction?.Invoke();
        }


        private void BeforeExecute(object o, CustomArgs e)
        {
            _executeBeforeEventHandler?.Invoke(o, e);
        }


        public void Execute(object o, CustomArgs e)
        {
            if (string.IsNullOrEmpty(e.SendString))
            {
                Console.WriteLine(CommandLineHandler.MessagesLang.NullParams);
                return;
            }

            BeforeExecute(o, e);


            if (_executeEventHandler is null)
            {
                System.Diagnostics.Debug.WriteLine($"NULL ACTION command with params={Title}-{CommandLineHandler.MessagesLang.GeneralizedError}");
                return;
            }
            _executeEventHandler?.Invoke(o, e);

            AfterExecute(o, e);
        }


        private void AfterExecute(object o, CustomArgs e)
        {
            _executeAfterEventHandler?.Invoke(o, e);
        }
    }


    public enum CommandType
    {
        Command = 0,
        List = 1,
    }
}
