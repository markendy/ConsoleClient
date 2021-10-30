using ConsoleClient.CardGame.Cards.Interfaces;
using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Common.Primitives;
using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills;
using System;


namespace ConsoleClient.CardGame.Cards
{
    public abstract class Warrior : Card, ILiveCard
    {
        private int _maxHP = BaseSettings.GameSettings.BaseMaxHP;
        private int _hp;
        private int _additionalDamage;
        private int _baseDamage = BaseSettings.GameSettings.BaseDamage;


        public Warrior(Scene scene, int inBoardId, CardTag cardTag) : base(scene, inBoardId, CardType.Warrior, cardTag)
        {
            MaxSkillCount = 3;
            Skills = new BaseSkill[MaxSkillCount];
            Title = $"{this.GetType().Name}";

            LoadSkills();
        }


        public int MaxHP
        {
            get => _maxHP;
            set
            {
                _maxHP = value;

                if (MaxHP < HP)
                {
                    HP = MaxHP;
                }
            }
        }


        public int HP
        {
            get => _hp;
            private set
            {
                int currentValue = Math.Max(0, Math.Min(MaxHP, value));
                
                _hp = currentValue;

                if (Title != Warrior.NullCardTitle)
                    CardGameEngine.WriteLog(LogTag.HP, $"{Title}[HP] = {HP}/{MaxHP}");                
            }
        }


        public int AdditionalDamage
        {
            get => _additionalDamage;
            set
            {
                _additionalDamage = Math.Max(value, 0);
            }
        }


        public int BaseDamage
        {
            get => _baseDamage;
            set
            {
                _baseDamage = Math.Max(value, 0);
            }
        }


        public int CurrentDamage => BaseDamage + AdditionalDamage;


        public void AfterForChildCreate()
        {
            CardGameEngine.WriteLog(LogTag.empty, $"Card {Title} created with hp:{HP}, dmg: {CurrentDamage}");
        }


        protected override void Attack()
        {
            Battle battleScence = CurrentScene as Battle;
            double error = 0.25;
            ILiveCard target = battleScence.GetCard(InBoardId, EnemyTag) as ILiveCard;

            if (target is null)
                return;

            target.TakeHP(new HpChangeEventArgs(this, 
                CardGameEngine.Random.Next((int)(CurrentDamage * (1 - error)), (int)(CurrentDamage * (1 + error)))));
        }


        public void TakeHP(HpChangeEventArgs args)
        {
            int value = HP - args.Value;
            IDescribed sender = args.Sender as IDescribed;

            if (Title != Warrior.NullCardTitle && sender is not null)
            {
                CardGameEngine.WriteLog(LogTag.damage, $"{sender.InnerTitle} -dmg-> {Title} ({HP}-{args.Value})");
            }            

            SetHP(new HpChangeEventArgs(args.Sender, value));
        }


        public void GiveHP(HpChangeEventArgs args)
        {
            int value = HP + args.Value;
            IDescribed sender = args.Sender as IDescribed;

            if (Title != Warrior.NullCardTitle && sender is not null)
            {
                CardGameEngine.WriteLog(LogTag.regen, $"{sender.InnerTitle} -rgn-> {Title} ({HP}+{args.Value})");
            }
            
            SetHP(new HpChangeEventArgs(args.Sender, value));
        }


        protected void SetHP(HpChangeEventArgs args)
        {
            if (args.Value <= 0)            
            {
                IsLive = false;

                IDescribed sender = args.Sender as IDescribed;
                CardGameEngine.WriteLog(LogTag.death, $"{sender.InnerTitle} -kill-> {Title}");

                (CurrentScene as Battle).RemoveCardFromCardList(InBoardId, Tag);
            }
            
            HP = args.Value;
        }
    }
}
