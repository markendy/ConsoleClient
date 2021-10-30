using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Interfaces;
using ConsoleClient.CardGame.Cards.Primitives;


namespace ConsoleClient.CardGame.Skills.Heals
{
    public class HolyHand : BaseSkill
    {
        public HolyHand(Card owner)
        {
            Owner = owner;
            Title = nameof(HolyHand);
        }


        /// <summary>
        /// Head hero in every step on 20%
        /// </summary>
        public override void Execute()
        {
            int mainValue = 100;
            double procent = 0.1;
            ILiveCard owner = Owner as ILiveCard;
            int addValue = (int)(owner.MaxHP * procent);
            int endValue = mainValue + addValue;

            CardGameEngine.WriteLog($"[skill] {Owner.Title}::{Title}> {Owner.Title} (+{endValue}) (+{mainValue}+{procent * 100}% of MaxHP)");

            owner.GiveHP(new HpChangeEventArgs(this, endValue));
        }
    }
}
