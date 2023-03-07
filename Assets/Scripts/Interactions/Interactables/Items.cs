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
        InventoryManager.instance.Add(referenceItem, 1);
        AudioManager.instance.PlayRandomPickupClip();
        MessageController.instance.DisplayPickupMessage(referenceItem.itemName);
        //Debug.Log(referenceItem.id);
        //Debug.Log(referenceItem.displayName);
        Destroy(gameObject);
    }
}
