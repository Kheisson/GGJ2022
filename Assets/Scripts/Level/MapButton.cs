using System.Collections.Generic;
using AirFishLab.Fixes;
using AirFishLab.ScrollingList;
using Core;
using UnityEngine;

namespace Level
{
    public class MapButton : MonoBehaviour
    {
        [SerializeField] private CircularScrollingList circularScrollingList;
        private List<IntListBox> _listBoxes = new List<IntListBox>();

        private void Start()
        {
            Init();
            InvokeRepeating(nameof(UpdateButtonStates), 0.1f, 0.1f);
        }

        //Affected by dynamic int thats passed from the button
        public void LoadLevel(int level)
        {
            var levelNumber = (int) circularScrollingList.listBank.GetListContent(level);
            GameManager.Instance.LoadLevel(levelNumber.ToString());
            print($"Loading scene: {levelNumber.ToString()}");
        }

        //Restores time scale to flow and caches the listBank 
        private void Init()
        {
            Time.timeScale = 1f;
            if (_listBoxes.Count != 0) return;
            foreach (var box in circularScrollingList.listBank.GetComponentsInChildren<IntListBox>())
            {
                _listBoxes.Add(box);
            }
        }
        
        //Runs every 1.0 seconds and updates the button state of the level selection buttons
        private void UpdateButtonStates()
        {
            foreach (var box in _listBoxes)
            {
                var mode = GameManager.Instance.GetClearedLevel(int.Parse(box.Text));
                box.TurnButton(mode);
            }
        }
    }
}