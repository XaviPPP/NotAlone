using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerMovement : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera mainCamera;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip windClip;

    private bool playedAudioClip = false;

    private Animator animator;
    [HideInInspector]
    public Vector3 velocity;
    private Vector3 movementDirection;
    private Vector3 velocityNew;

    [HideInInspector]
    public bool groundedPlayer;
    bool jumped;
    public bool isJumping;
    private bool jumpInOneDirection = false;
    public static bool isMoving;
    private float maxVelocityY = 0f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float jumpHorizontalSpeed;
    [SerializeField] private float gravityValue = -9.81f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    [HideInInspector]
    public float beginJumpTime;

    [Header("Masks")]
    public LayerMask groundMask;
    public LayerMask objMetalMask;
    public LayerMask objWoodMask;
    public LayerMask objRockMask;

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

        if (isJumping)
        {
            if (maxVelocityY < velocity.y)
            {
                maxVelocityY = velocity.y;
            }

            if (!jumpInOneDirection)
            {
                animator.applyRootMotion = false;

                movementDirection = GetMovementDirection(forwardPressed, backwardsPressed, leftPressed, rightPressed);

                velocityNew = movementDirection * jumpHorizontalSpeed;
            }

            controller.Move(velocityNew * jumpHorizontalSpeed * Time.deltaTime);

            jumpInOneDirection = true;

        } else
        {
            animator.applyRootMotion = true;
            jumpInOneDirection = false;
        }

        groundedPlayer = (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) || Physics.CheckSphere(groundCheck.position, groundDistance, objMetalMask)
            || Physics.CheckSphere(groundCheck.position, groundDistance, objWoodMask) || Physics.CheckSphere(groundCheck.position, groundDistance, objRockMask));

        if (groundedPlayer && velocity.y < 0)
        {
            jumped = false;
            isJumping = false;
            velocity.y = -1f;
            animator.SetBool(isGroundedHash, true);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isFallingHash, false);
            audioSource.Stop();
            playedAudioClip = false;
        }

        if (Input.GetKeyDown(KeyCode.Space) && groundedPlayer)
        {
            if (!jumped)
            {
                velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
                animator.SetBool(isJumpingHash, true);
                animator.SetBool(isGroundedHash, false);
                audioSource.Stop();
                isJumping = true;
                jumped = true;
                beginJumpTime = Time.time;
            }
        }

        if ((isJumping && velocity.y < maxVelocityY) || velocity.y < -2f)
        {
            animator.SetBool(isGroundedHash, false);
            animator.SetBool(isFallingHash, true);
        }

        if (!playedAudioClip && velocity.y < -10f)
        {
            StartCoroutine(AudioFader.FadeIn(audioSource, windClip, 3f));
            playedAudioClip = true;
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
                //Debug.Log("Forward and left");
                direction = transform.TransformDirection(new Vector3(-1, 0, 1));
            } else if (rightPressed)
            {
                //Debug.Log("Forward and right");
                direction = transform.TransformDirection(new Vector3(1, 0, 1));
            }
        } else if (backwardsPressed)
        {
            direction = -transform.forward;
            if (leftPressed)
            {
                direction = transform.TransformDirection(new Vector3(-1, 0, -1));
            } else if (rightPressed)
            {
                direction = transform.TransformDirection(new Vector3(1, 0, -1));
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
