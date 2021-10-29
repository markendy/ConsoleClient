namespace ConsoleClient
{
    public class Program
    {     
        public static void Main(string[] args)
        {
            //CommandLineHandler.Start();
            var game = new CardGame.CardGameEngine();
            game.Start();
        }
    }
}
