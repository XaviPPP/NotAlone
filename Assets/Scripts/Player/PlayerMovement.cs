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
    //private Animator animator;

    //animator variables
    //int isJumpingHash;
    //int isFallingHash;
    //int isGroundedHash;


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
    [Indent] public Transform roofCheck;
    [Indent] public float groundDistance = 0.4f;
    [Indent] public float roofDistance = 0.4f;


    //"private" variables
    [HideInInspector] public bool jumped;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isFalling;
    //private bool isTouchingRoof;
    private float maxVelocityY = 0f;
    private float groundedTimer;


    [Title("Crouch Settings")]
    [Indent, SerializeField] private float normalHeight;
    [Indent, SerializeField] private float crouchHeight;
    [Indent, SerializeField] private float timeToCrouch = 0.25f;
    [Indent, SerializeField] private Vector3 normalCenter;
    [Indent, SerializeField] private Vector3 crouchCenter;
    [Indent, SerializeField] private float crouchSpeed;
    private bool isCrouching;


    [Title("Masks")]
    [Indent] public MasksClass masks;

    bool noClip = false;



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
        survivalManager = GetComponent<SurvivalManager>();
        //cameraShake = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        //isJumpingHash = Animator.StringToHash("isJumping");
        //isFallingHash = Animator.StringToHash("isFalling");
        //isGroundedHash = Animator.StringToHash("isGrounded");
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

        if (Input.GetKeyDown(KeyCode.N))
        {
            noClip = !noClip;
        }

        if (noClip)
        {
            GetComponent<CharacterController>().enabled = false;
            HandleNoclipMovement();
        }
        else
        {
            GetComponent<CharacterController>().enabled = true;
            HandleNormalMovement(forwardPressed, backwardsPressed, leftPressed, rightPressed);
        }


    }

    private void HandleNormalMovement(bool forwardPressed, bool backwardsPressed, bool leftPressed, bool rightPressed)
    {
        Move();

        MoveWhileJumping(forwardPressed, backwardsPressed, leftPressed, rightPressed);

        HandleCrouch();

        SetGrounded();

        velocity.y += gravityValue * Time.deltaTime;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, masks.groundMask) || Physics.CheckSphere(groundCheck.position, groundDistance, masks.objMetalMask)
            || Physics.CheckSphere(groundCheck.position, groundDistance, masks.objWoodMask) || Physics.CheckSphere(groundCheck.position, groundDistance, masks.objRockMask);

        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance);

        //Debug.Log(isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            if (!jumped && survivalManager.CanJump())
            {
                Jump();
            }
        }

        //isFalling = !isGrounded && ((isJumping && velocity.y < maxVelocityY) || (!isCrouching && velocity.y < -2f));

        isFalling = !isGrounded && velocity.y < maxVelocityY;

        /*if (isFalling)
        {
            animator.SetBool(isGroundedHash, false);
            animator.SetBool(isFallingHash, true);
        }*/

        controller.Move(velocity * Time.deltaTime);
        //controller.Move(speed * Time.deltaTime * move);

        /*if (isTouchingRoof)
        {
            Debug.Log("Touched");
        }*/

        HandleCameraShake();
    }

    private void HandleNoclipMovement()
    {
        isFalling = false;

        Vector3 cameraForward = virtualCam.transform.forward;
        cameraForward.y = 0; // Make sure the vector is horizontal

        // Move the player based on input and camera direction
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movementVector = (horizontal * Camera.main.transform.right + vertical * cameraForward).normalized * 100f * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            movementVector += Vector3.up * 50f * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            movementVector -= Vector3.up * 50f * Time.deltaTime;
        }

        // Apply deceleration if not moving
        if (movementVector.magnitude < 0.1f && velocity.magnitude > 0.1f)
        {
            movementVector = -velocity.normalized * 0f;
        }

        transform.position += movementVector;
    }

    private void SetGrounded()
    {
        if (isGrounded && velocity.y < 0f)
        {
            jumped = false;
            isJumping = false;
            velocity.y = 0f;
            //animator.SetBool(isGroundedHash, true);
            //animator.SetBool(isJumpingHash, false);
            //animator.SetBool(isFallingHash, false);
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
        velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        //animator.SetBool(isJumpingHash, true);
        //animator.SetBool(isGroundedHash, false);
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
