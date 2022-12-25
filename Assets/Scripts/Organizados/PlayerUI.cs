using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private GameObject interactionText;
    [SerializeField] private TextMeshProUGUI promptText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void EnableInteractionText(bool i)
    {
        interactionText.SetActive(i);
    }

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }
}
