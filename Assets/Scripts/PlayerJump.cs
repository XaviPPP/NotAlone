using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private Animator animator;
    private Vector3 velocity;
    private bool groundedPlayer;
    private bool jumped;
    private bool isJumping;
    [SerializeField] private float jumpHeight = 3.0f;
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

        if ((isJumping && velocity.y < 0) || velocity.y < -2)
        {
            animator.SetBool(isFallingHash, true);
        }

        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        Debug.Log($"Grounded: {groundedPlayer}");
        Debug.Log($"Player velocity: {velocity.y}");
        Debug.Log($"Controller velocity: {controller.velocity.y}");
    }
}
