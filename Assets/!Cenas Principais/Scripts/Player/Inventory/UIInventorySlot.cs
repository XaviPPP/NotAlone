using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerClickHandler
{
    private InventoryItem item;

    [SerializeField] private Image m_icon;
    //[SerializeField] private GameObject m_stackObj;
    [SerializeField] private TextMeshProUGUI m_stackLabel;

    public void Clear()
    {
        m_icon.sprite = InventorySystem.instance.transparent;
        m_stackLabel.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            InventorySystem.instance.SetSelectedItem(item);
            InventorySystem.instance.DrawItemInfo(item);
        } else if (eventData.button == PointerEventData.InputButton.Right)
        {
            InventorySystem.instance.DropItem(item);
        }
    }

    public void Set(InventoryItem item)
    {
        this.item = item;
        m_icon.sprite = this.item.data.icon;

        if (item.stackSize == 1)
        {
            m_stackLabel.text = string.Empty;
        } else
        {
            m_stackLabel.text = this.item.stackSize.ToString();
        }
    }
}
