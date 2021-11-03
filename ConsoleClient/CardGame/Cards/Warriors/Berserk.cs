using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills.Damage;
using ConsoleClient.CardGame.Skills.Resistance;


namespace ConsoleClient.CardGame.Cards.Warriors
{
    public class Berserk : Warrior
    {
        public Berserk(Scene scene, int inBoardId, CardTag cardTag) : base (scene, inBoardId, cardTag)
        {
            MaxHP = 2500;            
            BaseDamage = 70;

            AfterForChildCreate();
        }



        protected override void LoadSkills()
        {
            Skills[0] = new BerserkersBlood(this);
            Skills[1] = new PainBall(this);
            Skills[2] = new HeartOfStone(this);
        }
    }
}
