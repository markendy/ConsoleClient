namespace ConsoleClient.CardGame.Cards.Interfaces
{
    public interface ILiveCard
    {
        public int MaxHP { get; set; }

        public int HP { get; set; }

        public int Damage { get; set; }       
    }
}
