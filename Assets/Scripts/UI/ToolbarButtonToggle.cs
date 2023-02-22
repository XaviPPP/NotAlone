using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ToolbarButtonToggle : MonoBehaviour
{
    [SerializeField] private Sprite selectedSprite;
    [SerializeField] private Color selectedColor;

    private Sprite originalSprite;
    private Color originalColor;

    private Image image;
    private TextMeshProUGUI text;
    private Toggle toggle;
    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle);
        });

        originalSprite = image.sprite;
        originalColor = text.color;
    }

    void ToggleValueChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            image.sprite = selectedSprite;
            text.color = selectedColor;
        } else
        {
            image.sprite = originalSprite;
            text.color = originalColor;
        }
    }
}
