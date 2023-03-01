using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemsData : ScriptableObject
{
    public string id;
    public string displayName;
    [Multiline] public string description;

    public ExposedReference<GameObject> obj;

    public Sprite icon;
    public GameObject prefab;
    public Category category;
    public bool isCraftable;
    [ShowIf("isCraftable")]
    [Indent]public CraftableItem[] itemsNeeded;
    public bool isUsable;
    [ShowIf("isUsable")]

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