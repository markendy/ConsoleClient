using ConsoleClient.CardGame.Common.Primitives;
using System.Collections.Generic;


namespace ConsoleClient.CardGame
{
    public static class BaseSettings
    {
        public static class GameSettings
        {
            public const int MaxCardCount = 14;

            public const int BaseMaxHP = 100;                        

            public const int BaseDamage = 25;
        }


        public static class EngineSettings
        {
            public static List<LogTag> LogFilter = new List<LogTag>();
        }
    }
}
