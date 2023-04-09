using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour, IPointerClickHandler
{
    private ItemClass item;

    [SerializeField] private Image m_icon;
    [SerializeField] private TextMeshProUGUI m_label;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            CraftingSystem.instance.SetSelectedItem(item);
            CraftingSystem.instance.DrawItemInfo(item);
        }
    }

    public void Set(ItemClass item)
    {
        this.item = item;
        m_icon.sprite = item.itemIcon;
        m_label.text = item.itemName;
    }
}
