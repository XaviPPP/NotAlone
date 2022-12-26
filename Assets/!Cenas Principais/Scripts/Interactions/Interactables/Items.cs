using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Items : Interactable
{
    public InventoryItemsData referenceItem;

    protected override void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnHandlePickupItem();
        }
    }

    public void OnHandlePickupItem()
    {
        InventorySystem.instance.Add(referenceItem);
        Debug.Log(referenceItem.id);
        Debug.Log(referenceItem.displayName);
        Destroy(gameObject);
    }
}
