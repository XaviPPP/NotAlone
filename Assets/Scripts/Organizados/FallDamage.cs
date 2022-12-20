using HFPS.Systems;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class FallDamage : MonoBehaviour
{
    [SerializeField] private float minFallVelocity = 15f;

    private DamageController damageController;
    private CharacterController controller;
    private PlayerMovement playerMovement;
    private SurvivalManager survivalManager;
    private Animator animator;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fallDeathClip;
    [SerializeField] private AudioClip boneBreakingClip;

    [SerializeField] private GameObject deathFade;

    private bool isGoingToTakeFallDamage;
    private int isDeadHash;

    void Start()
    {
        damageController = GetComponent<DamageController>();
        controller = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        survivalManager = GetComponent<SurvivalManager>();
        animator = GetComponent<Animator>();

        isGoingToTakeFallDamage = false;

        isDeadHash = Animator.StringToHash("isDead");
    }

    void Update()
    {
        //Debug.Log(controller.velocity.y);
        if ((controller.velocity.y < -minFallVelocity))
        {
            //Debug.Log("Falling");
            isGoingToTakeFallDamage = true;
            damageController.GetAccumulatedFallDamage(controller.velocity.y);

            if (damageController.GetFinalFallDamage() >= 100f)
            {
                GetComponent<DeathManager>().PlayerDied(DeathReasons.FALL);
            }
        }

        if (playerMovement.groundedPlayer && isGoingToTakeFallDamage)
        {
            //Debug.Log("Took damage");
            damageController.ApplyAccumulatedFallDamage();
            damageController.ResetAccumulatedFallDamage();

            if (survivalManager.GetCurrentHealth() <= 0f)
            {
                deathFade.GetComponent<Animator>().enabled = false;
                deathFade.GetComponent<Image>().color = new Color(0, 0, 0, 255);
                deathFade.SetActive(true);

                StartCoroutine(WaitForAudioAndLoadLevel());
            }

            isGoingToTakeFallDamage = false;
        }
    }

    IEnumerator WaitForAudioAndLoadLevel()
    {
        AudioManager.instance.PlayClip(audioSource, fallDeathClip, 1f);

        yield return new WaitWhile(() => audioSource.isPlaying);

        LevelManager.LoadLevel("DeathMenu");
    }
}
