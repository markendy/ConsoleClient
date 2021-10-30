using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Primitives;
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
            foreach(Warrior target in (Owner.CurrentScene as Battle).GetAllEnemyWarriors(Owner.EnemyTag))
            {
                if (target is null)
                    continue;

                double procent = 0.35;
                
                int addValue = (int)(target.HP * procent);
                CardGameEngine.WriteLog($"[skill] {Owner.Title}::{Title}> {target.Title} ({target.HP}-{addValue}) ({procent * 100}% of CurrentHP)");
                target.TakeHP(new HpChangeEventArgs(this, addValue));
                
                var owner = (Owner as Warrior);                              
                CardGameEngine.WriteLog($"and {owner.Title} ({owner.HP}+{addValue}) ({procent * 100}% of CurrentHP enemy)");
                owner.GiveHP(new HpChangeEventArgs(this, addValue));                
            }            
        }
    }
}
