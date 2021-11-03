using ConsoleClient.CardGame.Cards.Primitives;
using System;


namespace ConsoleClient.CardGame.Cards.Interfaces
{
    public interface ILiveCard
    {
        public EventHandler<HpChangeEventArgs> BeforeHpChanged { get; set; }

        public EventHandler<HpChangeEventArgs> AfterHpChanged { get; set; }

        public EventHandler<HpChangeEventArgs> BeforeHpTaked { get; set; }

        public EventHandler<HpChangeEventArgs> AfterHpTaked { get; set; }

        public EventHandler<HpChangeEventArgs> BeforeHpGived { get; set; }

        public EventHandler<HpChangeEventArgs> AfterHpGived { get; set; }

        public int MaxHP { get; }

        public int HP { get; }

        public int BaseDamage { get; set; }

        public int AdditionalDamage { get; set; }

        public int CurrentDamage { get; }

        public double PhysicalResistance { get; set; }

        public double MagicResistance { get; set; }


        public virtual void AfterCreate()
        {

        }


        public void TakeHP(HpChangeEventArgs args);

        public void GiveHP(HpChangeEventArgs args);
    }
}
