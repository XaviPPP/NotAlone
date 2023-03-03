using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class MouseLook : MonoBehaviour
{
    public float mouseSens = 100f;

    public Transform playerBody;
    public Transform playerHead;
    public bool enableHeadMove;

    float xRotation = 0f;
    float rotation = 0f;

    // Start is called before the first frame update
    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 75f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);       
    }

    private void LateUpdate()
    {
        if (enableHeadMove)
        {
            playerHead.transform.rotation = transform.rotation;
        }
    }
}
