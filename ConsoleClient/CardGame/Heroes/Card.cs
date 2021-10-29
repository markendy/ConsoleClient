using System;
using ConsoleClient.CardGame.Scenes;
using ConsoleClient.CardGame.Skills;


namespace ConsoleClient.CardGame.Heroes
{
    public abstract class Card
    {        
        public Card()
        {
            LoadSkills();
        }


        public int Id { get; set; }


        public int MaxSkillCount { get; set; } = 3;


        public ISkill[] Skills { get; private set; }



        public virtual void MakeStep(Scene scenes)
        {
            ExecuteSkills(scenes);
            // do nothing...
        }


        public virtual void ExecuteSkills(Scene scenes)
        {
            if (scenes is null)
            {
                throw new ArgumentNullException(nameof(scenes));
            }

            foreach (var skill in Skills)
            {
                skill.Execute(scenes);
            }
        }


        public virtual void LoadSkills()
        {
            throw new NotImplementedException();
        }
    }
}
