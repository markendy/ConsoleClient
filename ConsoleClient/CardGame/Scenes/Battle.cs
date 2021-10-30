using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Primitives;
using System.Collections.Generic;


namespace ConsoleClient.CardGame.Scenes
{
    public class Battle : Scene
    {
        public Battle()
        {
        }


        private List<Card> AllCards { get; set; } = new List<Card>();

        private Card[] FriendCards { get; set; }

        private Card[] EnemyCards { get; set; }

        public int MaxCardCount { get; set; } = BaseSettings.GameSettings.MaxCardCount;        

        public bool IsEndGame { get; set; }

        public int CurrentStep { get; set; }


        public override void Start(Card[] friendCards, Card[] enemyCards)
        {
            CardGameEngine.WriteLog("Start battle");
            
            FriendCards = friendCards;
            EnemyCards = enemyCards;

            AllCards.AddRange(friendCards);
            AllCards.AddRange(enemyCards);

            while (!IsEndGame)
            {
                CardGameEngine.WriteLog($"\n\n===========================================\nStep {CurrentStep}");

                ExecuteBoard(FriendCards);
                ExecuteBoard(EnemyCards);

                if (friendCards[0] is null || enemyCards[0] is null)
                {
                    IsEndGame = true;
                    string od = friendCards[0] is null ? "Enemy" : "Friend";
                    CardGameEngine.WriteLog($"\n===========================================\nGame end! Won {od} team");
                }

                ++CurrentStep;
            }
        }


        private void ExecuteBoard(Card[] cards)
        {
            for (int i = 0; i < cards.Length; ++i)
            {
                if (cards[i] is not null)
                {
                    cards[i].MakeStep();
                    CardGameEngine.WriteLog($"-------------------------------------------\n");
                }

                RebalanseCards(CardTag.Friend);
                RebalanseCards(CardTag.Enemy);
            }
        }


        public Card GetTargetForCard(int needInBoardCardId, CardTag needTag)
        {
            Card[] tempCards = null;

            switch (needTag)
            {
                case CardTag.Friend:
                    tempCards = FriendCards;
                    break;
                case CardTag.Enemy:
                    tempCards = EnemyCards;
                    break;                    
            }

            for (int i = needInBoardCardId; i >= 0; --i)
            {
                try
                {
                    if (tempCards[i] is not null)
                        return tempCards[i];
                }
                catch
                {
                    continue;
                }
            }

            return null;
        }


        public Card[] GetAllEnemyWarriors(CardTag needTag)
        {
            switch (needTag)
            {
                case CardTag.Friend:
                    return FriendCards;
                case CardTag.Enemy:
                    return EnemyCards;
            }
            return null;
        }


        public void RemoveCardFromCardList(int cardId, CardTag tag)
        {
            Card[] tempCards = null;

            switch (tag)
            {
                case CardTag.Friend:
                    tempCards = FriendCards;
                    break;
                case CardTag.Enemy:
                    tempCards = EnemyCards;
                    break;
            }

            tempCards[cardId] = null;
        }


        private void RebalanseCards(CardTag tag)
        {
            Card[] tempCards = null;

            switch (tag)
            {
                case CardTag.Friend:
                    tempCards = FriendCards;
                    break;
                case CardTag.Enemy:
                    tempCards = EnemyCards;
                    break;
            }

            for (int i = 0; i < tempCards.Length; ++i)
            {
                if (tempCards[i] is null) 
                {
                    int j = i;
                    Card tempCard = null;
                    while (j < tempCards.Length && tempCard is null)
                    {
                        tempCard = tempCards[j];
                        ++j;
                    }

                    if (tempCard is null)
                    {
                        return;
                    }

                    tempCards[i] = tempCard;
                    tempCards[i].InBoardId = i;
                    tempCards[j - 1] = null;
                }
            }
        }
    }
}
