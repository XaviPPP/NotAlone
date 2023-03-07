using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HideMonoScript]
public class AnimationStateController : MonoBehaviour
{   
    [Title("Properties")]
    [Indent] public float acceleration = 2.0f;
    [Indent] public float deceleration = 2.0f;
    [Indent] public float maximumWalkVelocity = 0.5f;
    [Indent] public float maximumRunVelocity = 2.0f;

    private float velocityZ = 0.0f;
    private float velocityX = 0.0f;

    private Animator animator;
    private SurvivalManager survivalManager;

    int velocityZHash;
    int velocityXHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        survivalManager = GetComponent<SurvivalManager>();

        velocityZHash = Animator.StringToHash("Velocity Z");
        velocityXHash = Animator.StringToHash("Velocity X");
    }

    //lida com a acelera��o e desacelera��o
    void ChangeVelocity(bool forwardPressed, bool backwardsPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        //se o jogador carregar na tecla para a frente, aumentar velocidade na dire��o Z
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        // se o jogador carregar na tecla para tr�s, diminuir velocidade z
        if (backwardsPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        //aumentar velocidade na dire��o esquerda
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //aumentar velocidade na dire��o direita
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //diminuir velocidadeZ
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        //aumentar velocidadeZ
        if (!backwardsPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }

        //aumentar velocidadeX se esquerda n�o estiver pressionada e velocidadeX < 0
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }

        //diminuir velocidadeX se direita n�o estiver pressionado e velocidadeX > 0
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }

    void LockOrResetVelocity(bool forwardPressed, bool backwardsPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        if (!forwardPressed && !backwardsPressed && velocityZ != 0 && velocityZ > -0.05f && velocityZ < 0.05f)
        {
            velocityZ = 0.0f;
        }

        if (!leftPressed && !rightPressed && velocityX != 0.0f && velocityX > -0.05f && velocityX < 0.05f)
        {
            velocityX = 0.0f;
        }

        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        if (backwardsPressed && runPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ = -currentMaxVelocity;
        }
        else if (backwardsPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;
            if (velocityZ < -currentMaxVelocity && velocityZ > (-currentMaxVelocity - 0.05f))
            {
                velocityZ = -currentMaxVelocity;
            }
        }
        else if (backwardsPressed && velocityZ > -currentMaxVelocity && velocityZ < (-currentMaxVelocity + 0.05f))
        {
            velocityZ = -currentMaxVelocity;
        }

        //bloquear esquerda
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        //desacelerar para a velocidade m�xima de caminhar
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            //arredondar para a currentMaxVelocity se dentro do offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        //arredondar para a currentMaxVelocity se dentro do offset
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.05f))
        {
            velocityX = -currentMaxVelocity;
        }

        //bloquear direita
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        //desacelerar para a velocidade m�xima de caminhar
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            //arredondar para a currentMaxVelocity se dentro do offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        //arredondar para a currentMaxVelocity se dentro do offset
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool backwardsPressed = Input.GetKey(KeyCode.S);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);       

        runPressed &= survivalManager.CanRun();

        float currentMaxVelocity = runPressed ? maximumRunVelocity : maximumWalkVelocity;

        ChangeVelocity(forwardPressed, backwardsPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);
        LockOrResetVelocity(forwardPressed, backwardsPressed, leftPressed, rightPressed, runPressed, currentMaxVelocity);

        animator.SetFloat(velocityZHash, velocityZ);
        animator.SetFloat(velocityXHash, velocityX);
    }
}
