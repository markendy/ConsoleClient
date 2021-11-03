namespace ConsoleClient
{
    public class Program
    {     
        public static void Main(string[] args)
        {
            //CommandLineHandler.Start();
            CardGame.CardGameEngine game = new CardGame.CardGameEngine();
            game.Start();
        }
    }
}
