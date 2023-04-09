using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
public class Keybinds : MonoBehaviour
{
    public static Keybinds instance;

    [Title("Keybinds")]
    [Indent] public KeyCode interactKey = KeyCode.E;
    [Indent] public KeyCode inventoryKey = KeyCode.I;
    [Indent] public KeyCode chopKey = KeyCode.Mouse1;
    [Indent] public KeyCode lighterKey = KeyCode.L;

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
