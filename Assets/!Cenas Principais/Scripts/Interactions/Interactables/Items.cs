using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Items : Interactable
{
    public InventoryItemsData referenceItem;    
    
    // Novo
    public GameObject icon;
    public Transform itemPosition;
    public Transform player;
    private GameObject iconInstance;
    private bool inReach;

    void Start()
    {
        GetComponent<Outline>().OutlineWidth = 8f;
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
