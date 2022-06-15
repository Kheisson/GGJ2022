using System;
using System.Collections;
using Spawn;
using UnityEngine;

namespace MovementModules
{
    public class TypeSixMovement : MovingEnemy
    {
        private void Awake()
        {
            orderOfExecutionDelay = 3f;
            horizontalSpeed = 0.05f;
        }

        public override void StartMoving()
        {
            StartCoroutine(nameof(StartMovingCoroutine));
        }
        private IEnumerator StartMovingCoroutine()
        {
            yield return new WaitForSeconds(orderOfExecutionDelay);
            var mostLeftPosition = SpawnGrid.GetSpot(0);
            while (transform.position.x > mostLeftPosition.x)
            {
                var holderPosition = new Vector3
                {
                    x = Mathf.Lerp(transform.position.x, mostLeftPosition.x, horizontalSpeed),
                    y = transform.position.y - 1f,
                    z = transform.position.z
                };
                transform.position = holderPosition;
                
                yield return new WaitForEndOfFrame();
                if (Mathf.Approximately(transform.position.x, mostLeftPosition.x))
                    break;
            }
        }
    }
}