using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Misc Class", menuName = "Item/Misc")]

public class MiscClass : ItemClass
{
    public override void Use()
    {
        //base.Use(caller);
    }

    public override MiscClass GetMisc() { return this; }
}
