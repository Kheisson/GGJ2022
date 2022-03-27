using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public abstract class Popup : MonoBehaviour
    {
        [SerializeField] private float inDuration = 0.25f;
        [SerializeField] private float outDuration = 0.15f;
        [SerializeField] private Ease inEase = Ease.InSine;
        [SerializeField] private Ease outEase = Ease.OutSine;
        public event Action PopupClosedEvent;

        private void OnEnable()
        {
            var rect = GetComponent<RectTransform>();
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, inDuration).SetEase(inEase).OnComplete(() =>
            {
                Time.timeScale = 0f;
            });
        }

        protected virtual void OnPopupClosedEvent()
        {
            var rect = GetComponent<RectTransform>();
            Time.timeScale = 1f;
            rect.DOScale(Vector3.zero, outDuration).SetEase(outEase).OnComplete(() =>
            {
                PopupClosedEvent?.Invoke();
            });
            
        }
    }
}