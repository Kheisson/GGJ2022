using System.Collections;
using UnityEngine;

namespace MovementModules
{
    public abstract class MovingEnemy : MonoBehaviour
    {
        public float orderOfExecutionDelay;
        public float horizontalSpeed;
        public abstract void StartMoving();

    }
}