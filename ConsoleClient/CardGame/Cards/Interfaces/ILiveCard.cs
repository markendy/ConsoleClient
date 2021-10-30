using ConsoleClient.CardGame.Cards.Primitives;

namespace ConsoleClient.CardGame.Cards.Interfaces
{
    public interface ILiveCard
    {
        public int MaxHP { get; }

        public int HP { get; }

        public int BaseDamage { get; set; }

        public int AdditionalDamage { get; set; }

        public int CurrentDamage { get; }



        public void TakeHP(HpChangeEventArgs args);

        public void GiveHP(HpChangeEventArgs args);
    }
}
