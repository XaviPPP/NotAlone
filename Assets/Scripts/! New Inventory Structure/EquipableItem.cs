using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EquipableItem : MonoBehaviour
{

    private Animator animator;

    [SerializeField] private ItemClass itemType;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (InventoryManager.instance.isClosed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("Hit");
            }
        }
    }
}
