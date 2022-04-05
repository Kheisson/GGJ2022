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
        [SerializeField] private GameObject priceText;
        [SerializeField] private Image cosmeticIconPlacement;
        #endregion

        #region Methods

        private void OnEnable()
        {
            cosmeticIconPlacement.sprite = shopItem.ItemImage;
            if (shopItem.ItemUnlocked)
            {
                equipButton.SetActive(true);
                return;
            }
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
                shopItem.ItemUnlocked = true;
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