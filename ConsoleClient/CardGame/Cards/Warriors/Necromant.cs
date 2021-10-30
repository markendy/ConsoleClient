using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills.Damage;


namespace ConsoleClient.CardGame.Cards.Warriors
{
    public class Necromant : Warrior
    {
        public Necromant(Scene scene) : base(scene)
        {
            MaxHP = 1240;
            HP = MaxHP;
            Damage = 275;
            Title = $"{nameof(Necromant)}";

            CardGameEngine.WriteLog($"Card {Title} created with hp:{HP}, dmg: {Damage}");
        }


        protected override void LoadSkills()
        {
            Skills[0] =  new Sadist(this);
        }
    }
}
