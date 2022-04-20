using Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItem : MonoBehaviour
    {
        #region Fields
        [SerializeField] private ShopItemData shopItem;
        [SerializeField] private GameObject buyButton;
        [SerializeField] private GameObject equipButton;
        [SerializeField] private GameObject selectionFrame;
        [SerializeField] private GameObject priceText;
        [SerializeField] private Image cosmeticIconPlacement;
        #endregion

        #region Methods

        private void OnEnable()
        {
            cosmeticIconPlacement.sprite = shopItem.ItemImage;
            if (PlayerPrefs.GetInt(shopItem.name, 0) == 1 || shopItem.name.Contains("0"))
            {
                equipButton.SetActive(true);
                buyButton.SetActive(false);
                priceText.SetActive(false);
            }
            else
            {
                buyButton.SetActive(true);
                priceText.SetActive(true);
                GetComponentInChildren<TextMeshProUGUI>().text = shopItem.ItemCost.ToString();
            }

            var body = DataManager.GetEquippedItem(shopItem.ItemType);
            if(body == shopItem.ItemId)
                SetSelected(true);
        }

        //Checks if there is enough credit, if there is then unlocks data and updates buttons (visually)
        public void Buy()
        {
            var wallet = DataManager.GetPlayerBalance();
            if (wallet - shopItem.ItemCost > 0)
            {
                wallet -= shopItem.ItemCost;
                PlayerPrefs.SetInt(shopItem.name, 1);
                buyButton.SetActive(false);
                priceText.SetActive(false);
                DataManager.SaveOnPurchase(wallet);
                equipButton.SetActive(true);
            }
            else
            {
                Debug.Log("Not enough credits");
            }
        }

        //Saves the equipped data in data.json
        public void Equip()
        {
            var itemID = DataManager.GetEquippedItem(shopItem.ItemType);
            if (itemID == shopItem.ItemId)
                return;
            DataManager.SaveOnEquip(shopItem.ItemType, shopItem.ItemId);
            var shopItems = GameObject.FindGameObjectsWithTag("ShopItem");
            foreach (var item in shopItems)
            {
                item.GetComponent<ShopItem>().SetSelected(false);
            }
            SetSelected(true);
        }

        private void SetSelected(bool isSelected)
        {
            selectionFrame.SetActive(isSelected);
        }

        #endregion
            
    }
}