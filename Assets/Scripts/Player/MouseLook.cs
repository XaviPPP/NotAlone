using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Cinemachine;

public class MouseLook : MonoBehaviour
{
    private CinemachineVirtualCamera cam;

    public float mouseSens = 100f;

    public Transform playerBody;
    public Transform playerHead;

    public float minX = -90f;
    public float maxX = 90f;

    float xRotation = 0f;

    Vector3 lastCameraRotation;


    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }


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

        playerBody.transform.LookAt(null);

        xRotation -= mouseY;

        if (cam.LookAt != null)
        {
            lastCameraRotation = cam.transform.eulerAngles;
            playerBody.transform.LookAt(new Vector3(cam.LookAt.transform.position.x, playerBody.transform.position.y, cam.LookAt.transform.position.z));
            xRotation = WrapAngle(lastCameraRotation.x);
        }

        xRotation = Mathf.Clamp(xRotation, minX, maxX);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);        
    }

    private static float WrapAngle(float angle)
    {
        // Make sure that we get value between (-360, 360], we cannot use here module of 180 and call it a day, because we would get wrong values
        angle %= 360;
        if (angle > 180)
        {
            // If we get number above 180 we need to move the value around to get negative between (-180, 0]
            return angle - 360;
        }
        else if (angle < -180)
        {
            // If we get a number below -180 we need to move the value around to get positive between (0, 180]
            return angle + 360;
        }
        else
        {
            // We are between (-180, 180) so we just return the value
            return angle;
        }
    }
}
