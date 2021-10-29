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
        /// Head hero in every step
        /// </summary>
        public override void Execute()
        {
            ILiveCard owner = Owner as ILiveCard;
            owner.HP += (int)(owner.MaxHP * 0.15);
        }
    }
}
