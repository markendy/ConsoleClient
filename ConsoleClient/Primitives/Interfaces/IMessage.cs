namespace ConsoleClient.Primitives.Interfaces
{
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
    }
}
