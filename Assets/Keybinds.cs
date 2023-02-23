using HFPS.Systems;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using static HFPS.Systems.InventoryScriptable.ItemMapper;

[HideMonoScript]
public class Keybinds : MonoBehaviour
{
    public static Keybinds instance;

    [Title("Keybinds")]
    [Indent] public KeyCode interactKey;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
}
