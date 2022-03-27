using UnityEngine;

namespace UI
{
    public class PopupManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject blackCover;
        private Popup _popup;

        #endregion

        #region Methods

        public void ShowPopup(Popup popup)
        {
            if (_popup == null)
            {
                var popupGameObject = Instantiate(popup, transform);
                _popup = popupGameObject.GetComponent<Popup>();
                _popup.PopupClosedEvent += HidePopup;
            }

            Time.timeScale = 0f;
            blackCover.SetActive(true);
        }

        private void HidePopup()
        {
            _popup.PopupClosedEvent -= HidePopup;
            Destroy(_popup.gameObject);
            Time.timeScale = 1f;
            blackCover.SetActive(false);
        }
        
        #endregion
    }
}