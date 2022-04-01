using Core;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Level
{
    public class MapButton : MonoBehaviour
    {
        public void LoadLevel()
        {
            var eventSystem = FindObjectOfType<EventSystem>();
            var levelName = eventSystem.currentSelectedGameObject.gameObject.GetComponentInChildren<TextMeshProUGUI>().text;
            GameManager.Instance.LoadLevel(levelName);
        }
    }
}