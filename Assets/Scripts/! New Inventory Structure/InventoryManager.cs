using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;

[HideMonoScript]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    /*[SerializeField]*/ private List<CraftingRecipeClass> craftingRecipes;

    //[Title("Player")]
    //[Indent, SerializeField] private GameObject player;


    [Title("Objects")]
    [Indent, SerializeField] private GameObject itemCursor;
    [Indent, SerializeField] private GameObject slotHolder;
    [Indent, SerializeField] private GameObject hotbarSlotHolder;
    

    [Title("Properties")]
    [Indent] public int rowCount = 5;


    [Title("UI")]
    [Indent, SerializeField] private GameObject inventory;
    [Indent, SerializeField] private GameObject itemsUI;
    [Indent, SerializeField] private GameObject interactions;
    [Indent, SerializeField] private GameObject toolbar;


    /*[Title("Properties")] 
    [Indent, SerializeField] private Color normalSlotColor;
    [Indent, SerializeField] private Color selectedSlotColor;
    */


    [Title("Status")]
    [Indent, ReadOnly, SerializeField] private int selectedSlotIndex = 0;
    [Indent, ReadOnly, SerializeField] private ItemClass selectedItem;
    [HideInInspector] public bool isClosed;

    private SlotClass[] items;

    private GameObject[] slots;
    //private GameObject[] hotbarSlots;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;

    private HotbarSystem hotbar;

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
    }

    void Start()
    {
        hotbar = HotbarSystem.instance;

        craftingRecipes = Resources.LoadAll<CraftingRecipeClass>("Items/Crafting").ToList();

        slots = new GameObject[slotHolder.transform.childCount];
        items = new SlotClass[slots.Length];

        /*hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;
        }*/

        for (int i = 0; i < items.Length; i++)
        {
            items[i] = new SlotClass();
        }

        //set all the slots
        for (int i = 0; i < slotHolder.transform.childCount; i++)
        {
            slots[i] = slotHolder.transform.GetChild(i).gameObject;
        }

        //init start items
        /*for (int i = 0; i < startingItems.Length; i++)
        {
            Add(startingItems[i].GetItem(), startingItems[i].GetQuantity());
        }*/

        RefreshUI();
        //Add(itemToAdd, 1);
        //Remove(itemToRemove);
    }

    void Update()
    {
        if (Input.GetKeyDown(Keybinds.instance.inventoryKey))
        {
            if (isClosed && !PauseMenu.instance.gameIsPaused)
            {
                OpenInv();
            }
            else
            {
                CloseInv();
            }
        }

        if (!isClosed)
        {
            //if (Input.GetKeyDown(KeyCode.C)) //handle crafting
            //    Craft(craftingRecipes[0]);

            itemCursor.SetActive(isMovingItem);
            itemCursor.transform.position = Input.mousePosition;

            if (isMovingItem)
                itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;

            //Mouse1 clicked
            if (Input.GetMouseButtonDown(0))
            {
                //find the closest slot
                if (isMovingItem)
                {
                    //stop moving item
                    EndItemMove();
                }
                else
                {
                    BeginItemMove();
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //find the closest slot
                if (isMovingItem)
                {
                    //stop moving item
                    EndItemMove_Single();
                }
                else
                {
                    BeginItemMove_Half();
                }
            }
            //hotbarSelector.transform.position = hotbarSlots[selectedSlotIndex].transform.position;
        }

        /*if (isClosed)
        {
            

            if (Input.GetAxis("Mouse ScrollWheel") < 0) //Scrolling up
            {
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + 1, 0, hotbarSlots.Length - 1);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") > 0) //scrolling down
            {
                selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1, 0, hotbarSlots.Length - 1);
            }

            

            for (int i = 0; i < hotbarSlots.Length; i++)
            {
                hotbarSlots[i].GetComponent<Image>().color = normalSlotColor;

                if (i == selectedSlotIndex)
                    hotbarSlots[i].GetComponent<Image>().color = selectedSlotColor;
            }

            if (Input.GetKeyDown(Keybinds.instance.interactKey))
            {
                if (selectedItem != null)
                    UseSelected();
            }
        }*/
        //hotbarSlots[selectedSlotIndex].GetComponent<Image>().color = selectedSlotColor;

        //ItemClass oldSelectedItem = selectedItem;

        //selectedItem = items[selectedSlotIndex + (hotbarSlots.Length * 5)].GetItem();

        /*if (oldSelectedSlotIndex != selectedSlotIndex || oldSelectedItem != selectedItem)
                OnSelectedItemChange();
        */
    } 

    /* #region Inventory Utils */

    private void OpenInv()
    {
        inventory.SetActive(true);
        itemsUI.SetActive(false);

        toolbar.GetComponent<Toolbar>().SetFirstToggleActive();

        RefreshUI();

        isClosed = false;

        Cursor.lockState = CursorLockMode.None;
        ScriptController.instance.EnableMouseLook(false);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerInteract), false);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerUI), false);
        interactions.SetActive(false);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerMovement), false);
        ScriptController.instance.EnablePlayerScript(typeof(AnimationStateController), false);
    }

    public void CloseInv()
    {
        inventory.SetActive(false);
        itemsUI.SetActive(true);
        //ResetInfoPanel();
        isClosed = true;

        CraftingSystem.instance.GoBack();
        CraftingSystem.instance.SetSelectedItem(null);

        Cursor.lockState = CursorLockMode.Locked;
        ScriptController.instance.EnableMouseLook(true);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerInteract), true);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerUI), true);
        interactions.SetActive(true);
        ScriptController.instance.EnablePlayerScript(typeof(PlayerMovement), true);
        ScriptController.instance.EnablePlayerScript(typeof(AnimationStateController), true);
    }

    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemIcon;
                if (items[i].GetItem().isStackable)
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity().ToString();
                else
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }

        hotbar.RefreshHotbar(items);
    }

    /*public void RefreshHotbar()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i + (hotbarSlots.Length * 5)].GetItem().itemIcon;
                if (items[i + (hotbarSlots.Length * 5)].GetItem().isStackable)
                    hotbarSlots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i + (hotbarSlots.Length * 5)].GetQuantity().ToString();
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
    }*/

    public bool Add(ItemClass item, int quantity)
    {
        //check if inventory contains item
        SlotClass slot = Contains(item);

        if (slot != null && slot.GetItem().isStackable && slot.GetQuantity() < item.stackSize)
        {
            // going to add 20 = quantity
            // there is already 5 = slot.quantity;
            var quantityCanAdd = slot.GetItem().stackSize - slot.GetQuantity(); //16 - 5 = 11
            var quantityToAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);

            var remainder = quantity - quantityCanAdd; // = 9

            slot.AddQuantity(quantityToAdd);
            if (remainder > 0)
                Add(item, remainder);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null) //this is an empty slot
                {
                    var quantityCanAdd = item.stackSize - items[i].GetQuantity(); //16 - 5 = 11
                    var quantityToAdd = Mathf.Clamp(quantity, 0, quantityCanAdd);

                    var remainder = quantity - quantityCanAdd; // = 9

                    items[i].AddItem(item, quantityToAdd);
                    if (remainder > 0)
                        Add(item, remainder);
                    break;
                }
            }
        }

        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item, int quantity = 1)
    {
        //items.Remove(item);

        //check if inventory contains item
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
                temp.SubtractQuantity(quantity);
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < items.Length; i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }

                items[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    /*public void UseSelected()
    {
        if (selectedItem is ConsumableClass)
        {
            selectedItem.Use(player);
            items[selectedSlotIndex + (hotbarSlots.Length * 5)].SubtractQuantity(1);
        }
        RefreshUI();
    }*/

    public SlotClass[] GetItems()
    {
        return items;
    }

    public SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item /*&& items[i].item.isStackable && */)
                return items[i];
        }

        return null;
    }

    public bool Contains(ItemClass item, int quantity)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == item && items[i].GetQuantity() >= quantity)
                return true;
        }

        return false;
    }

    public bool IsFull()
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetItem() == null)
                return false;
        }

        return true;
    }

    private void Craft(CraftingRecipeClass recipe)
    {
        if (recipe.CanCraft(this))
            recipe.Craft(this);
        else
            //show message on UI
            Debug.Log("cant craft item");
    }

    /* #endregion */

    /* #region Slot Moving */

    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null)
            return false; //no item

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null || originalSlot.GetItem() == null)
            return false; //no item

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));
        originalSlot.SubtractQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));

        if (originalSlot.GetQuantity() == 0)
            originalSlot.Clear();

        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        /*originalSlot = GetClosestSlot();

        if (originalSlot == null || movingSlot.GetItem().isStackable == false)
            return false;
        if (originalSlot.GetItem() != null && (originalSlot.GetItem() != movingSlot.GetItem() || originalSlot.GetQuantity() >= originalSlot.GetItem().stackSize))
            return false;

        movingSlot.SubtractQuantity(1);

        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem())
        {
            originalSlot.AddQuantity(1);
        }
        else
            originalSlot.AddItem(movingSlot.GetItem(), 1);

        if (movingSlot.GetQuantity() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
            isMovingItem = true;

        RefreshUI();
        return true;*/

        originalSlot = GetClosestSlot();

        if (originalSlot == null || movingSlot.GetItem().isStackable == false)
            return false;

        // Check if the original slot is empty and move the item there if it is
        if (originalSlot.GetItem() == null)
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1);
            movingSlot.SubtractQuantity(1);

            if (movingSlot.GetQuantity() <= 0)
            {
                isMovingItem = false;
                movingSlot.Clear();
            }
            else
                isMovingItem = true;

            RefreshUI();
            return true;
        }

        // Check if the original slot can stack with the item being moved
        if (originalSlot.GetItem() == movingSlot.GetItem() && originalSlot.GetQuantity() < originalSlot.GetItem().stackSize)
        {
            originalSlot.AddQuantity(1);
            movingSlot.SubtractQuantity(1);

            if (movingSlot.GetQuantity() <= 0)
            {
                isMovingItem = false;
                movingSlot.Clear();
            }
            else
                isMovingItem = true;

            RefreshUI();
            return true;
        }

        return false;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();

        if (originalSlot == null)
        {
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem() && originalSlot.GetItem().isStackable && originalSlot.GetQuantity() < originalSlot.GetItem().stackSize) // same item
                {
                    var quantityCanAdd = originalSlot.GetItem().stackSize - originalSlot.GetQuantity();
                    var quantityToAdd = Mathf.Clamp(movingSlot.GetQuantity(), 0, quantityCanAdd);
                    var remainder = movingSlot.GetQuantity() - quantityToAdd;

                    originalSlot.AddQuantity(quantityToAdd);

                    if (remainder == 0)
                        movingSlot.Clear();
                    else
                    {
                        movingSlot.SubtractQuantity(quantityCanAdd);
                        RefreshUI();
                        return false;
                    }
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot); //a = b
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity()); //b = c
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity()); //a = c
                    RefreshUI();
                    return true;
                }
            }
            else //place item normally
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }


        isMovingItem = false;
        RefreshUI();
        return true;
    }

    private SlotClass GetClosestSlot()
    {
        Debug.Log(Input.mousePosition);

        for (int i = 0; i < items.Length; i++)
        {
            if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
                return items[i];
        }

        return null;
    }

    /* #endregion */

    /*private void OnSelectedItemChange()
    {
        //Debug.Log("Called");
        if (selectedItem is ToolClass)
        {
            UseSelected();
        }
    }*/

}
