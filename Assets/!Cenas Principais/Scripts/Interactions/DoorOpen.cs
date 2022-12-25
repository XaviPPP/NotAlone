using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] private Animator door;
    [SerializeField] private GameObject openDoorButton;
    [SerializeField] private AudioSource doorAudioSource;
    [SerializeField] private AudioClip[] doorOpenClips;
    [SerializeField] private AudioClip[] doorCloseClips;
    [SerializeField] private AudioClip[] doorLockedClips;
    [SerializeField] private bool isLocked;
    private bool isOpen;
    private bool inReach;

    void Start()
    {
        inReach = false;
        isOpen = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = true;
            openDoorButton.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            inReach = false;
            openDoorButton.SetActive(false);
        }
    }

    void Update()
    {
        if (inReach && Input.GetButtonDown("Interact") && isLocked)
        {
            doorAudioSource.PlayOneShot(doorLockedClips[UnityEngine.Random.Range(0, doorLockedClips.Length)]);
        }
        else if (inReach && Input.GetButtonDown("Interact") && !isOpen)
        {
            DoorOpens();
            isOpen = true;
        }
        else if(inReach && Input.GetButtonDown("Interact") && isOpen)
        {
            DoorCloses();
            isOpen = false;
        }
    }

    private void DoorOpens()
    {
        door.SetBool("Open", true);
        door.SetBool("Closed", false);
        doorAudioSource.PlayOneShot(doorOpenClips[UnityEngine.Random.Range(0, doorOpenClips.Length)]);
    }

    private void DoorCloses()
    {
        door.SetBool("Open", false);
        door.SetBool("Closed", true);
        doorAudioSource.PlayOneShot(doorCloseClips[UnityEngine.Random.Range(0, doorCloseClips.Length)]);
    }
}
