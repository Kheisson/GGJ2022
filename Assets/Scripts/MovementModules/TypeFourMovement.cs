using System.Collections;
using Spawn;
using UnityEngine;

namespace MovementModules
{
    public class TypeFourMovement : MovingEnemy
    {
        private void Awake()
        {
            orderOfExecutionDelay = 3f;
            horizontalSpeed = 0.1f;
        }

        public override void StartMoving(float delay, float speed, bool b)
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
                    y = transform.position.y,
                    z = transform.position.z
                };
                transform.position = holderPosition;

                yield return new WaitForEndOfFrame();
                if (Mathf.Approximately(transform.position.x, mostLeftPosition.x))
                    break;
            }
            StartCoroutine(nameof(MoveToTheRight));
        }

        private IEnumerator MoveToTheRight()
        {
            var mostRightPosition = SpawnGrid.GetSpot(4);
            
            while (transform.position.x < mostRightPosition.x)
            {
                var holderPosition = new Vector3
                {
                    x = Mathf.Lerp(transform.position.x, mostRightPosition.x, horizontalSpeed),
                    y = transform.position.y,
                    z = transform.position.z
                };
                transform.position = holderPosition;
                
                yield return new WaitForEndOfFrame();
                if (Mathf.Approximately(transform.position.x, mostRightPosition.x))
                    break;
            }
        }
    }
}