using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class GameController : IInitializable
    {
        private float lastTryTime;
        private float bestTime = -1f;

        public void Initialize()
        {
        }

        public void Reset()
        {
            lastTryTime = Time.realtimeSinceStartup;
        }

        public float GetCurrentTime()
        {
            return Time.realtimeSinceStartup - lastTryTime;
        }

        public bool CheckHighScore()
        {
            if (bestTime == -1f || GetCurrentTime() < bestTime)
            {
                bestTime = GetCurrentTime();
                return true;
            }

            return false;
        }

        public float GetBestTime()
        {
            return bestTime;
        }
    }
}