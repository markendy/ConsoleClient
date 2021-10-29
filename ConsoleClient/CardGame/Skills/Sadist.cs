using ConsoleClient.CardGame.Heroes;
using ConsoleClient.CardGame.Scenes;


namespace ConsoleClient.CardGame.Skills
{
    public class Sadist : ISkill
    {
        public Sadist()
        {            
        }


        public int Id { get; set; }


        /// <summary>
        /// Sadist damage all enemy in 5% of HP
        /// </summary>
        /// <param name="scenes">battle scene</param>
        public void Execute(Scene scene)
        {
            foreach(Hero target in (scene as Battle).AllCards)
            {
                target.HP -= (int)(target.HP * 0.05);
            }
        }
    }
}
