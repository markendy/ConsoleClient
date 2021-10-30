using System;
using System.Collections.Generic;
using ConsoleClient.CardGame.Cards;
using ConsoleClient.CardGame.Cards.Primitives;
using ConsoleClient.CardGame.Cards.Warriors;
using ConsoleClient.CardGame.Common.Primitives;
using ConsoleClient.CardGame.Scenes;


namespace ConsoleClient.CardGame
{
    public class CardGameEngine
    {
        public static Random Random { get; } = new Random();

        public CardGameEngine()
        {
            Scenes.Add(new Battle());
        }


        public int CurrentSceneId { get; set; }

        public List<Scene> Scenes { get; } = new List<Scene>();




        public void Start()
        {
            LoadScene(0);
        }


        public void LoadScene(int id)
        {
            WriteLog("Start scene");

            CurrentSceneId = id;                       

            if (Scenes?[CurrentSceneId] is null)
            {
                WriteLog($"Loading scene {CurrentSceneId} is null");
                return;
            }

            // will get from outside
            Card[] tempFriend =
            {
                new Knight(Scenes[CurrentSceneId], 0, CardTag.Friend),
                new Knight(Scenes[CurrentSceneId], 1, CardTag.Friend),
                new Paladin(Scenes[CurrentSceneId], 2, CardTag.Friend)
            };

            Card[] tempEnemy =
            {
                new Necromant(Scenes[CurrentSceneId], 0, CardTag.Enemy),
                new Paladin(Scenes[CurrentSceneId], 1, CardTag.Enemy),
                new Paladin(Scenes[CurrentSceneId], 2, CardTag.Enemy)
            };

            Scenes[CurrentSceneId].Start(tempFriend, tempEnemy);
        }


        public static void WriteLog(LogTag logTag, string message)
        {
            if (BaseSettings.EngineSettings.LogFilter.Count == 0)
            {
                WriteLog(message, logTag);

                return;
            }

            if (BaseSettings.EngineSettings.LogFilter.Contains(logTag))
            {
                WriteLog(message, logTag);

                return;
            }
        }


        public static void WriteLog(string message)
        {
            Console.WriteLine(message);
            System.Diagnostics.Debug.WriteLine(message);
        }


        private static void WriteLog(string message, LogTag logTag)
        {
            if (logTag == LogTag.empty)
            {
                WriteLog(message);
                return;
            }

            Console.WriteLine($"[{logTag}]" + " " + message);
            System.Diagnostics.Debug.WriteLine($"[{logTag}]" + " " + message);
        }
    }    
}
