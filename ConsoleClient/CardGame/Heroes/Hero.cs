namespace ConsoleClient.CardGame.Heroes
{
    public class Hero : Card
    {
        public Hero()
        {
        }


        public int HP { get; set; } = BaseSettings.GameSettings.BaseHP;

        public int Damage { get; set; } = BaseSettings.GameSettings.BaseDamage;        
    }
}
