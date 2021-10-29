using System.Collections.Generic;
using ConsoleClient.CardGame.Scenes;


namespace ConsoleClient.CardGame
{
    public class CardGameEngine
    {
        public CardGameEngine()
        { }


        public int CurrentSceneId { get; set; }

        public List<Scene>Scenes { get; set; }



        public void Start()
        {
            LoadScene(0);
        }


        public void LoadScene(int id)
        {
            CurrentSceneId = id;

            Scenes[CurrentSceneId].Start();
        }
    }    
}
