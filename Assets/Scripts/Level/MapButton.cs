using AirFishLab.Fixes;
using AirFishLab.ScrollingList;
using Core;
using UnityEngine;

namespace Level
{
    public class MapButton : MonoBehaviour
    {
        [SerializeField] private CircularScrollingList circularScrollingList;


        private void Start()
        {
            InvokeRepeating(nameof(UpdateButtonStates), 0.1f, 0.1f);
            Time.timeScale = 1f;
        }

        public void LoadLevel(int level)
        {
            var levelNumber = (int) circularScrollingList.listBank.GetListContent(level);
            GameManager.Instance.LoadLevel(levelNumber.ToString());
            print($"Loading scene: {levelNumber.ToString()}");
        }

        private void UpdateButtonStates()
        {
            foreach (var box in circularScrollingList.listBank.GetComponentsInChildren<IntListBox>())
            {
                var mode = GameManager.Instance.GetClearedLevel(int.Parse(box.Text));
                box.TurnButton(mode);
            }
        }
    }
}