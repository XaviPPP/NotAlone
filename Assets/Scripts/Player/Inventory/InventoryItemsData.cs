using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemsData : ScriptableObject
{
    public string id;
    public string displayName;
    [Multiline] public string description;
    public Sprite icon;
    public GameObject prefab;
    public Category category;
    public bool isCraftable;
    [ShowIf("isCraftable")]
    [Indent]public CraftableItem[] itemsNeeded;

    public enum Category
    {
        Tools,
        Food,
        Crafting,
        Utility,
        Other
    }

    [System.Serializable]
    public class CraftableItem
    {
        public InventoryItemsData item;
        public int quantity;
    }
}