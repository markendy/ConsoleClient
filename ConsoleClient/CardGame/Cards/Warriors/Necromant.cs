﻿using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills.Damage;


namespace ConsoleClient.CardGame.Cards.Warriors
{
    public class Necromant : Warrior
    {
        public Necromant(Scene scene, int inBoardId, CardTag cardTag) : base(scene, inBoardId, cardTag)
        {
            MaxHP = 1240;
            GiveHP(new HpChangeEventArgs(MaxHP));
            BaseDamage = 275;
            Title = $"{nameof(Necromant)}";

            CardGameEngine.WriteLog($"Card {Title} created with hp:{HP}, dmg: {CurrentDamage}");
        }


        protected override void LoadSkills()
        {
            Skills[0] =  new Sadist(this);
        }
    }
}
