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

    [SerializeField] private AudioClip fallDeathClip;
    [SerializeField] private AudioClip windClip;
    [SerializeField] private GameObject deathFade;

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
            //Debug.Log("Took damage");
            damageController.ApplyAccumulatedFallDamage();
            damageController.ResetAccumulatedFallDamage();
            AudioManager.instance.StopPlayingWindClip();

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
        AudioManager.instance.PlayDeathClip(fallDeathClip);

        yield return new WaitWhile(() => AudioManager.instance.GetDeathAudioSource().isPlaying);

        LevelManager.LoadLevel("DeathMenu");
    }
}
