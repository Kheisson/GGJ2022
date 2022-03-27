using Core;

namespace UI
{
    public class LevelFailedPopup : Popup
    {
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
    }
}