using DG.Tweening;
using Spawn;
using UnityEngine;

namespace MovementModules
{
    public class TypeTwoMovement : MovingEnemy
    {

        public override void StartMoving(float delay, float speed, bool flip)
        {
            horizontalSpeed = speed;
            orderOfExecutionDelay = delay;
            this.flip = flip;
            Invoke(nameof(TweenMovement), orderOfExecutionDelay);
        }
        
        private void TweenMovement()
        {
            var positionOnGrid = SpawnGrid.GetSpotBasedOnPosition(Mathf.Floor(transform.position.x));
            var place = 0;
            if (flip)
            {
                place = positionOnGrid + 2 >= 4 ? 4 : positionOnGrid + 2;
            }
            else
            {
                place = positionOnGrid - 2 < 0 ? 0 : positionOnGrid - 2;
            }
            var mostLeftPosition = SpawnGrid.GetSpot(place);
            transform.DOMoveX(mostLeftPosition.x, horizontalSpeed).SetEase(Ease.InOutSine);
        }
    }
}