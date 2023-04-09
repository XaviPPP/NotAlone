using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

[HideMonoScript]
public class FallDamage : MonoBehaviour
{
    [Title("Properties")]
    [Indent][SerializeField] private float minFallVelocity = 15f;

    [Title("Audio")]
    [Indent][SerializeField] private AudioClip fallDeathClip;
    [Indent][SerializeField] private AudioClip windClip;

    [Title("UI")]
    [Indent][SerializeField] private GameObject canvasDeath;
    [Indent][SerializeField] private GameObject deathFade;

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
        
        if (playerMovement.isFalling && playerMovement.velocity.y < -minFallVelocity)
        {
            Debug.Log(playerMovement.velocity.y);
            isGoingToTakeFallDamage = true;
            damageController.GetAccumulatedFallDamage(playerMovement.velocity.y);

            if (!playWindClip)
            {
                AudioManager.instance.PlayWindClip(windClip);
                playWindClip = true;
            }

            if (damageController.GetFinalFallDamage() >= survivalManager.GetCurrentHealth())
            {
                DeathManager.instance.PlayerDied(DeathReasons.FALL);
            }
        }

        if (playerMovement.isGrounded && isGoingToTakeFallDamage)
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
        LevelManager.instance.LoadLevel("DeathMenu");
    }
}
