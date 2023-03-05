using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public override void Use()
    {
        base.Use();
        Debug.Log("Swing tool");
    }

    public override ToolClass GetTool() { return this; }
}
