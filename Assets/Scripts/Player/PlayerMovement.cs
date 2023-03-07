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
    [Title("Character")]
    [Indent][SerializeField] private CharacterController controller;
    [Indent][SerializeField] private CinemachineVirtualCamera virtualCam;

    [Title("Camera")]
    [Indent][SerializeField] private float walkShakeAmount = 1f;
    [Indent][SerializeField] private float runShakeAmount = 1.5f;

    [Title("Settings")]
    [Indent][SerializeField] private float speed = 12f;
    [Indent][SerializeField] private float runSpeed = 12f;
    [Indent][SerializeField] private float jumpHeight = 3.0f;
    [Indent][SerializeField] private float jumpHorizontalSpeed;
    //[Indent][SerializeField] private float jumpDelay = 0.1f;
    //private float jumpDelayCounter = 0f;
    [Indent][SerializeField] private float gravityValue = -9.81f;
    [Indent] public Transform groundCheck;
    [Indent] public float groundDistance = 0.4f;
    [HideInInspector]
    public float beginJumpTime;

    [Title("Masks")]
    [Indent] public MasksClass masks;

    private Animator animator;
    private SurvivalManager survivalManager;

    [HideInInspector] public Vector3 velocity;
    private Vector3 movementDirection;
    private Vector3 velocityNew;
    [HideInInspector] public bool jumped;
    [HideInInspector] public bool isJumping;
    [HideInInspector] public bool isGrounded;
    private bool isFalling;
    private bool isMoving;
    [HideInInspector] public bool isRunning;
    [HideInInspector] public bool canRun;
    private float maxVelocityY = 0f;

    int isJumpingHash;
    int isFallingHash;
    int isGroundedHash;

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

        isMoving = (forwardPressed || backwardsPressed || leftPressed || rightPressed) && (!isJumping || !isFalling);
        isRunning = (forwardPressed || backwardsPressed || leftPressed || rightPressed) && runPressed && survivalManager.CanRun();

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (!isRunning)
            controller.Move(speed * Time.deltaTime * move);
        else if (isRunning)
            controller.Move(runSpeed * Time.deltaTime * move);


        MoveWhileJumping(forwardPressed, backwardsPressed, leftPressed, rightPressed);

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

        isFalling = (isJumping && velocity.y < maxVelocityY) || velocity.y < -2f;

        if (isFalling)
        {
            animator.SetBool(isGroundedHash, false);
            animator.SetBool(isFallingHash, true);
        }

        velocity.y += gravityValue * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);    

        if (isMoving && !isRunning)
            CameraController.instance.ShakeCamera(virtualCam, 1, walkShakeAmount);
        else if (isRunning && !isJumping)
            CameraController.instance.ShakeCamera(virtualCam, 1, runShakeAmount);
        else
            CameraController.instance.ShakeCamera(virtualCam, 0);

        //Debug.Log($"Player velocity: {velocity.y}");
        //Debug.Log($"Max velocity: {maxVelocityY}");
    }

    private void SetGrounded()
    {
        if (isGrounded && velocity.y < 0)
        {
            jumped = false;
            isJumping = false;
            velocity.y = -1f;
            animator.SetBool(isGroundedHash, true);
            animator.SetBool(isJumpingHash, false);
            animator.SetBool(isFallingHash, false);
            //if (jumpDelayCounter < jumpDelay)
            //    jumpDelayCounter += Time.deltaTime;
        }
    }

    private void Jump()
    {
        velocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
        animator.SetBool(isJumpingHash, true);
        animator.SetBool(isGroundedHash, false);
        isJumping = true;
        jumped = true;
        //jumpDelayCounter = 0;
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

    /*private void EnableCameraShake(bool state, float quantity)
    {
        cameraShake.m_AmplitudeGain = state ? quantity : 0;
    }*/

    [System.Serializable]
    public class MasksClass
    {
        public LayerMask groundMask;
        public LayerMask objMetalMask;
        public LayerMask objWoodMask;
        public LayerMask objRockMask;
    }
}
