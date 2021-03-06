using System;
using Core;
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

        //Stops gameplay and performs a tween to enlarge rect scale from 0 to 1
        private void OnEnable()
        {
            Time.timeScale = 1f;
            var rect = GetComponent<RectTransform>();
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, inDuration).SetEase(inEase).OnComplete(() =>
            {
                Time.timeScale = 0f;
            });
        }
        //Shirks back the rect to 0
        protected virtual void OnPopupClosedEvent()
        {
            var rect = GetComponent<RectTransform>();
            Time.timeScale = 1f;
            rect.DOScale(Vector3.zero, outDuration).SetEase(outEase).OnComplete(() =>
            {
                PopupClosedEvent?.Invoke();
            });
            
        }

        protected void LoadToMap()
        {
            GameManager.Instance.LoadMap();
        }
    }
}