using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable Class", menuName = "Item/Consumable")]

public class ConsumableClass : ItemClass
{
    private SurvivalManager survivalManager;
    //data specific to consumables
    [Header("Consumable")]
    public float healthAdded;
    public float hungerAdded;
    public float thirstAdded;
    public float energyAdded;

    [Space]
    public float healthRemoved;
    public float hungerRemoved;
    public float thirstRemoved;
    public float energyRemoved;

    public override void Use(GameObject player)
    {
        survivalManager = player.GetComponent<SurvivalManager>();
        
        base.Use(player);
        Debug.Log("Eat consumable");
        InventoryManager.instance.UseSelected();
        survivalManager.ReplenishHealth(healthAdded);
        survivalManager.ReplenishHunger(hungerAdded);
        survivalManager.ReplenishThirst(thirstAdded);
        
        survivalManager.DepleteHealth(healthRemoved);
        survivalManager.DepleteHunger(hungerRemoved);
        survivalManager.DepleteThirst(thirstRemoved);
    }
    public override ConsumableClass GetConsumable() { return this; }
}
