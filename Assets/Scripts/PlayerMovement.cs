using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private Animator animator;
    private Vector3 velocity;
    private Vector3 movementDirection;

    private bool groundedPlayer;
    private bool jumped;
    private bool isJumping;
    public static bool isMoving;
    private float maxVelocityY = 0f;
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float jumpHorizontalSpeed;
    [SerializeField] private float gravityValue = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    int isJumpingHash;
    int isFallingHash;
    int isGroundedHash;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        isJumpingHash = Animator.StringToHash("isJumping");
        isFallingHash = Animator.StringToHash("isFalling");
        isGroundedHash = Animator.StringToHash("isGrounded");
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardsPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        movementDirection = GetMovementDirection(forwardPressed, backwardsPressed, leftPressed, rightPressed);

        if (isJumping)
        {
            if (maxVelocityY < velocity.y)
            {
                maxVelocityY = velocity.y;
            }

            animator.applyRootMotion = false;

            //movementDirection = GetMovementDirection(forwardPressed, backwardsPressed, leftPressed, rightPressed);

            Vector3 velocityNew = movementDirection * jumpHorizontalSpeed;

            controller.Move(velocityNew * jumpHorizontalSpeed * Time.deltaTime);
        } else
        {
            animator.applyRootMotion = true;
        }

        groundedPlayer = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (groundedPlayer && velocity.y < 0)
        {
            jumped = false;
            isJumping = false;
            velocity.y = -1f;
            animator.SetBool(isGroundedHash, true);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isFallingHash, false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            if (!jumped)
            {
                velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
                animator.SetBool(isJumpingHash, true);
                animator.SetBool(isGroundedHash, false);
                isJumping = true;
                jumped = true;
            }
        }

        if ((isJumping && velocity.y < maxVelocityY) || velocity.y < -2f)
        {
            animator.SetBool(isGroundedHash, false);
            animator.SetBool(isFallingHash, true);
        }

        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //Debug.Log($"Player velocity: {velocity.y}");
        //Debug.Log($"Max velocity: {maxVelocityY}");
    }

    private Vector3 GetMovementDirection(bool forwardPressed, bool backwardsPressed, bool leftPressed, bool rightPressed)
    {
        Vector3 direction = new Vector3();

        if (forwardPressed)
        {
            direction = transform.forward;
            if (leftPressed)
            {
                Debug.Log("Forward and left");
                direction = transform.InverseTransformDirection(new Vector3(-1, 0, 1));
            } else if (rightPressed)
            {
                Debug.Log("Forward and right");
                direction = transform.InverseTransformDirection(new Vector3(1, 0, 1));
            }
        } else if (backwardsPressed)
        {
            direction = -transform.forward;
            if (leftPressed)
            {
                direction = transform.InverseTransformDirection(new Vector3(-1, 0, -1));
            } else if (rightPressed)
            {
                direction = transform.InverseTransformDirection(new Vector3(1, 0, -1));
            }
        } else if (leftPressed)
        {
            direction = -transform.right;
        } else if (rightPressed)
        {
            direction = transform.right;
        }

        return direction;
    }
}
