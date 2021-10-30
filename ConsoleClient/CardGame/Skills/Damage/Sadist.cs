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

                target.HP -= (int)(target.HP * procent);
                CardGameEngine.WriteLog($"[skill] {Owner.Title}::{Title}> {target.Title} (-{(int)(target.HP * procent)}) ({procent * 100}% of CurrentHP)");
                
                var owner = (Owner as Warrior);
                owner.HP += (int)(target.HP * procent / 2);                
                CardGameEngine.WriteLog($"and {owner.Title} (+{(int)(target.HP * 0.35) / 2}) (17% of CurrentHP enemy)");
            }            
        }
    }
}
