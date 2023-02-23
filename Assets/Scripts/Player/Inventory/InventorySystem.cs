using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[HideMonoScript]
public class InventorySystem : MonoBehaviour
{
    public static InventorySystem instance;

    private Dictionary<InventoryItemsData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> _inventory { get; private set; }

    private InventoryItem selectedItem;
    private Toggle[] toggles;

    [Title("Player")]
    [Indent][SerializeField] private GameObject player;
    [Indent][SerializeField] private Camera cam;

    [Title("UI")]
    [Indent][SerializeField] private GameObject inventory;
    [Indent][SerializeField] private GameObject itemSlots;
    [Indent][SerializeField] private GameObject noItemsText;
    [Indent][SerializeField] private GameObject items;
    [Indent][SerializeField] private Transform info;
    [Indent][SerializeField] private Transform craft;
    [Indent][SerializeField] private GameObject actions;
    [Indent][SerializeField] private GameObject interactions;
    [Indent][SerializeField] private GameObject toolbar;
    [Indent] public Sprite transparent;

    [Title("Inventory Slot")]
    [Indent][SerializeField] private Transform parent;
    [Indent][SerializeField] private GameObject m_slotPrefab;

    [Title("Drop settings")]
    [Indent][SerializeField] private float spawnDistance = 2f;
    [Indent][SerializeField] private float spawnXRotation = 10f;
    [Indent][SerializeField] private float spawnZRotation = 25f;
    [Indent][SerializeField] private float spawnVerticalOffset = 1f;

    [HideInInspector] public bool isClosed;

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

        _inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemsData, InventoryItem>();
        isClosed = true;

        toggles = toolbar.GetComponentsInChildren<Toggle>();

        foreach (Toggle toggle in toggles)
        {
            toggle.onValueChanged.AddListener(delegate {
                OnToolbarToggleChanged(toggle);
            });
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(Keybinds.instance.inventoryKey))
        {
            if (isClosed && !PauseMenu.instance.gameIsPaused)
            {
                OpenInv();
                SetFirstToggleAsDefault(toggles);
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
        if (_inventory.Count == 0)
        {
            noItemsText.SetActive(true);
            info.gameObject.SetActive(false);
            actions.SetActive(false);
        } else
        {
            noItemsText.SetActive(false);
            info.gameObject.SetActive(true);
            actions.SetActive(true);

            foreach (InventoryItem item in _inventory)
            {
                AddInventorySlot(item);
            }
        }
    }

    public void DrawCrafting()
    {
        if (_inventory.Count == 0)
        {
            noItemsText.SetActive(true);
            craft.gameObject.SetActive(false);
        } else
        {
            noItemsText.SetActive(false);
            craft.gameObject.SetActive(true);
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(m_slotPrefab);
        obj.transform.SetParent(parent, false);

        UIInventorySlot slot = obj.GetComponent<UIInventorySlot>();
        slot.Set(item);
    }

    public void CloseInv()
    {
        inventory.SetActive(false);
        items.SetActive(true);
        ResetInfoPanel();
        isClosed = true;

        Cursor.lockState = CursorLockMode.Locked;
        ScriptController.instance.EnableMouseLook(true);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerInteract), true);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerUI), true);
        interactions.SetActive(true);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerMovement), true);
        ScriptController.instance.EnablePlayerScript(typeof(AnimationStateController), true);
    }

    private void OpenInv()
    {
        inventory.SetActive(true);
        items.SetActive(false);

        DrawInventory();

        isClosed = false;

        Cursor.lockState = CursorLockMode.None;
        ScriptController.instance.EnableMouseLook(false);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerInteract), false);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerUI), false);
        interactions.SetActive(false);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerMovement), false);
        ScriptController.instance.EnablePlayerScript(typeof(AnimationStateController), false);
    }

    private void ResetInfoPanel()
    {
        info.GetChild(0).GetComponent<Image>().sprite = transparent;
        info.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Empty;
        info.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Empty;
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
            _inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
    }

    public void DropItem(InventoryItem item) 
    {
        Remove(item.data);

        AudioManager.instance.PlayRandomDropClip();

        Vector3 playerPosition = player.transform.position;
        Vector3 playerDirection = player.transform.forward;
        Vector3 spawnPosition = playerPosition + (playerDirection + new Vector3(0, spawnVerticalOffset, 0)) * spawnDistance;
        Quaternion spawnRotation = Quaternion.Euler(spawnXRotation, player.transform.rotation.y, spawnZRotation);

        DynamicItemsManager.instance.InstantiateDynamic(item.data.prefab, spawnPosition, spawnRotation);

        OnUpdateInventory();
    }

    public void Remove(InventoryItemsData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                _inventory.Remove(value);
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
        Image icon = info.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI displayName = info.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = info.GetChild(2).GetComponent<TextMeshProUGUI>();

        icon.sprite = item.data.icon;
        displayName.text = item.data.displayName;
        description.text = item.data.description;
    }

    public void ClearItemInfo()
    {
        Image icon = info.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI displayName = info.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = info.GetChild(2).GetComponent<TextMeshProUGUI>();

        icon.sprite = transparent;
        displayName.text = string.Empty;
        description.text = string.Empty;
    }

    public void SetSelectedItem(InventoryItem item)
    {
        selectedItem = item;
    }

    public void OnToolbarToggleChanged(Toggle toggle)
    {
        if (toggle.name == "Items")
        {
            DrawInventoryToolbar();
        } else if (toggle.name == "Craft")
        {
            DrawCraftingToolbar();
        }
    }

    private void SetFirstToggleAsDefault(Toggle[] toggles)
    {
        toggles[0].isOn = true;
        toggles[0].GetComponent<ToolbarButtonToggle>().ToggleValueChanged(toggles[0]);
        //OnToolbarToggleChanged(toggles[0]);
    }

    private void DrawInventoryToolbar()
    {
        OnUpdateInventory();
        craft.gameObject.SetActive(false);
        itemSlots.SetActive(true);
    }

    private void DrawCraftingToolbar()
    {
        info.gameObject.SetActive(false);
        itemSlots.SetActive(false);
        actions.SetActive(false);
        DrawCrafting();
    }
}
