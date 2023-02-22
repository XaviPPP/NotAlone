using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
public class PlayerInteract : MonoBehaviour
{
    [Title("Camera")]
    [Indent][SerializeField] private Camera cam;

    [Title("Properties")]
    [Indent][SerializeField] private float distance = 3f;
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
            if (hitInfo.collider.TryGetComponent<Interactable>(out Interactable interactable))
            {             
                if (interactable.GetComponent<Items>() != null)
                {
                    lastInteractable = interactable;
                    interactable.GetComponent<Outline>().enabled = true;
                } else if (lastInteractable != null)
                {
                    lastInteractable.GetComponent<Outline>().enabled = false;
                    lastInteractable = null;
                }

                if (interactable.TryGetComponent<Locker>(out Locker locker))
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
                else if (interactable.TryGetComponent<Door>(out Door door))
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
