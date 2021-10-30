using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Common.Primitives;
using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills;


namespace ConsoleClient.CardGame.Cards
{
    public abstract class Card : IDescribed
    {
        public const string NullCardTitle = "NULL CARD TITLE";

        public Card(Scene scene, int inBoardId, CardType cardType, CardTag cartTag)
        {   
            IsLive = true;
            CurrentScene = scene;
            Type = cardType;
            Tag = cartTag;
            InBoardId = inBoardId;
        }


        public int Id { get; set; }

        public int InBoardId { get; set; }

        public int MaxSkillCount { get; set; }

        public BaseSkill[] Skills { get; set; }

        public Scene CurrentScene { get; private set; }

        public CardType Type { get; set; }

        public CardTag Tag { get; set; }

        public CardTag EnemyTag => Tag == CardTag.Friend ? CardTag.Enemy : CardTag.Friend;

        public bool IsLive { get; set; }

        public bool IsActive { get; set; }

        public string Title { get; set; } = NullCardTitle;

        public string InnerTitle => "Card:" + Title;

        public string Description { get; set; } = string.Empty;


        public virtual void MakeStep()
        {
            CardGameEngine.WriteLog($"___________________________________________\n\tCard {Title} make step\n___________________________________________");

            ExecuteSkills();            
            if (Type == CardType.Warrior)            
                Attack();
            // do something...
        }


        protected virtual void ExecuteSkills()
        {           
            foreach (var skill in Skills)
            {
                if (skill is null)
                    continue;

                skill.Execute();                               
            }
        }


        protected abstract void LoadSkills();


        protected abstract void Attack();
    }
}
