using System;

namespace Ddb.Domain.Services
{
    public class DiceRollService
    {
        private readonly Random _rand;
        private static DiceRollService _instance;

        public DiceRollService(int? seed = null)
        {
            // Optional seed is used in unit testing for deterministic results
            _rand = seed != null ? new Random((int)seed) : new Random();
        }

        // Using a Singleton pattern because the Random class should not be instantiated in quick succession: 
        // https://docs.microsoft.com/en-us/dotnet/api/system.random?view=netcore-3.1#instantiating-the-random-number-generator
        public static DiceRollService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DiceRollService();
            }
            return _instance;
        }

        public int RollDice(int sides, int howMany = 1, int modifier = 0)
        {
            int roll = 0;
            for (int i = 0; i < howMany; i++)
            {
                roll += _rand.Next(1, sides + 1);
            }
            roll += modifier;
            return roll;
        }

        public int RollAverageOrBetter(int sides)
        {
            // Special dice roll function for calculating HP.
            // Returns the dice's max / 2 + 1, or better if a better roll was rolled.
            int min = sides / 2 + 1;
            int roll = RollDice(sides);
            return roll > min ? roll : min;
        }


    }
}
