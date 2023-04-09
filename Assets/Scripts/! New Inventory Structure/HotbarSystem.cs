using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

[HideMonoScript]
public class HotbarSystem : MonoBehaviour
{
    public static HotbarSystem instance;


    [Title("Player")]
    [SerializeField] private GameObject player;


    [Title("Objects")]
    [SerializeField] private GameObject hotbarSlotHolder;
    //[SerializeField] private GameObject itemName;
    [SerializeField] private GameObject toolHolder;

    [Title("Properties")]
    [SerializeField] private Color normalSlotColor;
    [SerializeField] private Color selectedSlotColor;

    [Title("Status")]
    [ReadOnly] public int slotCount;
    [ReadOnly, SerializeField] private int selectedSlotIndex = 0;
    [ReadOnly, SerializeField] private ItemClass selectedItem;
    private GameObject selectedItemModel;
    [ReadOnly, SerializeField] private ItemClass oldSelectedItem;


    private GameObject[] hotbarSlots;

    private InventoryManager inventory;

    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (instance == null)
        {
            instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        InitializeHotbar();

        inventory = InventoryManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        HandleHotbar();
    }

    private void InitializeHotbar()
    {
        hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];

        slotCount = hotbarSlots.Length;

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;
        }
    }

    private void HandleHotbar()
    {
        if (inventory.isClosed)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0) //Scrolling up
            {
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + 1, 0, hotbarSlots.Length - 1);
                OnSlotChanged();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0) //scrolling down
            {
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1, 0, hotbarSlots.Length - 1);
                OnSlotChanged();
            }

            SelectSlotByNumber();

            for (int i = 0; i < hotbarSlots.Length; i++)
            {
                hotbarSlots[i].GetComponent<Image>().color = normalSlotColor;

                if (i == selectedSlotIndex)
                    hotbarSlots[i].GetComponent<Image>().color = selectedSlotColor;
            }

            if (Input.GetKeyDown(Keybinds.instance.interactKey))
            {
                if (selectedItem != null && !player.GetComponent<PlayerInteract>().isInteracting)
                    UseSelected();
            }
        }
    }

    public void RefreshHotbar(SlotClass[] items)
    {
        //SlotClass[] items = inventory.GetItems();

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i + (slotCount * inventory.rowCount)].GetItem().itemIcon;
                if (items[i + (slotCount * inventory.rowCount)].GetItem().isStackable)
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i + (slotCount * inventory.rowCount)].GetQuantity().ToString();
                else
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            catch
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }

        OnSlotChanged();
    }

    public SlotClass GetSelectedHotbarSlot()
    {
        return inventory.GetItems()[selectedSlotIndex + (slotCount * inventory.rowCount)];
    }

    public void UseSelected()
    {
        if (selectedItem is ConsumableClass)
        {
            selectedItem.Use(player);
            GetSelectedHotbarSlot().SubtractQuantity(1);
            OnSlotChanged();
        }
        inventory.RefreshUI();
    }

    private void SelectSlotByNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedSlotIndex = 0;
            OnSlotChanged();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedSlotIndex = 1;
            OnSlotChanged();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedSlotIndex = 2;
            OnSlotChanged();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedSlotIndex = 3;
            OnSlotChanged();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            selectedSlotIndex = 4;
            OnSlotChanged();
        }
    }

    private void SetEquippedModel()
    {
        if (selectedItem != null && selectedItem is ToolClass)
        {
            selectedItemModel = Instantiate(selectedItem.GetTool().usablePrefab);
            selectedItemModel.transform.SetParent(toolHolder.transform, false);
        }
    }

    private void UnsetEquippedModel()
    {
        Destroy(selectedItemModel);
    }

    private void OnSlotChanged()
    {
        selectedItem = GetSelectedHotbarSlot().GetItem();

        if (selectedItem is ToolClass)
        {
            UnsetEquippedModel();
            SetEquippedModel();
        }
        else
            UnsetEquippedModel();

    }
}
