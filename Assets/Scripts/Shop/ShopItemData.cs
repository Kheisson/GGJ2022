using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "New Shop Item", menuName = "Scriptables/New Shop Item")]
    public class ShopItemData : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private Sprite itemImage;
        [SerializeField] private bool unlocked;
        [SerializeField] private int cost;
        [SerializeField] private int id;
        [SerializeField] private ShopItemType type;

        public int ItemCost => cost;
        public string ItemName => itemName; 
        public bool ItemUnlocked
        {
            get => unlocked;
            set => unlocked = value;
        }
        public int ItemId => id;
        public Sprite ItemImage => itemImage;
        public ShopItemType ItemType => type;
    }
}