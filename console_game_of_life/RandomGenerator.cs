//Author : alessdangelo
//Date : 09/07/2021
//Description: Singleton class to generate a random number allowing to have only one instance of this class
using System;

namespace console_game_of_life
{
    /// <summary>
    /// Class generating a random number, using the singleton pattern
    /// </summary>
    public class RandomGenerator : Random
    {
        /// <summary>
        /// Random object
        /// </summary>
        private Random _random;

        /// <summary>
        /// Current and only instance of RandomGenerator
        /// </summary>
        private static RandomGenerator _instance;

        /// <summary>
        /// Singleton, instanciate the random object
        /// </summary>
        private RandomGenerator(int seed)
        {
            _random = new Random(seed);
        }

        /// <summary>
        /// Get current instance if exist, else instanciate it
        /// </summary>
        /// <returns>the singleton instance</returns>
        public static RandomGenerator GetInstance(int seed)
        {
            if (_instance == null)
            {
                _instance = new RandomGenerator(seed);
                return _instance;
            }
            return _instance;
        }
    }
}
