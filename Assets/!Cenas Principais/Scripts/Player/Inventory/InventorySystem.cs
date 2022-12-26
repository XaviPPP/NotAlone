using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.Port;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    private Dictionary<InventoryItemsData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject itemsUI;
    [SerializeField] private Camera cam;

    [SerializeField] private GameObject m_slotPrefab;

    [SerializeField] private GameObject[] slotArray;
    
    public Sprite transparent;

    private bool isClosed;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemsData, InventoryItem>();
        isClosed = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isClosed)
            {
                OpenInv();
                DrawInventory();
            }
            else
            {
                CloseInv();
            }
        }
    }

    public void DrawInventory()
    {
        foreach (GameObject slot in slotArray)
        {
            slot.GetComponent<UIInventorySlot>().Clear();
        }

        foreach (InventoryItem item in inventory)
        {
            foreach (GameObject slot in slotArray)
            {
                if (!slot.GetComponent<UIInventorySlot>().hasItem)
                {
                    slot.GetComponent<UIInventorySlot>().Set(item);
                    break;
                }
            }
        }
    }

    private void CloseInv()
    {
        inventoryUI.SetActive(false);
        itemsUI.SetActive(true);
        isClosed = true;

        Cursor.lockState = CursorLockMode.Locked;
        cam.GetComponent<MouseLook>().enabled = true;
    }

    private void OpenInv()
    {
        inventoryUI.SetActive(true);
        itemsUI.SetActive(false);
        isClosed = false;

        Cursor.lockState = CursorLockMode.None;
        cam.GetComponent<MouseLook>().enabled = false;
    }

    public InventoryItem Get(InventoryItemsData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void Add(InventoryItemsData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
    }

    public void Remove(InventoryItemsData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
    }
}
