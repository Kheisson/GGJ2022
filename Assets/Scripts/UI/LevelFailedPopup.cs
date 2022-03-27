using Core;

namespace UI
{
    public class LevelFailedPopup : Popup
    {
        #region Methods

        private void Hide()
        {
            OnPopupClosedEvent();
        }

        public void RetryButton()
        {
            GameManager.Instance.ReloadLevel();
        }

        public void AbortMission()
        {
            //TODO: Move Player to map scene
        }

        #endregion
    }
}