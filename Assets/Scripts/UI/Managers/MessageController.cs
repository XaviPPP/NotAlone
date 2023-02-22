using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[HideMonoScript]
public class MessageController : MonoBehaviour
{
    public static MessageController instance;

    [Title("Message")]
    [Indent][SerializeField] private Transform parent;
    [Indent][SerializeField] private GameObject prefab;

    [Title("Properties")]
    [Indent][SerializeField] private string pickupMessage;
    [Indent][SerializeField] private int maxMessages = 4;

    Queue<string> messageQueue = new Queue<string>();

    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (instance == null)
        {
            instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void DisplayMessage(string message)
    {
        StartCoroutine(EnqueueMessages(message));
    }

    public void DisplayPickupMessage(string itemName)
    {
        StartCoroutine(EnqueueMessages($"{pickupMessage} {itemName}"));
    }

    IEnumerator EnqueueMessages(string message) {
        messageQueue.Enqueue(message);
        while (parent.childCount >= maxMessages)
        {
            yield return null;
        }

        string newMessage = messageQueue.Dequeue();
        StartCoroutine(CreateMessage(newMessage));
    }

    IEnumerator CreateMessage(string message)
    {
        GameObject obj = Instantiate(prefab);
        obj.transform.SetParent(parent, false);

        obj.GetComponentInChildren<TextMeshProUGUI>().text = message;
        Animator anim = obj.GetComponentInChildren<Animator>();

        while (isPlaying(anim, "MessagePopup"))
        {
            yield return null;
        }

        Destroy(obj);
    }


    bool isPlaying(Animator anim, string stateName)
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }
}
