using UnityEngine;
using Zenject;

namespace DefaultNamespace
{
    public class GameController : IInitializable
    {
        private float lastTryTime;

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
    }
}