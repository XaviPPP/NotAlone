using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemsData : ScriptableObject
{
    public string id;
    public string displayName;
    public string description;
    public Sprite icon;
    public GameObject prefab;
}
