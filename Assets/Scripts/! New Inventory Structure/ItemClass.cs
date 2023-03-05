using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClass : ScriptableObject
{
    //data shared across every item
    [Header("Item")]
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable = true;
    public int stackSize = 64;

    public virtual void Use()
    {
        Debug.Log("Used Item");
    } 
    public virtual ItemClass GetItem() { return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
}