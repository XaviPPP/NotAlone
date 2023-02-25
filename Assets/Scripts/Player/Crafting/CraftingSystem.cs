using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    public static CraftingSystem instance;

    private List<InventoryItem> inventory = InventorySystem.instance._inventory;

    private List<InventoryItemsData> allItems;
    private List<InventoryItemsData> allCraftableItems;

    //Category Buttons
    private Button tools;

    //Craft Buttons
    private Button[] craftButtons;

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

        allItems = Resources.LoadAll<InventoryItemsData>("Scriptable Objects").ToList();

        allCraftableItems = new List<InventoryItemsData>();

        //Debug.Log(allItems.Count);

        foreach (InventoryItemsData item in allItems)
        {
            if (item.isCraftable) allCraftableItems.Add(item);
        }

        //Debug.Log(allCraftableItems.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
