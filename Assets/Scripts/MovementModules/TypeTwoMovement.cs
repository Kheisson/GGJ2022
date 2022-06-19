using System.Collections;
using DG.Tweening;
using Spawn;
using UnityEngine;

namespace MovementModules
{
    public class TypeTwoMovement : MovingEnemy
    {
        private void Awake()
        {
            orderOfExecutionDelay = 3f;
            horizontalSpeed = 1f;
        }

        public override void StartMoving()
        {
            //StartCoroutine(nameof(StartMovingCoroutine));
            Invoke(nameof(TweenMovement), 3f);
        }

        private IEnumerator StartMovingCoroutine()
        {
            yield return new WaitForSeconds(orderOfExecutionDelay);
            var mostLeftPosition = SpawnGrid.GetSpot(0);
            while (transform.position.x > mostLeftPosition.x)
            {
                
                var holderPosition = new Vector3
                {
                    x = Mathf.MoveTowards(transform.position.x, mostLeftPosition.x, horizontalSpeed),
                    y = transform.position.y,
                    z = transform.position.z
                };
                transform.position = holderPosition;
                
                yield return new WaitForEndOfFrame();
                if (Mathf.Approximately(transform.position.x, mostLeftPosition.x))
                    break;
            }
        }

        private void TweenMovement()
        {
            var mostLeftPosition = SpawnGrid.GetSpot(0);
            transform.DOMoveX(mostLeftPosition.x, horizontalSpeed).SetEase(Ease.InOutSine);
        }
    }
}