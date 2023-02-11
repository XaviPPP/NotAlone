using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[HideScriptField]
public class FallDamage : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float minFallVelocity = 15f;

    [Header("Audio")]
    [SerializeField] private AudioClip fallDeathClip;
    [SerializeField] private AudioClip windClip;

    [Header("UI")]
    [SerializeField] private GameObject canvasDeath;
    [SerializeField] private GameObject deathFade;

    private DamageController damageController;
    private CharacterController controller;
    private PlayerMovement playerMovement;
    private SurvivalManager survivalManager;

    private bool isGoingToTakeFallDamage;
    private bool playWindClip;

    void Start()
    {
        damageController = GetComponent<DamageController>();
        controller = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        survivalManager = GetComponent<SurvivalManager>();

        isGoingToTakeFallDamage = false;
        playWindClip = false;
    }

    void Update()
    {
        //Debug.Log(controller.velocity.y);
        if ((controller.velocity.y < -minFallVelocity))
        {
            //Debug.Log("Falling");
            isGoingToTakeFallDamage = true;
            damageController.GetAccumulatedFallDamage(controller.velocity.y);

            if (!playWindClip)
            {
                AudioManager.instance.PlayWindClip(windClip);
                playWindClip = true;
            }

            if (damageController.GetFinalFallDamage() >= 100f)
            {
                GetComponent<DeathManager>().PlayerDied(DeathReasons.FALL);
            }
        }

        if (playerMovement.groundedPlayer && isGoingToTakeFallDamage)
        {
            AudioManager.instance.StopPlayingWindClip();
            damageController.ApplyAccumulatedFallDamage();
            damageController.ResetAccumulatedFallDamage(); 

            if (survivalManager.GetCurrentHealth() <= 0f)
            {
                canvasDeath.SetActive(true);
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
        AudioManager.instance.PlayDeathClip(fallDeathClip);

        yield return new WaitWhile(() => AudioManager.instance.GetDeathAudioSource().isPlaying);

        Cursor.lockState = CursorLockMode.None;
        LevelManager.LoadLevel("DeathMenu");
    }
}
