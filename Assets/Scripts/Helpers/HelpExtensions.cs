
using UnityEngine;

namespace Helpers
{
    public static class HelpExtensions
    {
        /// <summary>
        /// Used to get a success probability
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>A bool if successful or not</returns>
        public static bool ReturnSuccessfulProbability(this object obj) => Random.Range(0f, 1f) > 0.5f ? true : false;
        
        public static bool ReturnSuccessfulProbability(this object obj, float percent) => Random.Range(0f, 1f) > percent ? true : false;

    }
}