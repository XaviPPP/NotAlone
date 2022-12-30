using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float distance = 3f;
    //[SerializeField] private LayerMask mask;
    private PlayerUI playerUI;  

    private Interactable lastInteractable;

    private void Start()
    {
        playerUI = GetComponent<PlayerUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.instance.gameIsPaused) return;

        playerUI.EnableInteractionText(false);
        playerUI.EnableLockedText(false);

        // create a ray at the center of the camera
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * distance);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, Physics.AllLayers))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                Interactable interactable = hitInfo.collider.GetComponent<Interactable>();
                
                if (interactable.GetComponent<Items>() != null)
                {
                    lastInteractable = interactable;
                    interactable.GetComponent<Outline>().enabled = true;
                } else if (lastInteractable != null)
                {
                    lastInteractable.GetComponent<Outline>().enabled = false;
                    lastInteractable = null;
                }

                Locker locker = interactable.GetComponent<Locker>();
                Door door = interactable.GetComponent<Door>();

                if (locker != null)
                {
                    if (locker.isLocked)
                    {
                        playerUI.EnableLockedText(true);
                    } else
                    {
                        playerUI.EnableLockedText(false);
                        playerUI.EnableInteractionText(true);
                        playerUI.UpdateText(interactable.promptMessage);
                    }
                }
                else if (door != null)
                {
                    if (door.isLocked)
                    {
                        playerUI.EnableLockedText(true);
                    }
                    else
                    {
                        playerUI.EnableLockedText(false);
                        playerUI.EnableInteractionText(true);
                        playerUI.UpdateText(interactable.promptMessage);
                    }
                }
                else
                {
                    playerUI.EnableLockedText(false);
                    playerUI.EnableInteractionText(true);
                    playerUI.UpdateText(interactable.promptMessage);
                }   

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.BaseInteract();
                }
            } 
            else
            {
                if (lastInteractable != null)
                {
                    lastInteractable.GetComponent<Outline>().enabled = false;
                    lastInteractable = null;
                }
            }
        }
    }
}
