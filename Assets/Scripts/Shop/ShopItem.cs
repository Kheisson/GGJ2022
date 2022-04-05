using Save;
using TMPro;
using UnityEngine;

namespace Shop
{
    public class ShopItem : MonoBehaviour
    {
        #region Fields
        [SerializeField] private ShopItemData shopItem;
        [SerializeField] private GameObject buyButton;
        [SerializeField] private GameObject equipButton;
        [SerializeField] private GameObject priceText;
        #endregion

        #region Methods

        private void OnEnable()
        {
            if (shopItem.ItemUnlocked) return;
            buyButton.SetActive(true);
            priceText.SetActive(true);
            GetComponentInChildren<TextMeshProUGUI>().text = shopItem.ItemCost.ToString();
        }

        public void Buy()
        {
            var wallet = DataManager.GetPlayerBalance();
            if (wallet - shopItem.ItemCost > 0)
            {
                wallet -= shopItem.ItemCost;
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

        public void Equip()
        {
            var itemID = DataManager.GetEquippedItem(shopItem.ItemType);
            if (itemID == shopItem.ItemId)
                return;
            DataManager.SaveOnEquip(shopItem.ItemType, shopItem.ItemId);
        }

        #endregion
            
    }
}