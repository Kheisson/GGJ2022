using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "New Shop Item", menuName = "Scriptables/New Shop Item")]
    public class ShopItemData : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField] private bool unlocked;
        [SerializeField] private int cost;
        [SerializeField] private int id;
        [SerializeField] private ShopItemType type;

        public int ItemCost => cost;
        public string ItemName => itemName; 
        public bool ItemUnlocked => unlocked; 
        public int ItemId => id;
        public ShopItemType ItemType => type;
    }
}