using AirFishLab.ScrollingList;
using Core;
using UnityEngine;

namespace Level
{
    public class MapButton : MonoBehaviour
    {
        [SerializeField] private CircularScrollingList _circularScrollingList;

        public void LoadLevel(int level)
        {
            var levelNumber = (int) _circularScrollingList.listBank.GetListContent(level);
            GameManager.Instance.LoadLevel(levelNumber.ToString());
            print($"Loading scene: {levelNumber.ToString()}");
        }
    }
}