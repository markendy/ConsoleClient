using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Interfaces;
using ConsoleClient.CardGame.Cards.Primitives;


namespace ConsoleClient.CardGame.Skills.Damage
{
    public class BerserkersBlood : BaseSkill
    {        
        private double addDamage;


        public BerserkersBlood(Card owner): base(owner)
        {
            (Owner as ILiveCard).AfterHpChanged += AddAditationalDamage;
        }

                
        public override void Execute()
        {
            return;
        }


        /// <summary>
        /// Passive add resistance when take damage
        /// </summary>  
        protected void AddAditationalDamage(object sender, HpChangeEventArgs args)
        {
            ILiveCard owner = Owner as ILiveCard;

            if (owner.MaxHP == 0 || owner.HP == 0 || owner.MaxHP == owner.HP)         
                return;

            owner.AdditionalDamage -= (int)addDamage;
            addDamage = 10 * (100 - owner.HP * 100 / owner.MaxHP);            
            owner.AdditionalDamage += (int)addDamage;
        }
    }
}
