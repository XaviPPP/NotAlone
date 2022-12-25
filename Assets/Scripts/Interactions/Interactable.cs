using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // message displayed to the player when looking at the interactable
    public string promptMessage;

    // this function will be called from our player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // wont have any code written here
        // its just a template function to be overridden by subclasses
    }
}
