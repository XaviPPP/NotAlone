using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class InventoryItem
{
    public InventoryItemsData data { get; private set; }
    public int stackSize { get; private set; }

    public UnityEvent OnUse;

    public InventoryItem(InventoryItemsData source)
    {
        data = source;
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }

    public void RemoveFromStack(int quantity)
    {
        stackSize -= quantity;
    }
}
