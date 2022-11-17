using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldAnimationStateController : MonoBehaviour
{
    private Animator animator;
    int isWalkingHash;
    int isRunningHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        bool canRun = PlayerMovement.canSprint;

        //se o jogador estiver a pressionar a tecla w
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }

        //se o jogador não estiver a pressionar a tecla w
        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        //se o jogador estiver a caminhar e não a correr e pressionar shift esquerdo
        if (!isRunning && (forwardPressed && runPressed) && canRun)
        {
            animator.SetBool(isRunningHash, true);
        }

        //se o jogador estiver a correr e parar de correr ou parar de caminhar
        if (isRunning && (!forwardPressed || !runPressed || !canRun))
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}
