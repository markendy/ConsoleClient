using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Interfaces;


namespace ConsoleClient.CardGame.Skills.Heals
{
    public class BloodDragon : BaseSkill
    {
        public BloodDragon(Card owner)
        {
            Owner = owner;
            Title = nameof(BloodDragon);
        }


        /// <summary>
        /// Head hero in every step on 20%
        /// </summary>
        public override void Execute()
        {
            double procent = 0.2;
            ILiveCard owner = Owner as ILiveCard;
            int addValue = (int)(owner.MaxHP * procent);
            
            CardGameEngine.WriteLog($"[skill] {Owner.Title}::{Title}> {Owner.Title} (+{addValue}) ({procent * 100}% of MaxHP)");
            
            owner.HP += addValue;
        }
    }
}
