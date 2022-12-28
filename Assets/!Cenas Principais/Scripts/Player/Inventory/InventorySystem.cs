using HFPS.Systems;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    private Dictionary<InventoryItemsData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject itemsUI;
    [SerializeField] private Camera cam;

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject m_slotPrefab;

    //[SerializeField] private GameObject[] slotArray;
    
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
                OnUpdateInventory();
            }
            else
            {
                CloseInv();
            }
        }
    }

    private void OnUpdateInventory()
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach (InventoryItem item in inventory)
        {
            AddInventorySlot(item);
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(m_slotPrefab);
        obj.transform.SetParent(parent, false);

        UIInventorySlot slot = obj.GetComponent<UIInventorySlot>();
        slot.Set(item);
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
