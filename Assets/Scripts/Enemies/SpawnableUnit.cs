using System;
using UnityEngine;

namespace Enemies
{
    [Serializable]
    public class SpawnableUnit
    {
        [Tooltip("Enemy Type to spawn, check the Help URL for details") ] public EnemyType enemyType;
        [Range(0,4) , Tooltip("Position selector - 0 is left most, 4 is right most")] public int positionOnGrid;
        [Tooltip("Delay between spawning")] public float delay;
        [Tooltip("Speed in which the object moves on the X-Axis")] public float horizontalSpeed;
        [Tooltip("Move Left-To-Right -->")] public bool flip;
        [Tooltip("At which point the unit starts moving")]public float screenSectionStartMoving;
    }

    
}