using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teste : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Stopped touching " + other.gameObject.name);
    }
}
