using Cinemachine;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[HideMonoScript]
public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    //animator variables
    int isJumpingHash;
    int isFallingHash;
    int isGroundedHash;


    private SurvivalManager survivalManager;


    [Title("Character")]
    [Indent, SerializeField] private CharacterController controller;
    [Indent, SerializeField] private CinemachineVirtualCamera virtualCam;


    [Title("Camera")]
    [Indent, SerializeField] private float walkShakeAmount = 1f;
    [Indent, SerializeField] private float runShakeAmount = 1.5f;
    [Indent, SerializeField] private float crouchShakeAmount = 0.5f;


    [Title("Movement Settings")]
    [Indent, SerializeField] private float normalSpeed = 12f;
    [Indent, SerializeField] private float runSpeed = 12f;

    //"private" variables
    [HideInInspector] public Vector3 velocity;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool canRun;
    private Vector3 movementDirection;
    private Vector3 velocityNew;
    private bool isMoving;


    [Title("Jump Settings")]
    [Indent, SerializeField] private float jumpHeight = 3.0f;
    [Indent, SerializeField] private float jumpHorizontalSpeed;
    [Indent, SerializeField] private float gravityValue = -9.81f;
    [Indent] public Transform groundCheck;
    [Indent] public float groundDistance = 0.4f;

    //"private" variables
    [HideInInspector] public bool jumped;
    public bool isJumping;
    public bool isGrounded;
    private bool isFalling;
    private float maxVelocityY = 0f;


    [Title("Crouch Settings")]
    [Indent, SerializeField] private float normalHeight;
    [Indent, SerializeField] private float crouchHeight;
    [Indent, SerializeField] private float timeToCrouch = 0.25f;
    [Indent, SerializeField] private Vector3 normalCenter;
    [Indent, SerializeField] private Vector3 crouchCenter;
    [Indent, SerializeField] private float crouchSpeed;
    [SerializeField] private bool isCrouching;


    [Title("Masks")]
    [Indent] public MasksClass masks;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        survivalManager = GetComponent<SurvivalManager>();
        //cameraShake = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

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
        bool crouchPressed = Input.GetKey(KeyCode.LeftControl);

        isMoving = (forwardPressed || backwardsPressed || leftPressed || rightPressed) && (!isJumping || !isFalling);
        isRunning = (forwardPressed || backwardsPressed || leftPressed || rightPressed) && runPressed && survivalManager.CanRun();
        isCrouching = crouchPressed && isGrounded;

        Move();

        MoveWhileJumping(forwardPressed, backwardsPressed, leftPressed, rightPressed);

        HandleCrouch();

        SetGrounded();

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, masks.groundMask) || Physics.CheckSphere(groundCheck.position, groundDistance, masks.objMetalMask)
            || Physics.CheckSphere(groundCheck.position, groundDistance, masks.objWoodMask) || Physics.CheckSphere(groundCheck.position, groundDistance, masks.objRockMask);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (!jumped && survivalManager.CanJump() /*&& jumpDelayCounter >= jumpDelay*/)
            {
                Jump();
            }
        }

        isFalling = (isJumping && velocity.y < maxVelocityY) || (!isCrouching && velocity.y < -2f);

        if (isFalling)
        {
            animator.SetBool(isGroundedHash, false);
            animator.SetBool(isFallingHash, true);
        }

        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        HandleCameraShake();
    }

    private void SetGrounded()
    {
        if (isGrounded && velocity.y < 0f)
        {
            jumped = false;
            isJumping = false;
            velocity.y = -1f;
            animator.SetBool(isGroundedHash, true);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isFallingHash, false);
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        float speed = isCrouching ? crouchSpeed : isRunning ? runSpeed : normalSpeed;
        
        controller.Move(speed * Time.deltaTime * move);
    }

    private void Jump()
    {
        velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
        animator.SetBool(isJumpingHash, true);
        animator.SetBool(isGroundedHash, false);
        isJumping = true;
        jumped = true;
    }

    private void HandleCrouch()
    {
        controller.height = isCrouching ? crouchHeight : normalHeight;
        controller.center = isCrouching ? crouchCenter : normalCenter;

        Vector3 camPos = virtualCam.transform.localPosition;
        camPos.y = controller.height;

        virtualCam.transform.localPosition = camPos;
    }

    private void HandleCameraShake()
    {
        if (isMoving && !isRunning && !isCrouching)
            CameraController.instance.ShakeCamera(virtualCam, 1, walkShakeAmount);
        else if (isRunning && !isJumping && !isCrouching)
            CameraController.instance.ShakeCamera(virtualCam, 1, runShakeAmount);
        else if (isCrouching && isMoving)
            CameraController.instance.ShakeCamera(virtualCam, 1, crouchShakeAmount);
        else
            CameraController.instance.ShakeCamera(virtualCam, 0);
    }

    /*private IEnumerator CrouchStand()
    {
        float timeElapsed = 0f;
        float targetHeight = isCrouching ? normalHeight : crouchHeight;
        float currentHeight = controller.height;
        Vector3 targetCenter = isCrouching ? normalCenter : crouchCenter;
        Vector3 currentCenter = controller.center;

        while (timeElapsed < timeToCrouch)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;
    }*/

    private Vector3 GetMovementDirection(bool forwardPressed, bool backwardsPressed, bool leftPressed, bool rightPressed)
    {
        Vector3 direction = new();

        if (forwardPressed)
        {
            direction = transform.forward;
            if (leftPressed)
            {
                //Debug.Log("Forward and left");
                direction = transform.TransformDirection(new Vector3(-1, 0, 1));
            }
            else if (rightPressed)
            {
                //Debug.Log("Forward and right");
                direction = transform.TransformDirection(new Vector3(1, 0, 1));
            }
        }
        else if (backwardsPressed)
        {
            direction = -transform.forward;
            if (leftPressed)
            {
                direction = transform.TransformDirection(new Vector3(-1, 0, -1));
            }
            else if (rightPressed)
            {
                direction = transform.TransformDirection(new Vector3(1, 0, -1));
            }
        }
        else if (leftPressed)
        {
            direction = -transform.right;
        }
        else if (rightPressed)
        {
            direction = transform.right;
        }

        return direction;
    }

    private void MoveWhileJumping(bool forwardPressed, bool backwardsPressed, bool leftPressed, bool rightPressed)
    {
        if (isJumping)
        {
            if (maxVelocityY < velocity.y)
            {
                maxVelocityY = velocity.y;
            }

            movementDirection = GetMovementDirection(forwardPressed, backwardsPressed, leftPressed, rightPressed);

            velocityNew = movementDirection * jumpHorizontalSpeed;


            controller.Move(jumpHorizontalSpeed * Time.deltaTime * velocityNew);

        }
    }

    [Serializable]
    public class MasksClass
    {
        public LayerMask groundMask;
        public LayerMask objMetalMask;
        public LayerMask objWoodMask;
        public LayerMask objRockMask;
    }
}
