using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{

    public GameObject Inventory;
    public GameObject FirstPersonPlayer;
    public Camera MainCamera;
    public bool isClosed;

    // Start is called before the first frame update
    void Start()
    {
        isClosed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("i"))
        {
            if (isClosed)
            {
                Inventory.SetActive(true);
                isClosed = false;

                Cursor.lockState = CursorLockMode.None;
                MainCamera.GetComponent<MouseLook>().enabled = false;
                FirstPersonPlayer.GetComponent<PlayerMovement>().enabled = false;
            }
            else
            {
                Inventory.SetActive(false);
                isClosed = true;

                Cursor.lockState = CursorLockMode.Locked;
                MainCamera.GetComponent<MouseLook>().enabled = true;
                FirstPersonPlayer.GetComponent<PlayerMovement>().enabled = true;
            }
        }
    }
}
