using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ToolbarButtonToggle : MonoBehaviour
{
    private Animator animator;
    private Toggle toggle;
    private Image image;

    [SerializeField] private GameObject icon;
    void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        toggle = GetComponent<Toggle>();

        toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(toggle);
        });
    }

    public void ToggleValueChanged(Toggle toggle)
    {
        if (toggle.isOn)
        {
            animator.SetBool("isSelected", true);
        } else
        {
            animator.SetBool("isSelected", false);
        }
    }
}
