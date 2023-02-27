using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem instance;

    private List<InventoryItemsData> allItems;
    private List<InventoryItemsData> allCraftableItems;

    //private Array categories = Enum.GetNames(typeof(InventoryItemsData.Category));

    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject slotPrefab;

    [SerializeField] private Transform items;
    [SerializeField] private Transform info;

    [SerializeField] private GameObject craftButton;
    [SerializeField] private GameObject backButton;

    private InventoryItemsData selectedItem;
    private bool canCraftSelectedItem = true;

    public Sprite transparent;

    //Category Buttons
    [SerializeField] private Button[] categoryButtons;

    //Craft Buttons
    //private Button[] craftButtons;

    //Requirements text
    //TextMeshProUGUI requirements;

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

        allCraftableItems = new List<InventoryItemsData>();
    }

    private void Start()
    {
        GetAllItemsData();
        AddListeners();      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetAllItemsData()
    {
        allItems = Resources.LoadAll<InventoryItemsData>("Scriptable Objects").ToList();

        foreach (InventoryItemsData item in allItems)
        {
            if (item.isCraftable) allCraftableItems.Add(item);
        }
    }

    private void AddListeners()
    {
        categoryButtons = parent.GetComponentsInChildren<Button>();

        foreach (Button btn in categoryButtons)
        {
            btn.onClick.AddListener(delegate
            {
                OpenCategory(btn);
            });
        }

        craftButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            Craft(selectedItem);
        });
    }

    private void OpenCategory(Button btn)
    {
        parent.SetActive(false);
        items.gameObject.SetActive(true);
        backButton.SetActive(true);
        
        foreach (Transform t in items)
        {
            if (t.gameObject.name == btn.gameObject.name)
            {
                Debug.Log($"gameobject {t.gameObject.name}");
                t.gameObject.SetActive(true);
                RemoveItemsFromCategory(t.gameObject);
                AddItemsToCategory(t.gameObject);
                return;
            }
        }
    }


    private void AddItemsToCategory(GameObject parent)
    {
        foreach (InventoryItemsData item in allCraftableItems)
        {
            GameObject obj = Instantiate(slotPrefab, parent.transform);
            CraftingSlot slot = obj.GetComponent<CraftingSlot>();
            slot.Set(item);
        }
    }

    private void RemoveItemsFromCategory(GameObject parent)
    {
        foreach (Transform t in parent.transform)
        {
            Destroy(t.gameObject);
        }
    }

    public void GoBack()
    {
        items.gameObject.SetActive(false);
        parent.SetActive(true);
        backButton.SetActive(false);

        foreach (Transform t in items)
        {
            t.gameObject.SetActive(false);
        }

        ClearInfo();
    }

    public void ClearInfo()
    {
        info.GetChild(0).GetComponent<Image>().sprite = transparent;
        info.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Empty;
        info.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Empty;
        info.GetChild(4).gameObject.SetActive(false);
    }

    public void DrawItemInfo(InventoryItemsData item)
    {
        InventoryItemsData.CraftableItem[] itemsNeeded = item.itemsNeeded;

        TextMeshProUGUI requirements = info.GetChild(2).GetComponent<TextMeshProUGUI>();
        requirements.text = string.Empty;

        info.GetChild(0).GetComponent<Image>().sprite = item.icon;
        info.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.displayName;

        foreach (InventoryItemsData.CraftableItem itemNeeded in itemsNeeded)
        {
            var itemInInventory = InventorySystem.instance.Get(itemNeeded.item);
            var quantity = itemInInventory == null ? 0 : itemInInventory.stackSize;

            requirements.text += $"{itemNeeded.quantity} {itemNeeded.item.displayName} - {quantity}\n";

            //Debug.Log($"Quantity needed: {itemNeeded.quantity}");
            //Debug.Log($"Quantity: {quantity}");
            canCraftSelectedItem &= quantity >= itemNeeded.quantity;

            //Debug.Log($"Can craft {item.displayName}: {canCraft}");
        }
        craftButton.SetActive(true);
        TextMeshProUGUI craftBtnText = craftButton.GetComponentInChildren<TextMeshProUGUI>();

        if (canCraftSelectedItem)
        {
            craftBtnText.color = Color.green;
            craftButton.GetComponent<Button>().interactable = true;
        } else
        {
            craftBtnText.color = Color.red;
            craftButton.GetComponent<Button>().interactable = false;
        }
    }

    private void Craft(InventoryItemsData item)
    {
        if (canCraftSelectedItem)
        {
            InventorySystem.instance.Add(item);

            InventoryItemsData.CraftableItem[] itemsNeeded = item.itemsNeeded;

            foreach (InventoryItemsData.CraftableItem itemNeeded in itemsNeeded)
            {
                InventorySystem.instance.Remove(itemNeeded.item, itemNeeded.quantity);
            }

            DrawItemInfo(item);

            Debug.Log("Crafted");
        }
    }

    public void SetSelectedItem(InventoryItemsData item)
    {
        selectedItem = item;
    }

    public InventoryItemsData GetSelectedItem()
    {
        return selectedItem;
    }
}
