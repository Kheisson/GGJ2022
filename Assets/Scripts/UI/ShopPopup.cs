using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopPopup : Popup
    {
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private GameObject cockpitDeals;
        [SerializeField] private GameObject wingDeals;
        [SerializeField] private GameObject tailDeals;
        
        //Sets the rect content to the cockpit parent and disables the others
        public void ShowCockpitParts(RectTransform rectTransform)
        {
            if (scrollRect.content == rectTransform)
                return;
            scrollRect.content = rectTransform;
            wingDeals.SetActive(false);
            tailDeals.SetActive(false);
            cockpitDeals.SetActive(true);
        }
        //Sets the rect content to the wings parent and disables the others
        public void ShowWingParts(RectTransform rectTransform)
        {
            if (scrollRect.content == rectTransform)
                return;
            scrollRect.content = rectTransform;
            cockpitDeals.SetActive(false);
            tailDeals.SetActive(false);
            wingDeals.SetActive(true);
        }
        //Sets the rect content to the tail parent and disables the others
        public void ShowTailParts(RectTransform rectTransform)
        {
            if (scrollRect.content == rectTransform)
                return;
            scrollRect.content = rectTransform;
            wingDeals.SetActive(false);
            cockpitDeals.SetActive(false);
            tailDeals.SetActive(true);
        }
        //Closes the popup
        public void Hide()
        {
            OnPopupClosedEvent();
        }
    }
}