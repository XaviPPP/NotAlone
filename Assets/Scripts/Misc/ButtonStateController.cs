using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonStateController : MonoBehaviour
{
    public void ResetButtonState()
    {
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
    }
}
