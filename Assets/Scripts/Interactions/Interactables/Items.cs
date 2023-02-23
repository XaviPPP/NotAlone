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
    [Indent] public InventoryItemsData referenceItem;
    [Indent] public GameObject icon;
    [Indent] public Transform itemPosition;
    [Indent] public Transform player;

    private GameObject iconInstance;
    private bool inReach;

    void Start()
    {
        GetComponent<Outline>().enabled = false;
        GetComponent<Outline>().OutlineWidth = 5f;
        icon.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
}
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Reach")
        {
            inReach = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
        }
    }

    void Update()
    {
        if (icon == null && itemPosition == null && player == null) return;

        if (inReach) { 
            if (iconInstance == null)
            {
                iconInstance = Instantiate(icon, new Vector3(itemPosition.position.x, itemPosition.position.y + 0.1f, itemPosition.position.z), Quaternion.identity);
                iconInstance.transform.parent = transform;

            }
            iconInstance.transform.LookAt(player);
        }
        else
        {
            if (iconInstance != null)
            {
                Destroy(iconInstance);
            }
        }
    }
    // Fim Novo

    protected override void Interact()
    {
        if (Input.GetKeyDown(Keybinds.instance.interactKey))
        {
            OnHandlePickupItem();
        }
    }

    public void OnHandlePickupItem()
    {
        InventorySystem.instance.Add(referenceItem);
        AudioManager.instance.PlayRandomPickupClip();
        MessageController.instance.DisplayPickupMessage(referenceItem.displayName);
        //Debug.Log(referenceItem.id);
        //Debug.Log(referenceItem.displayName);
        Destroy(gameObject);
    }
}
