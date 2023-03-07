using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBox : Interactable
{
    public string openPromptMessage;
    public string closePromptMessage;

    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] private Animator lightAnimator;
    [SerializeField] private AudioClip[] doorOpenClips;
    [SerializeField] private AudioClip[] doorCloseClips;
    [SerializeField] private AudioClip[] doorLockedClips;
    [SerializeField] private bool isDoor;

    private bool isOpen;
    public bool isLocked;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        isOpen = false;
    }

    // this is where we will design our interaction using code
    protected override void Interact(GameObject player)
    {
        if (Input.GetKeyDown(KeyCode.E) && isLocked)
        {
            AudioManager.instance.PlayClip(audioSource, doorLockedClips[UnityEngine.Random.Range(0, doorLockedClips.Length)], 1f);
        }
        else if (Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            DoorOpens();
            promptMessage = closePromptMessage;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.E) && isOpen)
        {
            DoorCloses();
            promptMessage = openPromptMessage;
            isOpen = false;
        }
    }

    private void DoorOpens()
    {
        animator.SetBool("Open", true);
        animator.SetBool("Closed", false);
        AudioManager.instance.PlayClip(audioSource, doorOpenClips[UnityEngine.Random.Range(0, doorOpenClips.Length)], 1f);
        if (!isDoor)
        {
            lightAnimator.SetBool("1LeverOn", true);
        }
    }

    private void DoorCloses()
    {
        animator.SetBool("Open", false);
        animator.SetBool("Closed", true);
        AudioManager.instance.PlayClip(audioSource, doorCloseClips[UnityEngine.Random.Range(0, doorCloseClips.Length)], 1f);
        if (!isDoor)
        {
            lightAnimator.SetBool("1LeverOn", false);
        }
    }
}
