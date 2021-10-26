using ConsoleClient.Primitives;
using System;


namespace ConsoleClient.Commands
{
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
}
