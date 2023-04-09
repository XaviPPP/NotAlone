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

    public bool isInteracting;

    public GameObject selectedTree;


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
                isInteracting = true;
            }
            else
            {
                if (lastInteractable != null)
                {
                    lastInteractable.GetComponent<Outline>().enabled = false;
                    lastInteractable = null;
                }
                isInteracting = false;
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
        else if (interactable.TryGetComponent<ChoppableTree>(out ChoppableTree tree))
        {
            InteractWithTree(tree);
        }
        else if (interactable.TryGetComponent<SleepSpot>(out SleepSpot sleep))
        {
            InteractWithSleepSpot(sleep);
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
            if (door.needsKey)
            {
                if (InventoryManager.instance.Contains(door.key, 1))
                {
                    playerUI.EnableInteractionText(true);
                    UpdatePromptText("desbloquear porta");
                }
            }
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

    private void InteractWithTree(ChoppableTree tree)
    {
        if (!tree.playerInRange)
        {
            playerUI.EnableInteractionText(false);
            selectedTree = null;
            return;
        }

        ItemClass selectedItem = HotbarSystem.instance.GetSelectedHotbarSlot().GetItem();
        ToolClass selectedTool = null;

        if (selectedItem != null)
        {
            selectedTool = selectedItem.GetTool();
        }

        if (selectedTool != null && selectedTool.toolType == ToolClass.ToolType.Axe)
        {
            selectedTree = tree.gameObject;
            playerUI.EnableInteractionText(true);
            UpdatePromptTextTree(tree);
        }
        else
        {
            playerUI.EnableInteractionText(false);
            selectedTree = null;
        }
    }

    private void InteractWithSleepSpot(SleepSpot sleep)
    {
        playerUI.EnableInteractionText(true);

        if (Enviro.EnviroManager.instance.Time.hours >= 20f || Enviro.EnviroManager.instance.Time.hours < 6f)
        {  
            UpdatePromptText(sleep.promptMessage);
        }
        else
        {
            UpdatePromptTextWithoutKey(sleep.cantSleepPrompt);
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

    private void UpdatePromptText(string text)
    {
        int? keyIndex = Keycodes.GetKeyByName(Keybinds.instance.interactKey.ToString());

        //Debug.Log(Keybinds.instance.interactKey.ToString());
        //Debug.Log(keyIndex);

        if (keyIndex == null) return;

        playerUI.UpdateText($"Pressione <sprite={keyIndex}> para {text}");
    }

    private void UpdatePromptTextWithoutKey(string text)
    {

        playerUI.UpdateText(text);
    }

    private void UpdatePromptText(KeyCode key, string text)
    {
        int? keyIndex = Keycodes.GetKeyByName(key.ToString());

        //Debug.Log(Keybinds.instance.interactKey.ToString());
        //Debug.Log(keyIndex);

        if (keyIndex == null) return;

        playerUI.UpdateText($"Pressione <sprite={keyIndex}> para {text}");
    }

    private void UpdatePromptTextTree(ChoppableTree tree)
    {
        playerUI.UpdateText($"Pressione <sprite=\"Keyboard_Black_Mouse_Left\" index=0> para {tree.promptMessage}");
    }
}
