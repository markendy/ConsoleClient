using System;
using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Interfaces;
using ConsoleClient.CardGame.Cards.Primitives;


namespace ConsoleClient.CardGame.Skills.Resistance
{
    public class HeartOfStone : BaseSkill
    {        
        public HeartOfStone(Card owner) : base(owner)
        {
            (Owner as ILiveCard).BeforeHpTaked += AddAditationalresistance;
        }

      
        public override void Execute()
        {
            return;                   
        }


        /// <summary>
        /// Passive add resistance when take damage
        /// </summary>  
        protected void AddAditationalresistance(object sender, HpChangeEventArgs args)
        {
            double addResistance = 0.05;
            ILiveCard owner = Owner as ILiveCard;
            owner.PhysicalResistance += Math.Round(owner.PhysicalResistance, 1) < 0.9 ? addResistance : 0;
        }
    }
}
