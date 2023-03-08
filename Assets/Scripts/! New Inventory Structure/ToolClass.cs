using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[CreateAssetMenu(fileName = "New Tool Class", menuName = "Item/Tool/Tool")]
public class ToolClass : ItemClass
{
    //data specific to tools
    [Header("Tool")]
    public ToolType toolType;
    public enum ToolType
    {
        Weapon,
        Pickaxe,
        Hammer,
        Axe
    }
    public GameObject usablePrefab;

    public override ToolClass GetTool() { return this; } 
}
