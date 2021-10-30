namespace ConsoleClient.CardGame.Common.Primitives
{
    public interface IDescribed
    {
        public string Title { get; set; }

        public string InnerTitle { get; }

        public string Description { get; set; }
    }
}
