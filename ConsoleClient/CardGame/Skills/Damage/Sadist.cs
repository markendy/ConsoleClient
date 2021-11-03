using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Common.Primitives;
using ConsoleClient.CardGame.Scenes;
using System;


namespace ConsoleClient.CardGame.Skills.Damage
{
    public class Sadist : BaseSkill
    {
        public Sadist(Card owner) : base(owner)
        {
        }


        /// <summary>
        /// Sadist give self hp all enemy 15% of MaxHP
        /// </summary>        
        public override void Execute()
        {
            foreach(Warrior target in (Owner.CurrentScene as Battle).GetAllEnemyWarriors(Owner.EnemyTag))
            {
                if (target is null)
                    continue;

                double procent = 0.15;
                
                int addValue = (int)(target.HP * procent);
                double miniProcent = Math.Round(procent * 0.75, 2);
                int miniAddValue = (int)(target.HP * miniProcent);

                CardGameEngine.WriteLog(LogTag.skill, $"{Owner.Title}::{Title}> " +
                    $"{target.Title} ({target.HP}-{addValue}) ({procent * 100}% of CurrentHP)");
                target.TakeHP(new HpChangeEventArgs(this, addValue));
                target.MaxHP -= (int)(addValue * 0.25);

                var owner = (Owner as Warrior);                              
                CardGameEngine.WriteLog(LogTag.empty, 
                    $"and {owner.Title} ({owner.HP}+{miniAddValue}) ({miniProcent * 100}% of CurrentHP enemy)");
                owner.MaxHP += (int)(miniAddValue * 0.5);
                owner.GiveHP(new HpChangeEventArgs(this, miniAddValue));                
            }            
        }
    }
}
