using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Common.Primitives;
using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills.Damage;
using ConsoleClient.CardGame.Skills.Heals;


namespace ConsoleClient.CardGame.Cards.Warriors
{
    public class Knight : Warrior
    {
        public Knight(Scene scene, int inBoardId, CardTag cardTag) : base (scene, inBoardId, cardTag)
        {
            MaxHP = 1550;
            GiveHP(new HpChangeEventArgs(MaxHP));
            BaseDamage = 250;

            AfterForChildCreate();
        }



        protected override void LoadSkills()
        {
            Skills[0] = new BloodDragon(this);
            Skills[1] = new FullCombatReadiness(this);
        }
    }
}
