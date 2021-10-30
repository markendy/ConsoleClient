using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills.Damage;
using ConsoleClient.CardGame.Skills.Heals;


namespace ConsoleClient.CardGame.Cards.Warriors
{
    public class Knight : Warrior
    {
        public Knight(Scene scene) : base (scene)
        {
            MaxHP = 1750;
            HP = MaxHP;
            Damage = 250;
            Title = $"{nameof(Knight)}";

            CardGameEngine.WriteLog($"Card {Title} created with hp:{HP}, dmg: {Damage}");
        }



        protected override void LoadSkills()
        {
            Skills[0] = new BloodDragon(this);
            Skills[1] = new FullCombatReadiness(this);
        }
    }
}
