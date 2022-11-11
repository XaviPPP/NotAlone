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
    public static bool isWalking;
    public static bool canSprint;

    public static bool jumped;
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

        isWalking = true;
        isSprinting = false;
        characterController.Move(move * speed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (canSprint)
            {
                if (z != 0)
                {
                    isWalking = false;
                    isSprinting = true;
                    characterController.Move(move * sprintSpeed * Time.deltaTime);
                } 
            }
        }
        

        if (Input.GetButtonDown("Jump") 
            && isGrounded 
            && GetComponent<SurvivalManager>().GetCurrentStamina() > GetComponent<SurvivalManager>().GetStaminaToJump())
        {
            if (!jumped)
            {
                Jump();
                jumped = true;
            }  
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
}
