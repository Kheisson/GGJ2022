using System.Collections.Generic;
using Enemies;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "New Stage", menuName = "Scriptables/New Stage"), HelpURL("https://media.discordapp.net/attachments/943219906059571273/970390327255007263/Enemy-Types.png?width=1080&height=283")]
    public class LevelProgressionSo : ScriptableObject
    {
        [SerializeField] private List<SpawnableUnit> units;

        public List<SpawnableUnit> SpawnableUnits => units;
    }
}