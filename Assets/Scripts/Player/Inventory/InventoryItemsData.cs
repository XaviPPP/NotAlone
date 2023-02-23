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
    public bool isCraftable;
    public Category category;

    public enum Category
    {
        Tools,
        Food,
        Crafting,
        Utility,
        Other
    }
}
