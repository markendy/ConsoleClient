using ConsoleClient.CardGame.Cards.Interfaces;
using ConsoleClient.CardGame.Cards.Warriors;
using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills;
using System;
using System.Linq;

namespace ConsoleClient.CardGame.Cards
{
    public abstract class Warrior : Card, ILiveCard
    {
        private int _maxHP = BaseSettings.GameSettings.BaseMaxHP;
        private int _hp = BaseSettings.GameSettings.BaseHP;
        private int _damage = BaseSettings.GameSettings.BaseDamage;

        public Warrior(Scene scene) : base(scene, Primitives.CardType.Warrior)
        {
            MaxSkillCount = 3;
            Skills = new BaseSkill[MaxSkillCount];

            LoadSkills();
        }


        public int MaxHP
        {
            get => _maxHP;
            set
            {
                _maxHP = value;
            }
        }


        public int HP
        {
            get => _hp;
            set
            {
                if (value <= 0)
                {
                    IsLive = false;
                    var ans = this is not Necromant ? (CurrentScene as Battle).FriendCards : (CurrentScene as Battle).EnemyCards;
                    int od = this is Necromant ? 1 : 0;
                    ans[InBoardId] = (CurrentScene as Battle).AllCards[od] = (CurrentScene as Battle).ExecuteCards[od] = null;
                }

                if (Title != Warrior.NullCardTitle)
                {
                    if (_hp > value)
                    {
                        var ans = this is Necromant ? (CurrentScene as Battle).FriendCards[InBoardId].Title : (CurrentScene as Battle).EnemyCards[InBoardId].Title;
                        CardGameEngine.WriteLog($"[damage] {ans} -dmg-> {Title} (-{_hp - value})");
                    }
                    else if (_hp < value)
                    {
                        CardGameEngine.WriteLog($"[regen] {Title} (+{value - _hp})");
                    }
                }
                
                _hp = value;

                if (Title != Warrior.NullCardTitle)
                    CardGameEngine.WriteLog($"[HP] {Title}[HP] = {_hp}");                
            }
        }

        public int Damage
        {
            get => _damage;
            set
            {
                _damage = Math.Max(value, 0);
            }
        }


        protected override void GiveDamage()
        {
            Battle battleScence = CurrentScene as Battle;
            
            ILiveCard target = (this is Necromant ? (CurrentScene as Battle).FriendCards[InBoardId] : (CurrentScene as Battle).EnemyCards[InBoardId]) as ILiveCard;

            target.HP -= CardGameEngine.Random.Next((int)(Damage * 0.75), (int)(Damage * 1.25));            
        }
    }
}
