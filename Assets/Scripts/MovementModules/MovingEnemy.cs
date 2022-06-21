using System.Collections;
using UnityEngine;

namespace MovementModules
{
    public abstract class MovingEnemy : MonoBehaviour
    {
        public float orderOfExecutionDelay;
        public float horizontalSpeed;
        public bool flip;
        public abstract void StartMoving(float delay, float speed, bool flip);

    }
}