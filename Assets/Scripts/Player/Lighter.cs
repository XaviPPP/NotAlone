using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : MonoBehaviour
{
    bool isOn;
    [SerializeField] private GameObject lighter;
    [SerializeField] private ItemClass lighterClass;

    // Start is called before the first frame update
    void Start()
    {
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        SlotClass slot = InventoryManager.instance.Contains(lighterClass);

        if (slot != null)
        {
            if (Input.GetKeyDown(Keybinds.instance.lighterKey))
            {
                isOn = !isOn;
            }

            lighter.SetActive(isOn);
        }
    }
}
