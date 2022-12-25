using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] Transform holdArea;
    [SerializeField] GameObject trigger;
    private GameObject heldObj;
    private Rigidbody heldObjRB;

    [Header("Canvas")]
    [SerializeField] GameObject pickupUI;

    [Header("Object Outline")]
    [SerializeField] private Outline.Mode outlineMode;
    [SerializeField] private Color outlineColor;
    [SerializeField] private float outlineWidth;
    private GameObject currentLookAtObject;
    private Outline currentOutline;

    [Header("Physics Parameters")]
    [SerializeField] private float pickupRange = 5.0f;
    [SerializeField] private float pickupForce = 150.0f;

    private bool move = false;
    private bool picked = false;

    private void Update()
    {
        /*if (trigger.GetComponent<TriggerCheck>().isInsideCollider)
        {
            if (!picked)
            {
                pickupUI.SetActive(false);

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                {
                    GameObject hitObject = hit.collider.gameObject;
                    if (hit.transform.tag == "Pickable")
                    {
                        // se o jogador estiver a olhar para um novo objeto, desativar o outline no
                        // objeto antigo e ativar no novo objeto
                        if (hitObject != currentLookAtObject)
                        {
                            if (currentOutline != null)
                            {
                                currentOutline.enabled = false;
                            }
                            
                            currentLookAtObject = hitObject;
                            currentOutline = hitObject.GetComponent<Outline>();

                            if (currentOutline == null)
                            {
                                currentOutline = hitObject.AddComponent<Outline>();
                                currentOutline.OutlineMode = outlineMode;
                                currentOutline.OutlineColor = outlineColor;
                                currentOutline.OutlineWidth = outlineWidth;
                            }

                            currentOutline.enabled = true;
                        }
                        pickupUI.SetActive(true);
                    } else
                    {
                        // se o jogador não estiver a olhar para um objeto com a tag desejada,
                        // desativar outline no objeto atual
                        if (currentOutline != null)
                        {
                            currentOutline.enabled = false;
                        }

                        currentLookAtObject = null;
                        currentOutline = null;
                    }
                } else
                {
                    // se o raycast não acertar nenhum objeto, desativar o outline
                    // no objeto atual
                    if (currentOutline != null)
                    {
                        currentOutline.enabled = false;
                    }

                    currentLookAtObject = null;
                    currentOutline = null;
                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (heldObj == null)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickupRange))
                    {
                        PickupObject(hit.transform.gameObject);
                        picked = true;
                        pickupUI.SetActive(false);
                    }
                }
                else
                {
                    DropObject();
                    picked = false;
                    pickupUI.SetActive(true);
                }
            }
        } 
        else
        {
            pickupUI.SetActive(false);
        }

        if (!move && heldObj != null)
        {
            move = true;
        }
        else
        {
            move = false;
        }
        */
    }

    private void FixedUpdate()
    {
        if (move)
        {
            MoveObject();
        }
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdArea.position) > 0.1f)
        {
            Vector3 moveDirection = (holdArea.position - heldObj.transform.position);
            heldObjRB.AddForce(moveDirection * pickupForce);
        }
    }

    void PickupObject(GameObject pickObj)
    {
        if (pickObj.GetComponent<Rigidbody>())
        {
            heldObjRB = pickObj.GetComponent<Rigidbody>();
            heldObjRB.useGravity = false;
            heldObjRB.drag = 10;
            heldObjRB.constraints = RigidbodyConstraints.FreezeRotation;

            heldObjRB.transform.parent = holdArea;
            heldObj = pickObj;
        }
    }

    void DropObject()
    {     
        heldObjRB.useGravity = true;
        heldObjRB.drag = 1;
        heldObjRB.constraints = RigidbodyConstraints.None;

        heldObjRB.transform.parent = null;
        heldObj = null;
    }
}
