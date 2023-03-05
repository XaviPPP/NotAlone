using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftingRecipe", menuName = "Crafting/Recipe")]
public class CraftingRecipeClass : ScriptableObject
{
    [Header("Crafting Recipe")]
    public SlotClass[] inputItems;
    public SlotClass outputItem;

    public bool CanCraft(InventoryManager inventory)
    {
        //check if we have space in inventory
        if (inventory.IsFull())
            return false;

        for (int i = 0; i < inputItems.Length; i++)
        {
            if (!inventory.Contains(inputItems[i].GetItem(), inputItems[i].GetQuantity()))
                return false;
        }     
        
        return true;
    }

    public void Craft(InventoryManager inventory)
    {
        //remove the input items from inventory
        for (int i = 0; i < inputItems.Length; i++)
        {
            inventory.Remove(inputItems[i].GetItem(), inputItems[i].GetQuantity());
        }

        //add output item to inventory
        inventory.Add(outputItem.GetItem(), outputItem.GetQuantity());
    }
}
