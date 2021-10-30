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
                CardGameEngine.WriteLog($"[skill] {Owner.Title} executed {Title}: {target.Title} was -dmg- (-{(int)(target.HP * procent)}) ({procent * 100}% of CurrentHP)");
                
                var owner = (Owner as Warrior);
                owner.HP += (int)(target.HP * procent);                
                CardGameEngine.WriteLog($"and {target.Title} give this hp self: (+{(int)(target.HP * 0.35)}) (35% of CurrentHP enemy)");
            }            
        }
    }
}
