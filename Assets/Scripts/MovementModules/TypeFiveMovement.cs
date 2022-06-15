using System.Collections;
using Spawn;
using UnityEngine;

namespace MovementModules
{
    public class TypeFiveMovement : MovingEnemy
    {
        private void Awake()
        {
            orderOfExecutionDelay = 2.5f;
            horizontalSpeed = 0.1f;
        }

        public override void StartMoving()
        {
            StartCoroutine(nameof(StartMovingCoroutine));
        }

        private IEnumerator StartMovingCoroutine()
        {
            yield return new WaitForSeconds(orderOfExecutionDelay);
            var middlePosition = SpawnGrid.GetSpot(2);
            while (transform.position.x > middlePosition.x)
            {
                var holderPosition = new Vector3
                {
                    x = Mathf.Lerp(transform.position.x, middlePosition.x, horizontalSpeed),
                    y = transform.position.y,
                    z = transform.position.z
                };
                transform.position = holderPosition;
                
                yield return new WaitForEndOfFrame();
                if (Mathf.Approximately(transform.position.x, middlePosition.x))
                    break;
            }

            StartCoroutine(nameof(MoveLeft));
        }

        private IEnumerator MoveLeft()
        {
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
        }
    }
}