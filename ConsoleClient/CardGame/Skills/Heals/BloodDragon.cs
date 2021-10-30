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
            double procent = 0.13;
            ILiveCard owner = Owner as ILiveCard;
            owner.HP += (int)(owner.MaxHP * procent);

            CardGameEngine.WriteLog($"[skill] {Owner.Title} executed {Title}: {Owner.Title} regen (+{(int)(owner.MaxHP * procent)}) ({procent * 100}% of MaxHP)");
        }
    }
}
