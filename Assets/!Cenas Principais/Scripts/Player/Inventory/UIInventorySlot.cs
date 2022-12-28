using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Reflection.Emit;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [SerializeField] private Image m_icon;
    //[SerializeField] private GameObject m_stackObj;
    [SerializeField] private TextMeshProUGUI m_stackLabel;

    public void Clear()
    {
        m_icon.sprite = InventorySystem.instance.transparent;
        m_stackLabel.text = string.Empty;
    }

    public void Set(InventoryItem item)
    {
        m_icon.sprite = item.data.icon;
        /*if (item.stackSize <= 1)
        {
            m_stackObj.SetActive(false);
            return;
        }*/
        if (item.stackSize == 1)
        {
            m_stackLabel.text = string.Empty;
        } else
        {
            m_stackLabel.text = item.stackSize.ToString();
        }
    }
}
