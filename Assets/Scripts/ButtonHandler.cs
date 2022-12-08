using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI text;
    public void IncreaseSize()
    {
        text.fontSize += 4;
    }

    public void DecreaseSize()
    {
        text.fontSize -= 4;
    }
}
