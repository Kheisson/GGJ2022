using UnityEngine;

namespace UI
{
    public class PopupManager : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject blackCover;
        private Popup _popupInstance;

        #endregion

        #region Methods

        public void ShowPopup(Popup popup)
        {
            if (_popupInstance == null)
            {
                var popupGameObject = Instantiate(popup, transform);
                _popupInstance = popupGameObject.GetComponent<Popup>();
                _popupInstance.PopupClosedEvent += HidePopup;
            }

            Time.timeScale = 0f;
            blackCover.SetActive(true);
        }

        private void HidePopup()
        {
            _popupInstance.PopupClosedEvent -= HidePopup;
            Destroy(_popupInstance.gameObject);
            Time.timeScale = 1f;
            blackCover.SetActive(false);
        }
        
        #endregion
    }
}