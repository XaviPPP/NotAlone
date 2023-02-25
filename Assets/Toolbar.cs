using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Toolbar : MonoBehaviour
{
    [SerializeField] private GameObject items;
    [SerializeField] private GameObject craft;
    private Toggle[] toggles;

    // Start is called before the first frame update
    void Awake()
    {
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
            items.SetActive(true);
            craft.SetActive(false);
        } else if (toggle.name == "Craft")
        {
            items.SetActive(false);
            craft.SetActive(true);
        }
    }
}
