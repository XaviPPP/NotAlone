using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCheck : MonoBehaviour
{
    public bool isInsideCollider;
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
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Pickable")
        {
            isInsideCollider = false;
        }
    }
}
