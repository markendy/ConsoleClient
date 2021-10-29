using ConsoleClient.CardGame.Heroes;


namespace ConsoleClient.CardGame.Scenes
{
    public class Battle : Scene
    {
        public Battle()
        {
        }


        public Card [] AllCards { get; set; }


        public Card[] FriendCards { get; set; }


        public Card[] EnemyCards { get; set; }


        public int MaxCardCount { get; set; } = BaseSettings.GameSettings.MaxCardCount;


        public int HeroId { get; set; } = 0;


        public int BonusCardId { get; set; } = BaseSettings.GameSettings.MaxCardCount - 1;
    }
}
