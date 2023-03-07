using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Interactable
{
    public float replenishValue = 20f;
    protected override void Interact(GameObject player)
    {
        if (Input.GetKeyDown(Keybinds.instance.interactKey))
        {
            player.GetComponent<SurvivalManager>().ReplenishThirst(replenishValue);
        }
    }
}
