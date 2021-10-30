using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Interfaces;
using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Common.Primitives;
using ConsoleClient.CardGame.Scenes;


namespace ConsoleClient.CardGame.Skills.Heals
{
    public class HolyHand : BaseSkill
    {
        public HolyHand(Card owner)
        {
            Owner = owner;
            Title = nameof(HolyHand);
        }


        /// <summary>
        /// Heal hero, which has minimum hp in 100+10% of MaxHP, and damge enemy
        /// </summary>
        public override void Execute()
        {
            Warrior owner = Owner as Warrior;
            int minHPCardId = owner.InBoardId;
            int mainValue = 100;
            double procent = 0.1;
            int addValue = (int)(owner.MaxHP * procent);
            int endValue = mainValue + addValue;
            Battle battleScene = Owner.CurrentScene as Battle;

            FindIdWithMinHP(battleScene, ref minHPCardId, CardTag.Friend);

            ILiveCard friend = battleScene.GetCard(minHPCardId, Owner.Tag) as ILiveCard;
            
            if (friend is null)
                return;
            
            CardGameEngine.WriteLog(LogTag.skill, $"{Owner.Title}::{Title}> " +
                $"{(friend as IDescribed).Title} (+{endValue}) (+{mainValue}+{procent * 100}% of MaxHP)");
            friend.GiveHP(new HpChangeEventArgs(this, endValue));

            FindIdWithMinHP(battleScene, ref minHPCardId, CardTag.Enemy);
            ILiveCard enemy = battleScene.GetCard(minHPCardId, Owner.EnemyTag) as ILiveCard;
            
            if (enemy is null)
                return;
            
            CardGameEngine.WriteLog(LogTag.skill, $"{Owner.Title}::{Title}> " +
                $"{(enemy as IDescribed).Title} (-{endValue}) (-{mainValue}+{procent * 100}% of MaxHP)");
            enemy.TakeHP(new HpChangeEventArgs(this, endValue));
        }


        private void FindIdWithMinHP(Battle scene, ref int minHPCardId, CardTag tag)
        {
            foreach (Warrior target in scene.GetAllEnemyWarriors(tag))
            {
                if (target is null)
                    continue;

                if (target.HP < (scene.GetCard(minHPCardId, Owner.Tag) as ILiveCard).HP)
                {
                    minHPCardId = target.InBoardId;
                }
            }
        }
    }
}
