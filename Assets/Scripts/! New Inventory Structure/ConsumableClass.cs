using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Class", menuName = "Item/Consumable")]

public class ConsumableClass : ItemClass
{
    //data specific to consumables
    [Header("Consumable")]
    public float healthAdded;

    public override void Use()
    {
        base.Use();
        Debug.Log("Eat consumable");
        //caller.inventory.UseSelected();
    }
    public override ConsumableClass GetConsumable() { return this; }
}
