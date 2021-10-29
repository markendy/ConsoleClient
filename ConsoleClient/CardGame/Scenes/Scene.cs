using ConsoleClient.CardGame.Cards;
using System;


namespace ConsoleClient.CardGame.Scenes
{
    public class Scene
    {
        public Scene()
        {
        }


        public int Id { get; set; }


        public virtual void Start(Card[] friendCards, Card[] enemyCards)
        {
            throw new NotImplementedException();
        }
    }
}
