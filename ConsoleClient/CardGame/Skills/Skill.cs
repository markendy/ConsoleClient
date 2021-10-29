using ConsoleClient.CardGame.Scenes;


namespace ConsoleClient.CardGame.Skills
{
    public interface ISkill
    {
        public int Id { get; set; }


        public void Init()
        {

        }


        public void Execute(Scene scenes);
    }
}
