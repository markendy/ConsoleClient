using ConsoleClient.Primitives;
using ConsoleClient.Primitives.Enums;
using System;
using System.Collections.Generic;


namespace ConsoleClient.Commands
{
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
            return $"command : {Title}";
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
}
