using HFPS.Systems;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.Port;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    private Dictionary<InventoryItemsData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    private InventoryItem selectedItem;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject noItemsText;
    [SerializeField] private GameObject itemsUI;
    [SerializeField] private Transform infoUI;
    [SerializeField] private Camera cam;

    [SerializeField] private Transform parent;
    [SerializeField] private GameObject m_slotPrefab;

    [SerializeField] private float spawnDistance = 2f;
    [SerializeField] private float spawnXRotation = 10f;
    [SerializeField] private float spawnZRotation = 25f;
    [SerializeField] private float spawnVerticalOffset = 1f;
    
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
            if (t.gameObject != noItemsText)
            {
                Destroy(t.gameObject);
            }
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        if (inventory.Count == 0)
        {
            noItemsText.SetActive(true);
        } else
        {
            noItemsText.SetActive(false);

            foreach (InventoryItem item in inventory)
            {
                AddInventorySlot(item);
            }
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
        ResetInfoPanel();
        isClosed = true;

        Cursor.lockState = CursorLockMode.Locked;
        cam.GetComponent<MouseLook>().enabled = true;
    }

    private void OpenInv()
    {
        inventoryUI.SetActive(true);
        itemsUI.SetActive(false);

        DrawInventory();

        isClosed = false;

        Cursor.lockState = CursorLockMode.None;
        cam.GetComponent<MouseLook>().enabled = false;
    }

    private void ResetInfoPanel()
    {
        infoUI.GetChild(0).GetComponent<Image>().sprite = transparent;
        infoUI.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Empty;
        infoUI.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Empty;
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

    public void DropItem(InventoryItem item) 
    {
        Remove(item.data);

        Vector3 playerPosition = player.transform.position;
        Vector3 playerDirection = player.transform.forward;
        Vector3 spawnPosition = playerPosition + (playerDirection + new Vector3(0, spawnVerticalOffset, 0)) * spawnDistance;
        Quaternion spawnRotation = Quaternion.Euler(spawnXRotation, player.transform.rotation.y, spawnZRotation);

        Instantiate(item.data.prefab, spawnPosition, spawnRotation);

        OnUpdateInventory();
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

                if (value == selectedItem)
                {
                    ClearItemInfo();
                    selectedItem = null;
                }
            }
        }
    }

    public void DrawItemInfo(InventoryItem item)
    {
        Image icon = infoUI.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI displayName = infoUI.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = infoUI.GetChild(2).GetComponent<TextMeshProUGUI>();

        icon.sprite = item.data.icon;
        displayName.text = item.data.displayName;
        description.text = item.data.description;
    }

    public void ClearItemInfo()
    {
        Image icon = infoUI.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI displayName = infoUI.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = infoUI.GetChild(2).GetComponent<TextMeshProUGUI>();

        icon.sprite = transparent;
        displayName.text = string.Empty;
        description.text = string.Empty;
    }

    public void SetSelectedItem(InventoryItem item)
    {
        selectedItem = item;
    }
}
