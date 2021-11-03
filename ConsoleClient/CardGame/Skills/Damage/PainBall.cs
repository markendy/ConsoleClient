using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Interfaces;
using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Common.Primitives;
using ConsoleClient.CardGame.Scenes;


namespace ConsoleClient.CardGame.Skills.Damage
{
    public class PainBall : BaseSkill
    {
        private int _inputDamageSum;


        public PainBall(Card owner) : base(owner)
        {
            (Owner as ILiveCard).AfterHpTaked += SumInputDamage;
        }


        /// <summary>
        /// Return all pain, what take to target
        /// </summary>        
        public override void Execute()
        {
            if (_inputDamageSum == 0)
                return;

            Battle battleScene = Owner.CurrentScene as Battle;

            if (battleScene.GetCard(Owner.InBoardId, Owner.EnemyTag) is not ILiveCard target)
                return;

            CardGameEngine.WriteLog(LogTag.skill, $"{Owner.Title}::{Title}> " +
                    $"{(target as IDescribed).Title} ({target.HP}-{_inputDamageSum})");
            target.TakeHP(new HpChangeEventArgs(this, _inputDamageSum));
            _inputDamageSum = 0;
        }


        protected void SumInputDamage(object sender, HpChangeEventArgs args)
        {
            _inputDamageSum += args.Value;
        }
    }
}
