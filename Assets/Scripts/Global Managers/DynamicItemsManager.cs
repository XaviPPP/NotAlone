using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[HideMonoScript]
public class DynamicItemsManager : MonoBehaviour
{
    public static DynamicItemsManager instance { get; set; }

    [Title("Objects")]
    [Indent][SerializeField] private GameObject parent;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void InstantiateDynamic(GameObject obj)
    {
        Instantiate(obj, parent.transform);
    }

    public void InstantiateDynamic(GameObject obj, Vector3 position, Quaternion rotation)
    {
        Instantiate(obj, position, rotation, parent.transform);
    }
}
