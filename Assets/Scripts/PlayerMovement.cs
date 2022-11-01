using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController characterController;

    public float speed = 12f;
    public float sprintSpeed = 24f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;
    public static bool isSprinting;

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        moveNormally(move);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (GetComponent<SurvivalManager>().getCurrentStamina() > 0)
            {
                moveSprinting(move);
            }
        }

        

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void moveNormally(Vector3 move)
    {
        characterController.Move(move * speed * Time.deltaTime);
        isSprinting = false;
    }

    private void moveSprinting(Vector3 move)
    {
        characterController.Move(move * sprintSpeed * Time.deltaTime);
        isSprinting = true;
    }
}
