using System;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Popup : MonoBehaviour
    {
        public event Action PopupClosedEvent;

        [SerializeField] private Sprite enabledButtonSprite;
        [SerializeField] private Sprite disabledButtonSprite;
        [SerializeField] private Image targetImageOfButton;
        public void Hide()
        {
            PopupClosedEvent?.Invoke();
        }

        public void Mute()
        {
            var mode = GameManager.Mute();
            targetImageOfButton.sprite = mode ? disabledButtonSprite : enabledButtonSprite;
        }

        private void Awake()
        {
            targetImageOfButton.sprite = GameManager.IsAudioMuted ? disabledButtonSprite : enabledButtonSprite;
        }
    }
}