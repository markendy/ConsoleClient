using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Interfaces;
using ConsoleClient.CardGame.Scenes;


namespace ConsoleClient.CardGame.Skills.Damage
{
    public class FullCombatReadiness : BaseSkill
    {
        private int _oldDamage;

        public FullCombatReadiness(Card owner)
        {
            Owner = owner;
            Title = nameof(FullCombatReadiness);
        }


        /// <summary>
        /// Add damage on step % 2 = 0 on 30%
        /// </summary>        
        public override void Execute()
        {
            ILiveCard owner = Owner as ILiveCard;

            if ((Owner.CurrentScene as Battle).CurrentStep % 2 != 0)
            {
                //owner.Damage = _oldDamage;
                return;
            }

            double procent = 0.3;            
            _oldDamage = owner.Damage;
            int addValue = (int)(_oldDamage * procent);
            owner.Damage += addValue;

            CardGameEngine.WriteLog($"[skill] {Owner.Title}::{Title}> {Owner.Title} ({_oldDamage}+{addValue}) ({procent * 100}% of Damage)");
        }
    }
}
