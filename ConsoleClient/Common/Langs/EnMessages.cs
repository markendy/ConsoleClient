using ConsoleClient.Primitives.Interfaces;


namespace ConsoleClient.Common.Langs
{
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
}
