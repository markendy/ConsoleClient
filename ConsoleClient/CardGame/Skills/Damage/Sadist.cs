using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Scenes;


namespace ConsoleClient.CardGame.Skills.Damage
{
    public class Sadist : BaseSkill
    {
        public Sadist(Card owner)
        {
            Owner = owner;
            Title = nameof(Sadist);
        }


        /// <summary>
        /// Sadist damage all enemy in 35% of HP
        /// </summary>        
        public override void Execute()
        {
            foreach(Warrior target in (Owner.CurrentScene as Battle).FriendCards)
            {
                if (target is null)
                    continue;

                double procent = 0.35;
                
                int addValue = (int)(target.HP * procent);
                target.HP -= addValue;
                CardGameEngine.WriteLog($"[skill] {Owner.Title}::{Title}> {target.Title} (-{addValue}) ({procent * 100}% of CurrentHP)");
                
                var owner = (Owner as Warrior);                              
                owner.HP += addValue;                
                CardGameEngine.WriteLog($"and {owner.Title} (+{addValue}) ({procent * 100}% of CurrentHP enemy)");
            }            
        }
    }
}
