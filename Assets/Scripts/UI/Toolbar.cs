using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour
{
    public static Toolbar instance;

    [SerializeField] private GameObject items;
    [SerializeField] private GameObject craft;
    private Toggle[] toggles;
    private Toggle activeToggle = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        toggles = GetComponentsInChildren<Toggle>();

        foreach (Toggle toggle in toggles)
        {
            Debug.Log(toggle.name);
            toggle.onValueChanged.AddListener(delegate {
                OnToolbarToggleChanged(toggle);
            });
        }
    }


    public void SetFirstToggleActive()
    {
        //Debug.Log("called");
        toggles[0].isOn = true;
        toggles[0].gameObject.GetComponent<Animator>().SetBool("isSelected", true);
    }

    void OnToolbarToggleChanged(Toggle toggle)
    {
        if (toggle.name == "Items")
        {
            craft.SetActive(false);
            items.SetActive(true);
            //InventorySystem.instance.OnUpdateInventory();
            InventoryManager.instance.RefreshUI();
        } else if (toggle.name == "Craft")
        {
            items.SetActive(false);
            craft.SetActive(true);

            ItemClass item = CraftingSystem.instance.GetSelectedItem();

            if (item != null) { CraftingSystem.instance.DrawItemInfo(item); }
        }
        activeToggle = toggle;
        Debug.Log(activeToggle.name);
    }

    public string GetActiveToggle()
    {
        return activeToggle.name;
    }
}
