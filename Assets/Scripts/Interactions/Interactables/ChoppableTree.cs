using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class ChoppableTree : Interactable
{
    public bool playerInRange;

    public float treeMaxHealth;
    public float treeHealth;

    public ItemClass wood;

    void Start()
    {
        treeHealth = treeMaxHealth;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Exit");
        }
    }

    public void GetHit(float health)
    {
        treeHealth -= health;

        if (treeHealth <= 0f) 
        {
            StartCoroutine(TreeFall());
            DropRandomWood();
        }
    }

    private void DropRandomWood()
    {
        int woodCount = Random.Range(8, 15);

        InventoryManager.instance.Add(wood, woodCount);

        MessageController.instance.DisplayMessage($"Obteste {woodCount} de madeira");
    }

    IEnumerator TreeFall()
    {
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().mass = 5f;
        gameObject.GetComponent<Rigidbody>().drag = .05f;
        gameObject.GetComponent<Rigidbody>().angularDrag = 5f;
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * 1500f);

        yield return new WaitForSeconds(10f);

        Destroy(gameObject);
    }
}
