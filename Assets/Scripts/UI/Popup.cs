using System;
using UnityEngine;

namespace UI
{
    public abstract class Popup : MonoBehaviour
    {
        public event Action PopupClosedEvent;

        protected virtual void OnPopupClosedEvent()
        {
            PopupClosedEvent?.Invoke();
        }
    }
}