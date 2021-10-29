using ConsoleClient.CardGame.Cards;


namespace ConsoleClient.CardGame.Skills
{
    public abstract class BaseSkill
    {
        public const string NullSkillTitle = "NULL SKILL TITLE";


        public int Id { get; set; }

        public string Title { get; set; } = NullSkillTitle;

        public string Description { get; set; } = string.Empty;

        public Card Owner { get; init; }


        public void Init()
        {

        }


        public abstract void Execute();
    }
}
