using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsPopup : Popup
    {
        
        [SerializeField] private Sprite enabledButtonSprite;
        [SerializeField] private Sprite disabledButtonSprite;
        [SerializeField] private Image targetImageOfButton;
        public void Hide()
        {
            OnPopupClosedEvent();
        }

        public void Mute()
        {
            var mode = GameManager.Mute();
            targetImageOfButton.sprite = mode ? disabledButtonSprite : enabledButtonSprite;
        }

        public void AbortMission()
        {
            LoadToMap();
        }

        private void Awake()
        {
            targetImageOfButton.sprite = GameManager.IsAudioMuted ? disabledButtonSprite : enabledButtonSprite;
        }
    }
}