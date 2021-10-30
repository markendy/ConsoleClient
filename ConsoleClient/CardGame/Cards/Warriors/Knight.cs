using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills.Heals;


namespace ConsoleClient.CardGame.Cards.Warriors
{
    public class Knight : Warrior
    {
        public Knight(Scene scene) : base (scene)
        {
            MaxHP = 250;
            HP = MaxHP;
            Damage = 45;
            Title = $"{nameof(Knight)}";

            CardGameEngine.WriteLog($"Card {Title} created with hp:{HP}, dmg: {Damage}");
        }



        protected override void LoadSkills()
        {
            Skills[0] = new BloodDragon(this);
        }
    }
}
