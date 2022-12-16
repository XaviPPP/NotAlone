using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    public bool isInsideCollider;
    public GameObject gameObject;
    // Start is called before the first frame update
    private void Start()
    {
        isInsideCollider = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Pickable")
        {
            isInsideCollider = true;
            gameObject = collider.gameObject;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Pickable")
        {
            isInsideCollider = false;
            gameObject = null;
        }
    }
}
