using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Outline))]
[HideMonoScript]
public class Items : Interactable
{   
    [Title("Properties")]
    [Indent] public ItemClass referenceItem;
    [Indent] public string limitMessage = "Não podes carregar mais deste item!";

    void Start()
    {
        GetComponent<Outline>().enabled = false;
        GetComponent<Outline>().OutlineWidth = 5f;
    }

    protected override void Interact(GameObject player)
    {
        if (Input.GetKeyDown(Keybinds.instance.interactKey))
        {
            OnHandlePickupItem();
        }
    }

    public void OnHandlePickupItem()
    {
        SlotClass slot = InventoryManager.instance.Contains(referenceItem);

        if (slot != null && (slot.GetQuantity() == referenceItem.stackSize || !slot.GetItem().isStackable))
        {
            MessageController.instance.DisplayMessage(limitMessage);
            return;
        }

        InventoryManager.instance.Add(referenceItem, 1);
        AudioManager.instance.PlayRandomPickupClip();
        MessageController.instance.DisplayPickupMessage(referenceItem.itemName);
        //Debug.Log(referenceItem.id);
        //Debug.Log(referenceItem.displayName);
        Destroy(gameObject);
    }
}
