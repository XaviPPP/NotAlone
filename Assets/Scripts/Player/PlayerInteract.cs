using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Cinemachine;

[HideMonoScript]
public class PlayerInteract : MonoBehaviour
{
    [Title("Camera")]
    [Indent][SerializeField] private CinemachineVirtualCamera cam;

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
                CheckInteractableType(interactable);
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

    private void CheckInteractableType(Interactable interactable)
    {
        if (interactable.GetComponent<Items>() != null)
        {
            lastInteractable = interactable;
            interactable.GetComponent<Outline>().enabled = true;
        }
        else if (lastInteractable != null)
        {
            lastInteractable.GetComponent<Outline>().enabled = false;
            lastInteractable = null;
        }

        if (interactable.TryGetComponent<Locker>(out Locker locker))
        {
            InteractWithLocker(locker, interactable);
        }
        else if (interactable.TryGetComponent<Door>(out Door door))
        {
            InteractWithDoor(door, interactable);
        }
        else
        {
            Interact(interactable);
        }

        if (Input.GetKeyDown(Keybinds.instance.interactKey))
        {
            interactable.BaseInteract(gameObject);
        }
    }

    private void InteractWithDoor(Door door, Interactable interactable)
    {
        if (door.isLocked)
        {
            playerUI.EnableLockedText(true);
        }
        else
        {
            playerUI.EnableLockedText(false);
            playerUI.EnableInteractionText(true);
            UpdatePromptText(interactable);
        }
    }

    private void InteractWithLocker(Locker locker, Interactable interactable)
    {
        if (locker.isLocked)
        {
            playerUI.EnableLockedText(true);
        }
        else
        {
            playerUI.EnableLockedText(false);
            playerUI.EnableInteractionText(true);
            UpdatePromptText(interactable);
        }
    }

    private void Interact(Interactable interactable)
    {
        playerUI.EnableLockedText(false);
        playerUI.EnableInteractionText(true);
        UpdatePromptText(interactable);
    }

    private void UpdatePromptText(Interactable interactable)
    {
        int? keyIndex = Keycodes.GetKeyByName(Keybinds.instance.interactKey.ToString());

        //Debug.Log(Keybinds.instance.interactKey.ToString());
        //Debug.Log(keyIndex);

        if (keyIndex == null) return;

        playerUI.UpdateText($"Pressione <sprite={keyIndex}> para {interactable.promptMessage}");
    }
}
