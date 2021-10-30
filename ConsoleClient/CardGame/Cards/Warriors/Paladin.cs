using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Common.Primitives;
using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills.Heals;


namespace ConsoleClient.CardGame.Cards.Warriors
{
    public class Paladin : Warrior
    {
        public Paladin(Scene scene, int inBoardId, CardTag cardTag) : base (scene, inBoardId, cardTag)
        {
            MaxHP = 2200;
            GiveHP(new HpChangeEventArgs(MaxHP));
            BaseDamage = 150;

            AfterForChildCreate();
        }



        protected override void LoadSkills()
        {
            Skills[0] = new HolyHand(this);
        }
    }
}
