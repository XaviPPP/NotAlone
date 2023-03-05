using System.Collections;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;

    public SlotClass()
    {
        item = null;
        quantity = 0;
    }

    public SlotClass(ItemClass _item, int _quantity)
    {
        item = _item;
        quantity = _quantity;
    }

    public SlotClass(SlotClass slot)
    {
        item = slot.GetItem();
        quantity = slot.GetQuantity();
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }

    public ItemClass GetItem() { return item; }
    public int GetQuantity() { return quantity; }
    public void AddQuantity(int _quantity) { quantity += _quantity; }
    public void SubtractQuantity(int _quantity)
    {
        quantity -= _quantity;

        if (quantity <= 0)
        {
            Clear();
        }
    }
    public void AddItem(ItemClass _item, int _quantity) { item = _item; quantity = _quantity; }
}
