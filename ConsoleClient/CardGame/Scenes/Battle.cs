using ConsoleClient.CardGame.Cards;
using System;
using System.Collections.Generic;


namespace ConsoleClient.CardGame.Scenes
{
    public class Battle : Scene
    {
        public Battle()
        {
        }


        public List<Card> AllCards { get; set; } = new List<Card>();

        public Card[] FriendCards { get; set; }

        public Card[] EnemyCards { get; set; }

        public List<Card> ExecuteCards { get; } = new List<Card>();

        public int MaxCardCount { get; set; } = BaseSettings.GameSettings.MaxCardCount;

        public int HeroId { get; set; } = 0;

        public int BonusCardId { get; set; } = BaseSettings.GameSettings.MaxCardCount - 1;

        public bool IsEndGame { get; set; }

        public int CurrentStep { get; set; }


        public override void Start(Card[] friendCards, Card[] enemyCards)
        {
            CardGameEngine.WriteLog("Start battle");
            
            FriendCards = friendCards;
            EnemyCards = enemyCards;

            AllCards.AddRange(friendCards);
            AllCards.AddRange(enemyCards);

            ExecuteCards.AddRange(friendCards);
            ExecuteCards.AddRange(enemyCards);


            while (!IsEndGame)
            {
                CardGameEngine.WriteLog($"===========\nStep {CurrentStep}");
                for (int i = 0; i < ExecuteCards.Count; ++i)
                {
                    ExecuteCards[i]?.MakeStep();                                        
                }

                if (friendCards[0] is null || enemyCards[0] is null)
                {
                    IsEndGame = true;
                    int od = ExecuteCards[0] is null ? 1 : 0;
                    CardGameEngine.WriteLog($"Game end! Won {ExecuteCards[od].Title}");
                }

                ++CurrentStep;
            }
        }
    }
}
