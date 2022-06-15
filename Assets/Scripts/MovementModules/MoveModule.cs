using System;
using Enemies;
using UnityEngine;

namespace MovementModules
{
    public static class MoveModule
    {
        public static MovingEnemy AddMovingModule(EnemyType type, Transform parent)
        {
            MovingEnemy moveModule = null;
            switch (type)
            {
                case EnemyType.Type1:
                    break;
                case EnemyType.Type2:
                    moveModule = parent.gameObject.AddComponent<TypeTwoMovement>();
                    break;
                case EnemyType.Type3:
                    moveModule = parent.gameObject.AddComponent<TypeThreeMovement>();
                    break;
                case EnemyType.Type4:
                    moveModule = parent.gameObject.AddComponent<TypeFourMovement>();
                    break;
                case EnemyType.Type5:
                    moveModule = parent.gameObject.AddComponent<TypeFiveMovement>();
                    break;
                case EnemyType.Type6:
                    moveModule = parent.gameObject.AddComponent<TypeSixMovement>();
                    break;
                case EnemyType.Type7:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, $"{type} is out of scope of enemy");
            }

            return moveModule;
        }
    }
}