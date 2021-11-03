using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Common.Primitives;


namespace ConsoleClient.CardGame.Skills
{
    public abstract class BaseSkill : IDescribed
    {
        public const string NullSkillTitle = "NULL SKILL TITLE";


        public BaseSkill(Card owner)
        {
            Owner = owner;
            Title = GetType().Name;
        }


        public int Id { get; set; }

        public string Title { get; set; } = NullSkillTitle;

        public string InnerTitle => "Skill:" + Title;

        public string Description { get; set; } = string.Empty;

        public Card Owner { get; init; }


        public void Init()
        {

        }


        public abstract void Execute();
    }
}
