using ConsoleClient.Primitives.Interfaces;


namespace ConsoleClient.Common.Langs
{
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
}
