using DG.Tweening;
using Spawn;

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
            horizontalSpeed = speed;
            orderOfExecutionDelay = delay;
            this.flip = flip;
            Invoke(nameof(TweenMovement), orderOfExecutionDelay);
        }

        private void TweenMovement()
        {
            var mostLeftPosition = SpawnGrid.GetSpot(0);
            transform.DOMoveX(mostLeftPosition.x, horizontalSpeed).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                var mostRightPosition = SpawnGrid.GetSpot(4);
                transform.DOMoveX(mostRightPosition.x, horizontalSpeed).SetEase(Ease.InOutSine).SetDelay(2f);
            });

        }
    }
}